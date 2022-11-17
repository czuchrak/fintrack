import React, {createContext, useContext, useEffect, useState} from "react";
import {Navigate} from "react-router-dom";
import {initializeApp} from "firebase/app";
import Loader from "../components/Loader";
import {deleteUser as bckDeleteUser, getUser, setMailVerificationSent,} from "src/services";
import {
    applyActionCode,
    browserLocalPersistence,
    browserSessionPersistence,
    confirmPasswordReset,
    createUserWithEmailAndPassword,
    deleteUser,
    EmailAuthProvider,
    getAuth,
    GoogleAuthProvider,
    onAuthStateChanged,
    reauthenticateWithCredential,
    sendEmailVerification,
    sendPasswordResetEmail,
    setPersistence,
    signInWithEmailAndPassword,
    signInWithPopup,
    updatePassword,
    verifyPasswordResetCode,
} from "firebase/auth";
import {firebaseConfig} from "../config/config";
import {appConfig} from "src/config/config";
import {initializeAppCheck, ReCaptchaV3Provider} from "firebase/app-check";
import {getAnalytics} from "firebase/analytics";
import {useDispatch} from "react-redux";
import {addError} from "src/redux/slices/errorSlice";
import {addProfile} from "src/redux/slices/profileSlice";
import {useReduxActions} from "../redux/store";

const demo = appConfig.demo;

if (!demo) {
  const app = initializeApp(firebaseConfig);
  getAnalytics();
  if (appConfig.prodMode)
    initializeAppCheck(app, {
      provider: new ReCaptchaV3Provider(appConfig.captchaKey),
      isTokenAutoRefreshEnabled: true,
    });
}

const authContext = createContext();

export function ProvideAuth({ children }) {
  const auth = useProvideAuth();

  return <authContext.Provider value={auth}>{children}</authContext.Provider>;
}

export function useAuth() {
  return useContext(authContext);
}

function useProvideAuth() {
  const dispatch = useDispatch();
  const reduxActions = useReduxActions();

  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState(null);
  let auth = getAuth();
  auth.languageCode = "pl";

  const getToken = async () => {
    return demo ? "" : await user.getIdToken();
  };

  const login = async (email, password, remember) => {
    reduxActions.reset();
    await setPersistence(
      auth,
      remember ? browserLocalPersistence : browserSessionPersistence
    );
    try {
      const result = await signInWithEmailAndPassword(auth, email, password);
      setUser(result.user);
    } catch (error) {
      if (
        error.code === "auth/wrong-password" ||
        error.code === "auth/user-not-found"
      ) {
        return error.code;
      }
      dispatch(addError(error.message));
    }
  };

  const register = async (email, password) => {
    try {
      await createUserWithEmailAndPassword(auth, email, password);
      await login(email, password, true);
    } catch (error) {
      if (error.code === "auth/email-already-in-use") {
        return error.code;
      }
      dispatch(addError(error.message));
    }
  };

  const verifyEmail = async () => {
    try {
      await sendEmailVerification(auth.currentUser);
      await setMailVerificationSent(await getToken());
      return true;
    } catch (error) {
      dispatch(addError(error.message));
      return false;
    }
  };

  const signInWithGoogle = async () => {
    try {
      reduxActions.reset();
      const googleProvider = new GoogleAuthProvider();
      const result = await signInWithPopup(auth, googleProvider);
      setUser(result.user);
    } catch (error) {
      dispatch(addError(error.message));
    }
  };

  const applyVerificationCode = async (code) => {
    try {
      await applyActionCode(auth, code);
      return true;
    } catch (error) {
      if (error.code === "auth/invalid-action-code") {
        return false;
      }
      dispatch(addError(error.message));
    }
  };

  const changePassword = async (oldPassword, newPassword) => {
    try {
      await reauthenticateWithCredential(
        user,
        EmailAuthProvider.credential(user.email, oldPassword)
      );
      await updatePassword(user, newPassword);
    } catch (error) {
      if (error.code === "auth/wrong-password") {
        return error.code;
      }
      dispatch(addError(error.message));
    }
  };

  const resetPassword = async (email) => {
    try {
      await sendPasswordResetEmail(auth, email);
    } catch (error) {
      if (error.code === "auth/user-not-found") {
        return error.code;
      }
      dispatch(addError(error.message));
    }
  };

  const verifyResetCode = async (code) => {
    try {
      await verifyPasswordResetCode(auth, code);
      return true;
    } catch (error) {
      return false;
    }
  };

  const setNewPasswordReset = async (code, newPassword) => {
    try {
      await confirmPasswordReset(auth, code, newPassword);
    } catch (error) {
      dispatch(addError(error.message));
    }
  };

  const deleteAccount = async (providerId, password) => {
    try {
      let credential;
      if (providerId === "google.com") {
        const googleProvider = new GoogleAuthProvider();
        const result = await signInWithPopup(auth, googleProvider);
        credential = GoogleAuthProvider.credentialFromResult(result);
      } else {
        credential = EmailAuthProvider.credential(user.email, password);
      }
      await reauthenticateWithCredential(user, credential);
      await bckDeleteUser(await getToken());
      await deleteUser(user);
    } catch (error) {
      if (error.code === "auth/wrong-password") {
        return error.code;
      }
      dispatch(addError(error.message));
    }
  };

  const refreshBckUser = async () => {
    let bckUser = await getUser(await user.getIdToken());
    dispatch(addProfile(bckUser));
  };

  const logout = () => {
    auth.signOut();
    setTimeout(() => {
      reduxActions.reset();
    }, 1000);
  };

  useEffect(() => {
    const unsubscribe = onAuthStateChanged(
      auth,
      async (user) => {
        if (user) {
          let bckUser = await getUser(await user.getIdToken());
          user.bck = {
            isAdmin: bckUser.isAdmin,
          };
          setUser(user);
          dispatch(addProfile(bckUser));
        } else {
          setUser(false);
        }
        setLoading(false);
      },
      (error) => {
        dispatch(addError(error.message));
        setLoading(false);
      }
    );

    return () => unsubscribe();
  }, [auth, dispatch]);

  return {
    user,
    getToken,
    login,
    register,
    verifyEmail,
    signInWithGoogle,
    applyVerificationCode,
    changePassword,
    resetPassword,
    verifyResetCode,
    setNewPasswordReset,
    deleteAccount,
    refreshBckUser,
    logout,
    loading,
  };
}

function PrivateRoute({ children, admin, mailVerification }) {
  let auth = useAuth();

  if (!demo && auth.loading)
    return (
      <>
        <Loader open={true} />
      </>
    );

  if (admin) {
    return !demo && auth.user && auth.user.bck && auth.user.bck.isAdmin ? (
      children
    ) : (
      <Navigate to={process.env.PUBLIC_URL + "/networth/dashboard"} replace />
    );
  }

  if (!demo && mailVerification && auth.user && !auth.user.emailVerified) {
    return (
      <Navigate
        to={process.env.PUBLIC_URL + "/onboarding/mailverification"}
        replace
      />
    );
  }

  return demo || auth.user ? (
    children
  ) : (
    <Navigate to={process.env.PUBLIC_URL + "/login"} replace />
  );
}

export default PrivateRoute;

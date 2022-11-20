export const firebaseConfig = {
  apiKey: process.env.REACT_APP_APIKEY,
  authDomain: process.env.REACT_APP_AUTHDOMAIN,
  projectId: process.env.REACT_APP_PROJECTID,
  storageBucket: process.env.REACT_APP_STORAGEBUCKET,
  messagingSenderId: process.env.REACT_APP_MESSAGINGSENDERID,
  appId: process.env.REACT_APP_APPID,
  measurementId: process.env.REACT_APP_MEASUREMENTID,
};

export const appConfig = {
  demo: process.env.REACT_APP_DEMO === "true",
  testApp: process.env.REACT_APP_TEST === "true",
  captchaKey: process.env.REACT_APP_CAPTCHAKEY,
  prodMode: process.env.REACT_APP_PROD_MODE === "true",
};

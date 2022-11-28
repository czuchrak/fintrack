import { Navigate, useRoutes } from "react-router-dom";
import PrivateRoute, { useAuth } from "./navigation/PrivateRoute";
import MainLayout from "./layouts/";
import EmptyLayout from "./layouts/EmptyLayout";
import Login from "./pages/authentication/Login";
import Home from "./pages/Home";
import Register from "./pages/authentication/Register";
import NetWorthDashboard from "./pages/netWorth/NetWorthDashboard";
import NetWorthParts from "./pages/netWorth/NetWorthParts";
import NetWorthData from "./pages/netWorth/NetWorthData";
import ResetPassword from "./pages/authentication/ResetPassword";
import Contact from "./pages/profile/Contact";
import Loader from "./components/Loader";
import { appConfig } from "src/config/config";
import Auth from "./pages/authentication/Auth";
import Settings from "./pages/profile/Settings";
import Terms from "./pages/Terms";
import Privacy from "./pages/Privacy";
import Logs from "./pages/admin/Logs";
import Users from "./pages/admin/Users";
import Admin from "./pages/admin/Admin";
import Notifications from "./pages/admin/Notifications";
import PropertyCategories from "./pages/admin/PropertyCategories";
import PropertySettings from "./pages/property/PropertySettings";
import PropertyDetails from "./pages/property/PropertyDetails";
import MailVerification from "./pages/onboarding/MailVerification";
import React from "react";
import ExchangeRates from "./pages/admin/ExchangeRates";
import NetWorthGoals from "./pages/netWorth/NetWorthGoals";

// ----------------------------------------------------------------------

export default function Router() {
  let auth = useAuth();
  const demo = appConfig.demo;
  const testApp = appConfig.testApp;

  const getUrl = (path) => {
    return process.env.PUBLIC_URL + path;
  };

  const CheckLoading = ({ children }) => {
    return demo ? (
      <Navigate to={getUrl("/networth/dashboard")} replace />
    ) : auth.loading ? (
      <Loader open={true} />
    ) : auth.user ? (
      <Navigate to={getUrl("/networth/dashboard")} replace />
    ) : (
      children
    );
  };

  const getPrivateChild = (path, element, mailVerification, admin) => {
    return {
      path: path,
      element: (
        <PrivateRoute mailVerification={mailVerification} admin={admin}>
          {element}
        </PrivateRoute>
      ),
    };
  };

  const getHomeChild = (path, element) => {
    return {
      path: path,
      element: demo ? (
        <Navigate to={getUrl("/networth/dashboard")} replace />
      ) : (
        element
      ),
    };
  };

  const getAuthChild = (path, element) => {
    return {
      path: path,
      element: <CheckLoading>{element}</CheckLoading>,
    };
  };

  const getSettingsChild = (path, element) => {
    return {
      path: path,
      element: demo ? (
        <Navigate to={getUrl("/networth/dashboard")} replace />
      ) : auth.user ? (
        <MainLayout />
      ) : (
        <EmptyLayout />
      ),
      children: [getPrivateChild("", element)],
    };
  };

  const getPrivateRoutes = (path, children, mailVerification, admin) => {
    return {
      path: getUrl(path),
      element: demo || auth.user ? <MainLayout /> : <EmptyLayout />,
      children: children.map((route) => {
        return getPrivateChild(
          route.path,
          route.element,
          mailVerification,
          admin,
          true
        );
      }),
    };
  };

  return useRoutes([
    {
      path: getUrl("/onboarding"),
      element: demo || auth.user ? <MainLayout /> : <EmptyLayout />,
      children: [
        { path: "", element: <Navigate to="mailverification" replace /> },
        {
          path: "mailverification",
          element:
            demo || testApp || (auth.user && auth.user.emailVerified) ? (
              <Navigate to={getUrl("/networth/dashboard")} replace />
            ) : (
              <PrivateRoute>
                <MailVerification />
              </PrivateRoute>
            ),
        },
      ],
    },
    getPrivateRoutes(
      "/networth",
      [
        { path: "", element: <Navigate to="dashboard" replace /> },
        { path: "dashboard", element: <NetWorthDashboard /> },
        { path: "data", element: <NetWorthData /> },
        { path: "goals", element: <NetWorthGoals /> },
        { path: "parts", element: <NetWorthParts /> },
      ],
      true
    ),
    getPrivateRoutes(
      "/property",
      [
        { path: "", element: <Navigate to="settings" replace /> },
        { path: ":id", element: <PropertyDetails /> },
        { path: "settings", element: <PropertySettings /> },
      ],
      true
    ),
    getPrivateRoutes(
      "/admin",
      [
        { path: "", element: <Admin /> },
        { path: "logs", element: <Logs /> },
        { path: "users", element: <Users /> },
        { path: "exchangeRates", element: <ExchangeRates /> },
        { path: "notifications", element: <Notifications /> },
        { path: "propertyCategories", element: <PropertyCategories /> },
      ],
      true,
      true
    ),
    {
      path: getUrl("/"),
      element: <EmptyLayout />,
      children: [
        getHomeChild("", <Home />),
        getHomeChild("terms", <Terms />),
        getHomeChild("privacy", <Privacy />),
        getHomeChild("auth", <Auth />),

        getAuthChild("login", <Login />),
        getAuthChild("register", <Register />),
        getAuthChild("resetpassword", <ResetPassword />),

        getSettingsChild("contact", <Contact />),
        getSettingsChild("settings", <Settings />),

        { path: "*", element: <Navigate to={getUrl("/")} replace /> },
      ],
    },
    { path: "*", element: <Navigate to={getUrl("/")} replace /> },
  ]);
}

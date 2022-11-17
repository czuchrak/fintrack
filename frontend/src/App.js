import Router from "./routes";
import ThemeConfig from "./theme";
import ScrollToTop from "./components/ScrollToTop";
import {ProvideAuth} from "./navigation/PrivateRoute";
import {BrowserRouter} from "react-router-dom";
import {appConfig} from "src/config/config";
import "src/utils/dropConsole";

export default function App() {
  const demo = appConfig.demo;

  return (
    <ThemeConfig>
      {demo ? (
        <BrowserRouter>
          <ScrollToTop />
          <Router />
        </BrowserRouter>
      ) : (
        <ProvideAuth>
          <BrowserRouter>
            <ScrollToTop />
            <Router />
          </BrowserRouter>
        </ProvideAuth>
      )}
    </ThemeConfig>
  );
}

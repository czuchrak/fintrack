<div align="center">
  <br>
  <a href="https://fintrack.app" target="_blank">
    <img src="https://fintrack.app/static/images/logo_background.png" alt="Fintrack.app" width="300" />
  </a>
  <br>
</div>

<p align="center"><b>Web application to track changes in your net worth</b></p>

<p align="center">
  <a href="https://github.com/czuchrak/fintrack/actions/workflows/dotnet.yml">
    <img src="https://github.com/czuchrak/fintrack/actions/workflows/dotnet.yml/badge.svg"
         alt=".NET">
  </a>
  <a href="https://github.com/czuchrak/fintrack/actions/workflows/codeql.yml">
    <img src="https://github.com/czuchrak/fintrack/actions/workflows/codeql.yml/badge.svg"
         alt="CodeQL">
  </a><br/>
  <a href="https://github.com/czuchrak/fintrack/actions/workflows/deployTest.yml">
    <img src="https://github.com/czuchrak/fintrack/actions/workflows/deployTest.yml/badge.svg"
         alt="DeployTest">
  </a>
  <a href="https://github.com/czuchrak/fintrack/actions/workflows/deployProd.yml">
    <img src="https://github.com/czuchrak/fintrack/actions/workflows/deployProd.yml/badge.svg"
         alt="DeployTest">
  </a>
</p>

## About

Fintrack.app is a web application that allows you to track changes in your net worth. Add assets and liabilities,
provide their current value regularly, set financial goals and view statistics and charts.

## Demo

See demo <a href="https://fintrack.app/demo" target="_blank">here</a>.

## Tech Stack

**Client:** React, Redux

**Server:** .NET 6

**Database:** MS SQL Server

## Run locally

1. Install <a href="https://git-scm.com" target="_blank">Git</a>, <a href="https://nodejs.org" target="_blank">
   Node.js</a>, <a href="https://www.docker.com" target="_blank">Docker</a>
   and <a href="https://dotnet.microsoft.com/en-us/download/dotnet/6.0" target="_blank">
   .NET 6</a> on your machine.

2. Clone the repo
   ```sh
   git clone https://github.com/czuchrak/fintrack.git
   ```
3. Go to the project directory
   ```sh
   cd fintrack
   ```
4. Run database
   ```sh
   docker-compose up --build
   ```
5. Run application
   ```sh
   cd backend/Fintrack.WebApi
   dotnet run
   ```
6. Open website
   ```html
   https://localhost:5001
   ```

## Contributing

See [contributing.md](https://github.com/czuchrak/fintrack/blob/main/CONTRIBUTING.md) for ways to get started.

Please adhere to this project's [code of conduct](https://github.com/czuchrak/fintrack/blob/main/CODE_OF_CONDUCT.md).

## License

Distributed under the AGPL-3.0 license. See [LICENSE](https://github.com/czuchrak/fintrack/blob/main/LICENSE) for more
information.

## Feedback

If you have any feedback, please reach out to us at contact@fintrack.app
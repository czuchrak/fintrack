<div align="center">
  <br>
  <a href="https://fintrack.app">
    <img style="background-color: rgb(300, 300, 300); padding: 10px" src="https://fintrack.app/static/logo.png" alt="Fintrack.app" width="200" />
  </a>
  <br>
</div>

<h4 align="center">Web application to track changes in your net worth</h4>

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

# About

Fintrack.app is a web application that allows you to track changes in your net worth. Add assets and liabilities,
provide their current value regularly, set financial goals and view statistics and charts.

## Demo

See demo [here](https://fintrack.app/demo).

## Tech Stack

**Client:** React, Redux

**Server:** .NET 6

**Database:** MS SQL Server

## Run locally

### Prerequisites

Install [Git](https://git-scm.com), [Node.js](https://nodejs.org/), [Docker](https://www.docker.com)
and [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) on your machine.

### Run

1. Clone the repo
   ```sh
   git clone https://github.com/czuchrak/fintrack.git
   ```
2. Go to the project directory
   ```sh
   cd fintrack
   ```
3. Run database
   ```sh
   docker-compose up --build
   ```
4. Run application
   ```sh
   cd backend/Fintrack.WebApi
   dotnet run
   ```
5. Open website
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
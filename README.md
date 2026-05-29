# cCoder.Scheduling

`cCoder.Scheduling` contains the Scheduling domain for the cCoder platform.

## Contents

- `src/cCoder.Scheduling`
  The main library package published to NuGet.
- `src/Scheduling.Web`
  The standalone web host for the Scheduling domain.
- `src/cCoder.Scheduling.Tests`
  Unit tests for the domain.
- `src/Scheduling.AcceptanceTests`
  Acceptance tests for the standalone host.

## Build

```powershell
dotnet build src/cCoder.Scheduling.sln -v minimal
```

## Test

```powershell
dotnet test src/cCoder.Scheduling.sln -v minimal --no-build
```

## Local Configuration

The standalone web host reads local secrets from environment variables rather than committed config.

Before running `src/Scheduling.Web`, set:

- `ConnectionStrings__Core`
- `ConnectionStrings__SSO`
- `Settings__DecryptionKey`

The committed `appsettings.json` keeps these values blank so user or machine environment variables can supply them during local development.

## Package

The NuGet package produced by this repository is:

- `cCoder.Scheduling`

## Publishing

GitHub Actions is configured to publish the main package using NuGet trusted publishing.

Before the first publish, configure a trusted publishing policy on nuget.org for:

- Repository owner: `ccoder-co-uk`
- Repository: `cCoder.Scheduling`
- Workflow file: `publish.yml`

The workflow also expects a `NUGET_USER` repository secret containing the nuget.org profile name used during trusted publishing login.

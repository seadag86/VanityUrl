# Vanity URL Generator

![Build Status](https://github.com/seadag86/VanityUrl/actions/workflows/ci.yml/badge.svg)

This is a simple ASP.NET Core WebAPI designed to generate shortened URLs using a random code or specified alias.

This repository makes use of the following libraries:

- [EF Core](https://docs.microsoft.com/en-us/ef/core/)
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file)
- [MediatR](https://github.com/jbogard/MediatR)
- [Carter](https://github.com/CarterCommunity/Carter)
- [xUnit](https://xunit.net)]

## Versions

``` http://localhost:<port>/swagger ```

## GET All Urls

``` http://localhost:<port>/api/urls ```

## GET Url by Alias (Redirect)

``` http://localhost:<port>/alias ```)

## POST Create a Url

``` http://localhost:<port>/api/urls ```

```
{
	"url": "https://www.google.com",
	"alias": "google"
}

```

## Building a sample

Build Vanity.Url project using the .NET Core CLI, which is installed with [the .NET Core SDK](https://www.microsoft.com/net/download). Then run
these commands from the CLI in the directory of any sample:

```console
dotnet build
dotnet run
```

These will install the dependencies, build the project, and run
the project respectively.

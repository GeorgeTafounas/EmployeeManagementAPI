# Employee Management API

A RESTful API built with ASP.NET Core 8 and Entity Framework Core for managing Employees and Departments.

## Tech Stack
- ASP.NET Core Web API (.NET 9)
- Entity Framework Core 9
- SQL Server (via Docker)
- Swagger UI

## Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Setup

### 1. Start SQL Server in Docker
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password123!" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest

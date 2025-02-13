# Microservices Team Performance Analysis

This repository contains the `microservices-team-performance-analysis` microservice, developed in **.NET 8** and using **PostgreSQL** as its database.

## Prerequisites

Before cloning and running this project, make sure you have the following installed:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [Git](https://git-scm.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download) (optional if running with Docker)

## Clone the Repository

```sh
git clone https://github.com/Anggeloo/microservices-team-performance-analysis.git
cd microservices-team-performance-analysis
```

## Environment Configuration

The database connection is already configured in `appsettings.json`. If you need to modify it, update the `ConnectionStrings` section.

## Running the Microservice

### Option 1: Using Docker

To run the service in a Docker container, use the following commands:

```sh
docker build -t microservices-team-performance .
docker run -p 90:90 -p 4000:4000 --env-file .env microservices-team-performance
```

### Option 2: Running Locally with .NET

If you prefer to run the service without Docker, follow these steps:

```sh
dotnet restore
dotnet run
```

The service will run on `http://localhost:90`.

## Available Endpoints

### Swagger API Documentation

Swagger is enabled in this microservice. You can access the API documentation at:

```
http://localhost:90/swagger/index.html
```

## Additional Notes

- The microservice uses PostgreSQL as its database, so ensure that the connection to the AWS database is available.
- If changes are made to the environment variables, you need to rebuild the container if using Docker.
## Authors
Cadena Anggelo and Caiza Katherine

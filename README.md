# Microservices Team Performance Analysis

## Description
This microservice analyzes team performance in the system using a REST API. It is built with .NET, utilizing PostgreSQL as the database.

## Prerequisites
Ensure you have the following installed:
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)

## Clone the Repository
To clone this project, run:
```sh
git clone https://github.com/Anggeloo/microservices-team-performance-analysis.git
cd microservices-team-performance-analysis
```

## Installation
1. Restore dependencies:
   ```sh
   dotnet restore
   ```

2. Build the service:
   ```sh
   dotnet build
   ```

3. Run the service:
   ```sh
   dotnet run
   ```

## Running with Docker
1. Build the Docker image:
   ```sh
   docker build -t microservices-team-performance-analysis .
   ```

2. Run the container:
   ```sh
   docker run -p 4200:4000 --env-file .env microservices-team-performance-analysis
   ```

3. Alternatively, use Docker Compose:
   ```sh
   docker-compose up --build
   ```

## API Documentation
This service provides API documentation via Swagger. Once the service is running, access it at:
```
http://localhost:5050/swagger/index.html
```

## Database Setup
Ensure you have a PostgreSQL database running. If using Docker, you can spin up a PostgreSQL container with:
```sh
docker run --name postgres-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=AWSPServices0099 -e POSTGRES_DB=db_team_performance_analysis -p 5432:5432 -d postgres
```

## Testing
Currently, no test scripts are defined. You may add tests and run them using:
```sh
dotnet test
```
## Authors
Cadena Anggelo and Caiza Katherine

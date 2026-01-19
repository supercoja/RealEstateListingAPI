# Real Estate Listing API

This document provides information on the Real Estate Listing API, including instructions for running it locally using Docker Compose and a section for documenting API changes.

## API Changes

I have updated the folders to organize each project in his own folder, its a pattern that I follow

I added new projects for Domain, Services and Infrastructure

Domain holds the logic, Services are for creating contracts - abstracting with interfaces - 
that later will be coupled with DI.

Infrastructure is to handle the DB and other Infra (like file manipulation in case needed).
Used a Generic repository and UnitOfWork Pattern here.

I also added a validation with DTO's (to avoid "polluting" the application domain)

Created the Envelope class to wrap all responses from API with a pattern. This allows
to not validate Model inside the controllers, cause a middleware will handle the messages.

I also created two Test projects, in order to Test/Validate the Domain and Also the Controllers (simple examples to validate Domain and Controllers with Tests)

This is basic the structure that I like to use to organize the Project.

## Getting Started with Docker Compose

This section outlines how to get the Real Estate Listing API up and running on your local machine using Docker Compose.

### Prerequisites

Before you begin, ensure you have the following installed:

*   **Docker Desktop**: Includes Docker Engine and Docker Compose. You can download it from [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop).

### Running the API with .net 8

1.  **Navigate to the project directory:**
    Open your terminal or command prompt and go to the directory containing the `docker-compose.yml` file (the `RealEstateListingAPI` directory):

    cd RealEstateListingAPI

2.  **Build and run the services:**
    Execute the following command to build the Docker image for the API and start the service in detached mode:

    docker-compose up --build -d
    ```
    *   `--build`: This flag ensures that the Docker image for the API is built (or re-built if changes have occurred) before starting the containers.
    *   `-d`: This flag runs the containers in detached mode, meaning they will run in the background.

3.  **Verify services are running:**
    You can check the status of your running services with:

    docker-compose ps

4.  **Access the API:**
    The Real Estate Listing API should now be accessible at `http://localhost:8080`.
    For demo purposes, the Swagger is enabled with docker (which is 'Prod', not Development)
    The Test for API can be accessed by http://localhost:8080/swagger

5.  ** See Logs in Docker:**
    You can check the logs for the container with:

    docker logs --follows <containerId>

### Stopping the Services

To stop and remove the running containers, networks, and volumes created by `docker-compose up`, navigate to the `RealEstateListingAPI` directory and run:

docker-compose down


## Getting Started with .NET CLI

This section outlines how to run the Real Estate Listing API directly using the .NET 8 SDK command-line interface.

### Prerequisites

Before you begin, ensure you have the following installed:

*   **.NET 8 SDK**: You can download it from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0).

### Running the API

1.  **Navigate to the root project directory:**
    Open your terminal or command prompt and go to the root `RealEstateListingAPI` directory:

    cd RealEstateListingAPI

2.  **Run the API project:**
    Execute the following command to build and run the API project:

    dotnet run --project src/RealEstateListingAPI/RealEstateListingApi.csproj
    ```

3.  **Access the API:**
    The API will typically run on `http://localhost:5000` or `https://localhost:5001` by default, but it will display the exact URLs in your console output when it starts. Look for messages like "Now listening on: http://localhost:XXXX".
    The Test for API can be accessed by http://localhost:8080/swagger

### Stopping the API

To stop the running API, simply press `Ctrl+C` in the terminal where it is running.
    

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY src/RealEstateListingAPI/RealStateListingAPI.sln src/RealEstateListingAPI/

COPY src/RealEstateListingAPI/ src/RealEstateListingAPI/
COPY src/RealEstateListing.Domain/ src/RealEstateListing.Domain/
COPY src/RealEstateListing.Infrastructure/ src/RealEstateListing.Infrastructure/
COPY src/RealEstateListing.Services/ src/RealEstateListing.Services/
COPY src/RealEstateListing.Mapping/ src/RealEstateListing.Mapping/
COPY src/RealEstateListing.Domain.Tests/ src/RealEstateListing.Domain.Tests/

RUN dotnet restore src/RealEstateListingAPI/RealStateListingAPI.sln

RUN dotnet build src/RealEstateListingAPI/RealEstateListingApi.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish src/RealEstateListingAPI/RealEstateListingApi.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateListingApi.dll"]

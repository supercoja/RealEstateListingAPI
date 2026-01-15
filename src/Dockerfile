FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy solution and project files
COPY ["RealEstateListingAPI/RealStateListingAPI.sln", "RealEstateListingAPI/"]
COPY ["RealEstateListingAPI/RealEstateListingApi.csproj", "RealEstateListingAPI/"]
COPY ["RealEstateListing.Common/RealEstateListing.Common.csproj", "RealEstateListing.Common/"]
COPY ["RealEstateListing.Infrastructure/RealEstateListing.Infrastructure.csproj", "RealEstateListing.Infrastructure/"]
COPY ["RealEstateListing.Services/RealEstateListing.Services.csproj", "RealEstateListing.Services/"]
COPY ["RealEstateListing.Domain/RealEstateListing.Domain.csproj", "RealEstateListing.Domain/"]

RUN dotnet restore "RealEstateListingAPI/RealStateListingAPI.sln"

COPY . .
WORKDIR "/src/RealEstateListingAPI"
RUN dotnet build "RealEstateListingApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RealEstateListingApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateListingApi.dll"]


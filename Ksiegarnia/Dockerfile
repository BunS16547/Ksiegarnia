FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ksiegarnia/Ksiegarnia.csproj", "Ksiegarnia/"]
RUN dotnet restore "Ksiegarnia/Ksiegarnia.csproj"
COPY . .
WORKDIR "/src/Ksiegarnia"
RUN dotnet build "./Ksiegarnia.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ksiegarnia.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ksiegarnia.dll"]
ENV ASPNETCORE_URLS=http://0.0.0.0:10000


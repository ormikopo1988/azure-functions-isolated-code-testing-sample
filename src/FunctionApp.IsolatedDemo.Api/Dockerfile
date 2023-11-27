#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FunctionApp.IsolatedDemo.Api/FunctionApp.IsolatedDemo.Api.csproj", "FunctionApp.IsolatedDemo.Api/"]
RUN dotnet restore "FunctionApp.IsolatedDemo.Api/FunctionApp.IsolatedDemo.Api.csproj"
COPY . .
WORKDIR "/src/FunctionApp.IsolatedDemo.Api"
RUN dotnet build "FunctionApp.IsolatedDemo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FunctionApp.IsolatedDemo.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
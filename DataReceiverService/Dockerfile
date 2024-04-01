FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR "/app"
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"
COPY ["DataReceiverService.csproj", "DataReceiverService/"]
RUN dotnet restore "DataReceiverService/DataReceiverService.csproj"
COPY [".", "DataReceiverService/"]
WORKDIR "/src/DataReceiverService"
RUN dotnet publish "DataReceiverService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/build .
ENV ASPNETCORE_HTTP_PORTS=8081
ENTRYPOINT ["dotnet", "DataReceiverService.dll"]
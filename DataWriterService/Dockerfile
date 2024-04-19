FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR "/app"
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"
COPY ["DataWriterService/DataWriterService.csproj", "DataWriterService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "DataWriterService/DataWriterService.csproj"
COPY ["DataWriterService", "DataWriterService/"]
COPY ["Shared", "Shared/"]
WORKDIR "/src/DataWriterService"
RUN dotnet publish "DataWriterService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/build .
ENV ASPNETCORE_HTTP_PORTS=8082
ENTRYPOINT ["dotnet", "DataWriterService.dll"]
# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy all files and restore dependencies
COPY . .
RUN dotnet restore "./Student_API/Student_API.csproj" --disable-parallel

# Install FluentMigrator CLI globally
RUN dotnet tool install -g FluentMigrator.DotNet.Cli

# Ensure the tools path is available in PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Publish the application
RUN dotnet publish "./Student_API/Student_API.csproj" -c Release -o /app --no-restore


# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published files from the build stage to the app directory
COPY --from=build /app ./

# Expose the port
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "Student_API.dll"]

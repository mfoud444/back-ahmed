# # Base image for the ASP.NET Core runtime
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# WORKDIR /app
# EXPOSE 5125

# ENV ASPNETCORE_URLS=http://0.0.0.0:5125

# # Build and publish the ASP.NET Core app
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG configuration=Release
# WORKDIR /src
# COPY ["Backend.csproj", "./"]
# RUN dotnet restore "Backend.csproj"
# COPY . . 
# WORKDIR "/src/."
# RUN dotnet build "Backend.csproj" -c $configuration -o /app/build

# FROM build AS publish
# ARG configuration=Release
# RUN dotnet publish "Backend.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# # Final stage combining the ASP.NET app and PostgreSQL
# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .

# # Install PostgreSQL
# RUN apt-get update && apt-get install -y postgresql postgresql-contrib

# # Set environment variables for PostgreSQL
# ENV POSTGRES_USER=postgres
# ENV POSTGRES_PASSWORD=1111
# ENV POSTGRES_DB=ArtifyDB

# # Configure PostgreSQL
# RUN service postgresql start && \
#     su - postgres -c "psql -c \"ALTER USER postgres PASSWORD '${POSTGRES_PASSWORD}';\"" && \
#     su - postgres -c "createdb ${POSTGRES_DB}"

# COPY wait-for-postgres.sh /app/
# RUN chmod +x /app/wait-for-postgres.sh
# CMD /app/wait-for-postgres.sh && dotnet Backend.dll


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5125

ENV ASPNETCORE_URLS=http://0.0.0.0:5125

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Backend.csproj", "./"]
RUN dotnet restore "Backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Backend.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Backend.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]

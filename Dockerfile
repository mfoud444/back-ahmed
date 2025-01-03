FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5125

ENV ASPNETCORE_URLS=http://0.0.0.0:5125
# ENV ConnectionStrings__Local="Host=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.xergfpyauvdbavdyzzbk;Password=009988Ppooii@@@@"
ENV ConnectionStrings__Local="Host=aws-0-eu-central-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.hpbtdersdvvpxrkmhbfe;Password=009988Ppooii@@@@"
ENV JWT__KEY="your-long-secret-key"
ENV JWT__ISSUER="your-issuer"
ENV JWT__AUDIENCE="your-audience"
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend.csproj", "./"]
RUN dotnet restore "Backend.csproj"
COPY . .
RUN dotnet build "Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]
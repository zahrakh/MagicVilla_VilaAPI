FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["MagicVilla_Web/MagicVilla_Web.csproj", "MagicVilla_Web/"]
COPY ["MagicVilla_Utility/MagicVilla_Utility.csproj", "MagicVilla_Utility/"]
RUN dotnet restore "MagicVilla_Web/MagicVilla_Web.csproj"

COPY . .
WORKDIR "/src/MagicVilla_Web"
RUN dotnet publish "MagicVilla_Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MagicVilla_Web.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Printly/Printly.csproj", "Printly/"]
COPY ["Printly.Dto/Printly.Dto.csproj", "Printly.Dto/"]
RUN dotnet restore "Printly/Printly.csproj"
COPY . .
WORKDIR "/src/Printly"
RUN dotnet build "Printly.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Printly.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Printly.dll"]
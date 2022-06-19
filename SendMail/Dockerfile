FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SendMail.csproj", "."]
RUN dotnet restore "./SendMail.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SendMail.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SendMail.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SendMail.dll"]
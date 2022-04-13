#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=http://*:5001
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BoaSaudeRefund.csproj", "."]
RUN dotnet restore "BoaSaudeRefund.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "BoaSaudeRefund.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BoaSaudeRefund.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoaSaudeRefund.dll"]

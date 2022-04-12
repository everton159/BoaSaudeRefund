FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["BoaSaudeRefund.csproj", "."]
COPY Setup.sh Setup.sh

RUN dotnet tool install --global dotnet-ef

RUN dotnet restore "BoaSaudeRefund.csproj"
COPY . .
WORKDIR "/src/."

RUN chmod +x ./Setup.sh
CMD /bin/bash ./Setup.sh
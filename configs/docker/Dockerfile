# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

RUN apt-get update && \
    apt-get install -y git && \
    rm -rf /var/lib/apt/lists/*

COPY STO-Desk-backend/*.csproj ./STO-Desk-backend/

RUN dotnet restore ./STO-Desk-backend/STO-Desk-backend.csproj

COPY . .

WORKDIR /app/STO-Desk-backend

RUN dotnet publish STO-Desk-backend.csproj \
    -c Release \
    -o /out

# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

RUN apt-get update && \
    apt-get install -y curl && \
    rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "STO-Desk-backend.dll"]

# 7D2D ServerInfo (Console)

Dieses Projekt zeigt Server- und Kalenderinformationen für **7 Days to Die** in einer Console-UI an.  
Es basiert auf **.NET 8** und lädt seine Laufzeit-Konfiguration serverseitig über GitHub (kein eigener Server nötig).

## Features

- Console-UI mit Serverstatus, Spielern und Kalenderansicht
- UDP-Query an den 7DTD-Server
- Zentrale Konfiguration über GitHub-JSON
- Auto-Updates über GitHub Releases (NetSparkle)

## Voraussetzungen

- Windows (Console-App mit Windows-Target)
- .NET 8 SDK

## Lokaler Build

```bash
dotnet build 7D2D_ServerInfo/7D2D_ServerInfo.csproj
```

## Konfiguration über GitHub

Die App lädt ihre Konfiguration von:

```
https://raw.githubusercontent.com/<org>/<repo>/main/config/server-config.json
```

Beispiel (`config/server-config.json`):

```json
{
  "serverHost": "127.0.0.1",
  "serverPort": 26900,
  "refreshIntervalSeconds": 2.5,
  "updateAppCastUrl": "https://github.com/<org>/<repo>/releases/latest/download/appcast.xml",
  "updatePublicKey": "PASTE_YOUR_NETSPARKLE_PUBLIC_KEY_HERE"
}
```

### Felder

- `serverHost`: Hostname oder IP des 7DTD-Servers
- `serverPort`: Game-Port (Query nutzt `port + 1`)
- `refreshIntervalSeconds`: Aktualisierungsrate der Console-UI
- `updateAppCastUrl`: URL zur NetSparkle AppCast-Datei
- `updatePublicKey`: Ed25519 Public Key für Signaturprüfung

## Auto-Updates (GitHub Releases)

Für Updates wird NetSparkle verwendet.  
Die App startet den Update-Loop automatisch, sobald `updateAppCastUrl` und `updatePublicKey` gesetzt sind.

## Debug-Modus

Zum Starten mit Dummy-Daten:

```bash
dotnet run --project 7D2D_ServerInfo/7D2D_ServerInfo.csproj -- /Debug
```

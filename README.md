# 7D2D ServerInfo (Console)

## English

This project shows server and calendar information for **7 Days to Die** in a console UI.  
It is based on **.NET 8** and loads its runtime configuration from GitHub (no dedicated server required).

### Features

- Console UI with server status, players, and calendar view
- UDP query to the 7DTD server
- Central configuration via GitHub JSON
- Auto-updates via GitHub Releases (NetSparkle)

### Requirements

- Windows (console app with Windows target)
- .NET 8 SDK

### Local Build

```bash
dotnet build 7D2D_ServerInfo/7D2D_ServerInfo.csproj
```

### Configuration via GitHub

The app loads its configuration from:

```
https://raw.githubusercontent.com/<org>/<repo>/main/config/server-config.json
```

Example (`config/server-config.json`):

```json
{
  "serverHost": "127.0.0.1",
  "serverPort": 26900,
  "refreshIntervalSeconds": 2.5,
  "updateAppCastUrl": "https://github.com/<org>/<repo>/releases/latest/download/appcast.xml",
  "updatePublicKey": "PASTE_YOUR_NETSPARKLE_PUBLIC_KEY_HERE"
}
```

#### Fields

- `serverHost`: Hostname or IP of the 7DTD server
- `serverPort`: Game port (query uses `port + 1`)
- `refreshIntervalSeconds`: Refresh rate of the console UI
- `updateAppCastUrl`: URL to the NetSparkle appcast file
- `updatePublicKey`: Ed25519 public key for signature verification

### Auto-Updates (GitHub Releases)

NetSparkle is used for updates.  
The app starts the update loop automatically once `updateAppCastUrl` and `updatePublicKey` are set.

### Debug Mode

To start with dummy data:

```bash
dotnet run --project 7D2D_ServerInfo/7D2D_ServerInfo.csproj -- /Debug
```

---

## Deutsch

Dieses Projekt zeigt Server- und Kalenderinformationen für **7 Days to Die** in einer Console-UI an.  
Es basiert auf **.NET 8** und lädt seine Laufzeit-Konfiguration serverseitig über GitHub (kein eigener Server nötig).

### Features

- Console-UI mit Serverstatus, Spielern und Kalenderansicht
- UDP-Query an den 7DTD-Server
- Zentrale Konfiguration über GitHub-JSON
- Auto-Updates über GitHub Releases (NetSparkle)

### Voraussetzungen

- Windows (Console-App mit Windows-Target)
- .NET 8 SDK

### Lokaler Build

```bash
dotnet build 7D2D_ServerInfo/7D2D_ServerInfo.csproj
```

### Konfiguration über GitHub

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

#### Felder

- `serverHost`: Hostname oder IP des 7DTD-Servers
- `serverPort`: Game-Port (Query nutzt `port + 1`)
- `refreshIntervalSeconds`: Aktualisierungsrate der Console-UI
- `updateAppCastUrl`: URL zur NetSparkle AppCast-Datei
- `updatePublicKey`: Ed25519 Public Key für Signaturprüfung

### Auto-Updates (GitHub Releases)

Für Updates wird NetSparkle verwendet.  
Die App startet den Update-Loop automatisch, sobald `updateAppCastUrl` und `updatePublicKey` gesetzt sind.

### Debug-Modus

Zum Starten mit Dummy-Daten:

```bash
dotnet run --project 7D2D_ServerInfo/7D2D_ServerInfo.csproj -- /Debug
```

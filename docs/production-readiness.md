# Production Readiness & Code Review

This document summarizes the production checks, risks, and the applied improvements.

## Overview

The application is a .NET 8 console UI that queries 7 Days to Die server data via UDP and renders a calendar. Runtime configuration is loaded remotely and can fall back to a local file.

## Review Findings & Fixes

### 1) Configuration & Validation
**Risk:** Invalid or missing configuration can cause crashes or undefined behavior.

**Fixes:**
- Normalize config values (trim, default interval for invalid values).
- Validate critical fields (host present, port in range 1–65534).
- Log warnings and validation failures.

### 2) Update Mechanism
**Risk:** A non-HTTPS feed or invalid URL can introduce security issues.

**Fixes:**
- Validate appcast URL and enforce HTTPS.
- Catch and log updater startup errors.
- Keep the updater instance alive so the loop is not collected prematurely.

### 3) UDP Query Stability
**Risk:** Bad hostnames, invalid ports, or encoding issues can break queries.

**Fixes:**
- Strict host/port validation in the UDP connector.
- Latin-1 encoding for debug payload and parser to avoid data loss.
- Parser shields bad values and logs the cause instead of crashing.

### 4) Console UI Robustness
**Risk:** Small or redirected consoles can cause exceptions from `SetCursorPosition`.

**Fixes:**
- Guard against redirected output (no interactive rendering).
- Check window and buffer size and show a clear message instead of crashing.

## Operations & Deployment Notes

- **Configuration:**
  - Remote JSON must contain valid `serverHost` and `serverPort`.
  - `refreshIntervalSeconds` must not be 0/NaN/Infinity; invalid values default to 1 second.
  - Updates require an HTTPS URL and a valid public key.

- **Console Size:**
  - Recommended minimum: 111x21.
  - Smaller sizes will show a clear message instead of rendering.

- **Network:**
  - UDP query uses `serverPort + 1`.
  - DNS resolution prefers IPv4.

## Recommended Next Steps

- Enable a CI pipeline with `dotnet test` and linting.
- Document release signing and appcast deployment (key management).
- Optional: add a health check or retry backoff for UDP failures.

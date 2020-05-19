# Changelog

All notable changes to this project will be documented in this file.
The format is based on Keep a Changelog, and this project adheres to Semantic Versioning.

## Unreleased

### Added
* Loki grpc serilog sink in Titan.
* `LokiLabelProvider` in Titan.

### Changed
* `ToLowerInvariant()` to `ToUpperInvariant()` in `TitanLibHelper.GetLogLevel()`.
* `ToLowerInvariant()` to `ToUpperInvariant()` in GraylogOptions class methods `GetMessageIdGeneratorType()` and `GetTransportType()`.
* Bump Microsoft.Extensions.Configuration from 3.1.3 to 3.1.4
* Bump Microsoft.Extensions.DependencyInjection from 3.1.3 to 3.1.4
* Bump NSwag.AspNetCore from 13.4.2 to 13.5.0
* Bump AspNetCore.HealthChecks.UI from 3.1.1-preview4 to 3.1.1-preview6
* Bump Microsoft.Extensions.Options from 3.1.3 to 3.1.4

### Removed
* Seq Serilog sink.
* Greylog Serilog sink.
* Seq metrics publisher.



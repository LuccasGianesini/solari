# Solari 🚀

![GitHub issues](https://img.shields.io/github/issues/LuccasGianesini/solari)
[![Build Status](https://dev.azure.com/luccaslauthgianesini/Solari/_apis/build/status/LuccasGianesini.solari?branchName=master&jobName=Build%20solution%20and%20create%20NuGet%20packages)](https://dev.azure.com/luccaslauthgianesini/Solari/_build/latest?definitionId=24&branchName=master)
![Nuget](https://img.shields.io/nuget/v/Solari.Sol)

####[Changelog](https://github.com/LuccasGianesini/solari/wiki/Changelog)

### Description

  Solari is a set of libraries that aim to make it easy to use most common technologies in dotnet core. These technologies include MongoDb. Elasticsearch log backend and Apm, Jaeger and others.

### Features

  * MongoDb Integration
  * MongoDb Tracing
  * RabbitMq Tracing with ElasticApm
  * Jaeger Integration
  * ElasticApm Tracing
  * Elasticsearch backend for logs
  * Http requests as code
  * CQRS tooling

### Projects

 |   Project | Component                            |
 | :-------- |:------------------------------------ |
 | Sol       | Application Bootstrapper             |
 | Io        | Health Checks                        |
 | Ceres     | Metrics                              |
 | Eris      | CQRS                                 |
 | Titan     | Logging (Serilog)                    |
 | Vanth     | CommonResponse                       |
 | Callisto  | MongoDb                              |
 | Deimos    | Tracing (Jaeger and ElasticApm       |
 | Ganymede  | Http                                 |
 | Juno      | Hashicorp Vault                      |
 | Hyperion  | Harshicorp Consul                    |
 | Themis    | Monitoring stack (ceres, deimos, io) |
 | Oberon    | Redis                                |
  

### Install
   |   Method            | Code                                                          |
   | :------------------ |:------------------------------------------------------------- |
   | Package Manager     | `Install-Package <PackageName> -Version x.x.x`                |
   | Package Reference   | `<PackageReference Include=<PackageName> Version="x.x.x" />`|
   | .Net CLI            | `dotnet add package <PackageName> --version x.x.x`            |
   | Packet CLI          | `paket add <PackageName> --version x.x.x`                     |



### Usage
SOON on the wiki
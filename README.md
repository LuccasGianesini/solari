# Solari 🚀

![GitHub issues](https://img.shields.io/github/issues/LuccasGianesini/solari)
[![Build Status](https://dev.azure.com/luccaslauthgianesini/Solari/_apis/build/status/LuccasGianesini.solari?branchName=master&jobName=Build%20solution%20and%20create%20NuGet%20packages)](https://dev.azure.com/luccaslauthgianesini/Solari/_build/latest?definitionId=24&branchName=master)

   |   Package                                                                             | Version                                                                 | Pre-Release                                                                    | 
   | :------------------------------------------------------------------------------------ |:----------------------------------------------------------------------- |:-------------------------------------------------------------------------- |
   | [Sol](https://www.nuget.org/packages/Solari.Sol/)                                     | ![Nuget](https://img.shields.io/nuget/v/Solari.Sol)                     | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Sol)                     |
   | [Io](https://www.nuget.org/packages/Solari.Io/)                                       | ![Nuget](https://img.shields.io/nuget/v/Solari.Io)                      | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Io)                      |
   | [Rhea](https://www.nuget.org/packages/Solari.Rhea/)                                   | ![Nuget](https://img.shields.io/nuget/v/Solari.Rhea)                    | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Rhea)                    |
   | [Eris](https://www.nuget.org/packages/Solari.Eris/)                                   | ![Nuget](https://img.shields.io/nuget/v/Solari.Eris)                    | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Eris)                    |
   | [Titan](https://www.nuget.org/packages/Solari.Titan/)                                 | ![Nuget](https://img.shields.io/nuget/v/Solari.Titan)                   | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Titan)                   |
   | [Titan Abstractions](https://www.nuget.org/packages/Solari.Titan.Abstractions/)       | ![Nuget](https://img.shields.io/nuget/v/Solari.Titan.Abstractions)      | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Titan.Abstractions)      | 
   | [Vanth](https://www.nuget.org/packages/Solari.Vanth/)                                 | ![Nuget](https://img.shields.io/nuget/v/Solari.Vanth)                   | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Vanth)                   | 
   | [Callisto](https://www.nuget.org/packages/Solari.Callisto/)                           | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto)                | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Callisto)                | 
   | [Callisto Abstractions](https://www.nuget.org/packages/Solari.Callisto.Abstractions/) | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto.Abstractions)   | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Callisto.Abstractions)   |
   | [Callisto Connector](https://www.nuget.org/packages/Solari.Callisto.Connector/)       | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto.Connector)      | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Callisto.Connector)      |
   | [Callisto Tracer](https://www.nuget.org/packages/Solari.Callisto.Tracer/)             | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto.Tracer)         | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Callisto.Tracer)         |
   | [Deimos](https://www.nuget.org/packages/Solari.Deimos/)                               | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos)                  | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Deimos)                  |
   | [Deimos Abstractions](https://www.nuget.org/packages/Solari.Deimos.Abstractions/)     | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos.Abstractions)     | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Deimos.Abstractions)     |
   | [Deimos Elastic](https://www.nuget.org/packages/Solari.Deimos.Elastic/)               | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos.Elastic)          | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Deimos.Elastic)          |
   | [Deimos Jaeger](https://www.nuget.org/packages/Solari.Deimos.Jaeger/)                 | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos.Jaeger)           | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Deimos.Jaeger)           |
   | [Miranda Tracer](https://www.nuget.org/packages/Solari.Miranda.Tracer/)               | ![Nuget](https://img.shields.io/nuget/v/Solari.Miranda.Tracer)          | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Miranda.Tracer)          |
   | [Miranda](https://www.nuget.org/packages/Solari.Miranda/)                             | ![Nuget](https://img.shields.io/nuget/v/Solari.Miranda)                 | ![Nuget](https://img.shields.io/nuget/vpre/Solari.Miranda)                 |
  
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

 |   Project | Component                      |
 | :-------- |:------------------------------ |
 | Sol       | Application Bootstrapper       |
 | Io        | Extensions                     |
 | Rhea      | Utils                          |
 | Eris      | CQRS                           |
 | Titan     | Logging (Serilog)              |
 | Vanth     | CommonResponse                 |
 | Callisto  | MongoDb                        |
 | Deimos    | Tracing (Jaeger and ElasticApm |
 | Ganymede  | Http                           |
 | Miranda   | RabbitMq (RawRabbit)           |
  

### Install
   |   Method            | Code                                                          |
   | :------------------ |:------------------------------------------------------------- |
   | Package Manager     | `Install-Package <PackageName> -Version x.x.x`                |
   | Package Reference   | `<PackageReference Include=<PackageName> Version="x.x.x" />`|
   | .Net CLI            | `dotnet add package <PackageName> --version x.x.x`            |
   | Packet CLI          | `paket add <PackageName> --version x.x.x`                     |



### Usage
SOON
# Solari 🚀

![GitHub issues](https://img.shields.io/github/issues/LuccasGianesini/solari)
[![Build Status](https://dev.azure.com/luccaslauthgianesini/Solari/_apis/build/status/LuccasGianesini.solari?branchName=master&jobName=Build%20solution%20and%20create%20NuGet%20packages)](https://dev.azure.com/luccaslauthgianesini/Solari/_build/latest?definitionId=24&branchName=master)
![Nuget](https://img.shields.io/nuget/v/Solari.Sol)
![Nuget](https://img.shields.io/nuget/vpre/Solari.Sol)

   |   Package                                                                              
   | :------------------------------------------------------------------------------------ |
   | [Sol](https://www.nuget.org/packages/Solari.Sol/)                                     |
   | [Io](https://www.nuget.org/packages/Solari.Io/)                                       |
   | [Rhea](https://www.nuget.org/packages/Solari.Rhea/)                                   |
   | [Eris](https://www.nuget.org/packages/Solari.Eris/)                                   |
   | [Titan](https://www.nuget.org/packages/Solari.Titan/)                                 |
   | [Titan Abstractions](https://www.nuget.org/packages/Solari.Titan.Abstractions/)       | 
   | [Vanth](https://www.nuget.org/packages/Solari.Vanth/)                                 | 
   | [Callisto](https://www.nuget.org/packages/Solari.Callisto/)                           | 
   | [Callisto Abstractions](https://www.nuget.org/packages/Solari.Callisto.Abstractions/) |
   | [Callisto Connector](https://www.nuget.org/packages/Solari.Callisto.Connector/)       |
   | [Callisto Tracer](https://www.nuget.org/packages/Solari.Callisto.Tracer/)             |
   | [Deimos](https://www.nuget.org/packages/Solari.Deimos/)                               |
   | [Deimos Abstractions](https://www.nuget.org/packages/Solari.Deimos.Abstractions/)     |
   | [Deimos Elastic](https://www.nuget.org/packages/Solari.Deimos.Elastic/)               |
   | [Deimos Jaeger](https://www.nuget.org/packages/Solari.Deimos.Jaeger/)                 |
   | [Miranda Tracer](https://www.nuget.org/packages/Solari.Miranda.Tracer/)               |
   | [Miranda](https://www.nuget.org/packages/Solari.Miranda/)                             |
   | [Miranda](https://www.nuget.org/packages/Solari.Oberon/)                              | 
  
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
 | Oberon    | Redis                          |
  

### Install
   |   Method            | Code                                                          |
   | :------------------ |:------------------------------------------------------------- |
   | Package Manager     | `Install-Package <PackageName> -Version x.x.x`                |
   | Package Reference   | `<PackageReference Include=<PackageName> Version="x.x.x" />`|
   | .Net CLI            | `dotnet add package <PackageName> --version x.x.x`            |
   | Packet CLI          | `paket add <PackageName> --version x.x.x`                     |



### Usage
SOON
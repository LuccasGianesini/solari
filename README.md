# Solari 🚀

![GitHub issues](https://img.shields.io/github/issues/LuccasGianesini/solari)
[![Build Status](https://dev.azure.com/luccaslauthgianesini/Solari/_apis/build/status/LuccasGianesini.solari?branchName=master&jobName=Build%20solution%20and%20create%20NuGet%20packages)](https://dev.azure.com/luccaslauthgianesini/Solari/_build/latest?definitionId=24&branchName=master)

   |   Package                                                                             | Version                                                                 |
   | :------------------------------------------------------------------------------------ |:----------------------------------------------------------------------- |
   | [Sol](https://www.nuget.org/packages/Solari.Sol/)                                     | ![Nuget](https://img.shields.io/nuget/v/Solari.Sol)                     |
   | [Io](https://www.nuget.org/packages/Solari.Io/)                                       | ![Nuget](https://img.shields.io/nuget/v/Solari.Io)                      |
   | [Rhea](https://www.nuget.org/packages/Solari.Rhea/)                                   | ![Nuget](https://img.shields.io/nuget/v/Solari.Rhea)                    |
   | [Eris](https://www.nuget.org/packages/Solari.Eris/)                                   | ![Nuget](https://img.shields.io/nuget/v/Solari.Eris)                    |
   | [Titan](https://www.nuget.org/packages/Solari.Titan/)                                 | ![Nuget](https://img.shields.io/nuget/v/Solari.Titan)                   |
   | [Titan Abstractions](https://www.nuget.org/packages/Solari.Titan.Abstractions/)       | ![Nuget](https://img.shields.io/nuget/v/Solari.Titan.Abstractions)      | 
   | [Vanth](https://www.nuget.org/packages/Solari.Vanth/)                                 | ![Nuget](https://img.shields.io/nuget/v/Solari.Vanth)                   | 
   | [Callisto](https://www.nuget.org/packages/Solari.Callisto/)                           | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto)                | 
   | [Callisto Abstractions](https://www.nuget.org/packages/Solari.Callisto.Abstractions/) | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto.Abstractions)   |
   | [Callisto Connector](https://www.nuget.org/packages/Solari.Callisto.Connector/)       | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto.Connector)      |
   | [Callisto Tracer](https://www.nuget.org/packages/Solari.Callisto.Tracer/)             | ![Nuget](https://img.shields.io/nuget/v/Solari.Callisto.Tracer)         |
   | [Deimos](https://www.nuget.org/packages/Solari.Deimos/)                               | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos)                  |
   | [Deimos Abstractions](https://www.nuget.org/packages/Solari.Deimos.Abstractions/)     | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos.Abstractions)     |
   | [Deimos Elastic](https://www.nuget.org/packages/Solari.Deimos.Elastic/)               | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos.Elastic)          |
   | [Deimos Jaeger](https://www.nuget.org/packages/Solari.Jaeger/)                       | ![Nuget](https://img.shields.io/nuget/v/Solari.Deimos.Jaeger)            |
   | [Miranda Tracer](https://www.nuget.org/packages/Solari.Miranda.Tracer/)               | ![Nuget](https://img.shields.io/nuget/v/Solari.Miranda.Tracer)          |
  
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

1.  Sol:                  -> Sol a application bootstrapper (based on [Convey Stack](https://convey-stack.github.io/)).
2.  Io                    -> Is the place where common extensions and helper live!
3.  Rhea                  -> Is a utilities project.
4.  Eris                  -> CQRS components.
5.  Titan                 -> Titan is a wrapper library around Serilog.
6.  Titan Abstractions    -> Abstractions and common objects of Titan.
7.  Vanth                 -> Vanth provides a way to provide homogeneity of reponses.
8.  Callisto              -> MongoDb integration.
9.  Callisto Abstractions -> Abstractions and common objects of Callisto.
10. Callisto Connector    -> MongoDb connector
11. Callisto Tracer       -> Tracing plugins for MongoDb.
12. Deimos                -> Tracing library
13. Deimos Abstractions   -> Abstractions and common objects of Deimos.  
14. Deimos Elastic        -> ElasticApm infrastructure.
15. Deimos Jaeger         -> Jaeger infrastructure.
16. Miranda Tracer        -> Tracing for RabbitMq using ElasticApm.
  

### Install
   |   Method            | Code                                                          |
   | :------------------ |:------------------------------------------------------------- |
   | Package Manager     | `Install-Package <PackageName> -Version x.x.x`                |
   | Package Reference   | `<PackageReference Include=<PackageName> Version="x.x.x" />`|
   | .Net CLI            | `dotnet add package <PackageName> --version x.x.x`            |
   | Packet CLI          | `paket add <PackageName> --version x.x.x`                     |



### Usage
SOON
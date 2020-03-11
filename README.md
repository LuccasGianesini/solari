### TODO - delete todo
1. Create develop branch
2. Change Readme file (change nuget.org link and definition id in azure-pipelines)
3. Create pipeline in azure
4. Pause CI
5. Create project(s) If more than one, create a folder to store all project files with the same name. - `dotnet new classlib -n Project.Name -o src\`
6. Override csproj propertygroup according to the csprojTemplate
7. Create test projects if any
8. Create solution in the root path - `dotnet new solution -n Projec.Name`
9. Add project(s) to solution - `dotnet sln add src\Project.Name.csproj`
10. Delete 'tests\testsgohere'
11. Delete 'src\csprojTemplate'

### Follow readme template underneath - delete this line

# Lib Name

![GitHub issues](https://img.shields.io/github/issues/LuccasGianesini/solari-vanth?style=flat-square)
![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/luccaslauthgianesini/93ab7f40-7d1c-4c60-b976-c4c834399700/10/master?style=flat-square&logo=azure-pipelines)
![Nuget](https://img.shields.io/nuget/v/Solari.Vanth?style=flat-square&logo=nuget)

### Description

  Description goes here

### Features

  Put features here

### Install

[Nuget.org page](https://www.nuget.org/packages/Solari.Vanth/)

   |   Method            | Code                                                        |
   | :------------------ |:----------------------------------------------------------- |
   | Package Manager     | `Install-Package Solari.Eris -Version x.x.x`                |
   | Package Reference   | `<PackageReference Include="Solari.Eris" Version="x.x.x" />`|
   | .Net CLI            | `dotnet add package Solari.Eris --version x.x.x`            |
   | Packet CLI          | `paket add Solari.Eris --version x.x.x`                     |



### Usage
  Put Usage Here

### APi
   
Check out the Wiki for the public api's exposed by the library: Library public Api's

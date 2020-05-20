FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
RUN mkdir /app/src
# Copy csproj and restore as distinct layers
COPY / ./
#RUN find -maxdepth 4 -type d -name bin -prune -exec rm -rf {} \; && find -maxdepth 4 -type d -name obj -prune -exec rm -rf {} \;
RUN dotnet restore
#
# Copy everything else and build
COPY . ./

RUN dotnet publish --no-restore -c Release -o out
# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 5000
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Solari.Samples.WebApi.dll"]

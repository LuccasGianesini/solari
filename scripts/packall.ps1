[CmdletBinding()]
param (
  $VERSION,
  $BUILD_ARTIFACT_DIR
)

function packSrc  {
    (Get-ChildItem ./src/ -Recurse -Depth 3 -Include *.csproj) | foreach {
      Write-Host('Packaging ' + $_.Name + ' with version ' + $VERSIONs)
      dotnet pack $_ -c release /p:PackageVersion=$VERSION --no-restore -o $BUILD_ARTIFACT_DIR -v q
      Write-Host('Package created at ' + $BUILD_ARTIFACT_DIR)
    }
  }
packSrc
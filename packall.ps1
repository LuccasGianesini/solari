    [CmdletBinding()]
    param (
        $BUILD_NUMBER,
        $BUILD_ARTIFACT_DIR
    )
    (Get-ChildItem ./src/ -Recurse -Depth 3 -Include *.csproj) | foreach {
        Write-Host('Packaging ' + $_.Name + 'with version ' + $BUILD_NUMBER)
        dotnet pack $_ -c release /p:PackageVersion=$BUILD_NUMBER --no-restore -o $BUILD_ARTIFACT_DIR -v q
        Write-Host('Package created at ' + $BUILD_ARTIFACT_DIR)
    }

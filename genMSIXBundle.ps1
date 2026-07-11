# Based on
# $arch = switch ($env:PROCESSOR_ARCHITECTURE) { "AMD64" { "x64" } "x86" { "x86" } "ARM64" { "arm64" } default { "x64" } }; Write-Host "Detected: $arch"; $found = Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin\*\$arch\makeappx.exe" -ErrorAction SilentlyContinue | Sort-Object Name -Descending | Select-Object -First 1; if ($found) { Write-Host "SUCCESS: $($found.FullName)" -ForegroundColor Green; $found.FullName } else { Write-Host "Not found for $arch" -ForegroundColor Red }

if (-not (Get-Command makeappx.exe -ErrorAction SilentlyContinue)) {
    $arch = switch ($env:PROCESSOR_ARCHITECTURE) { "AMD64" { "x64" } "x86" { "x86" } "ARM64" { "arm64" } default { "x64" } }
    $sdkTool = Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin\*\$arch\makeappx.exe" -ErrorAction SilentlyContinue |
        Select-Object -First 1

    if (-not $sdkTool) {
        Write-Error "makeappx.exe not found. Please install the Windows SDK."
        exit 1
    }

    $makeAppx = $sdkTool.FullName
}
else {
    $makeAppx = "makeappx.exe"
}

if (!(Test-Path buildArtifact)) {
    mkdir buildArtifact
}

# Dirs
$rt = "$PSScriptRoot"
$ba = "$rt\buildArtifact"

Push-Location $rt\CommandBeep

dotnet build --configuration Release -p:GenerateAppxPackageOnBuild=true -p:Platform=x64 -p:AppxPackageDir="AppPackages\x64\"
dotnet build --configuration Release -p:GenerateAppxPackageOnBuild=true -p:Platform=ARM64 -p:AppxPackageDir="AppPackages\ARM64\"

# This is to get all MSIX (non scale), and to generate a file name in accordance with the Microsoft Documentation
# <ExtensionName>_<VersionNumber>_Bundle.msixbundle
$msixes_items = Get-ChildItem bin -Recurse -Filter *.msix | Where-Object { $_.Name -notmatch 'scale' }
$msixbundle_name = ($msixes_items | Select-Object -First 1).Name -replace 'arm64|x64', 'Bundle' -replace '.msix', '.msixbundle'

Push-Location $ba

if (Test-Path bundle_mapping.txt) {
    Remove-Item bundle_mapping.txt
}

Write-Output "[Files]" > bundle_mapping.txt
foreach ( $item in $msixes_items ) {
    '"{0}" "{1}"' -f $item.FullName, $item.Name >> bundle_mapping.txt
}

if (Test-Path $msixbundle_name) {
    Remove-Item $msixbundle_name
}

& $makeAppx bundle /f bundle_mapping.txt /p $msixbundle_name

Remove-Item bundle_mapping.txt

if (Test-Path $msixbundle_name) {
    Write-Host "MSIX Bundle created: $ba\$msixbundle_name" -ForegroundColor Green
}
else {
    Write-Host "MSIX Bundle creation failed." -ForegroundColor Red
}

Pop-Location
Pop-Location

exit 0
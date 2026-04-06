# Publie le dacpac vers la base Aspire "formation" (SQL en localhost:5678).
# Prérequis : MSBuild VS (SSDT) pour générer le .dacpac ; SqlPackage (outil global dotnet ou SSDT).
#
# Usage (depuis ce dossier) :
#   .\Publish-Local.ps1
#   .\Publish-Local.ps1 -Password 'autreMotDePasse'
#
# Installer SqlPackage si besoin :
#   dotnet tool install -g microsoft.sqlpackage
#   ($env:Path doit contenir le dossier des outils dotnet globaux)

[CmdletBinding()]
param(
    [string] $Server = '127.0.0.1',
    [int] $Port = 5678,
    [string] $Database = 'formation',
    [string] $User = 'sa',
    [string] $Password = 'yourStrong(!)Password'
)

$ErrorActionPreference = 'Stop'
$here = $PSScriptRoot
$sqlproj = Join-Path $here 'WeChooz.TechAssessment.Database.SqlServer.sqlproj'

$vs = & "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -property installationPath
if (-not $vs) { throw 'Visual Studio + MSBuild introuvable (vswhere).' }
$msbuild = Join-Path $vs 'MSBuild\Current\Bin\MSBuild.exe'
if (-not (Test-Path $msbuild)) { throw "MSBuild introuvable : $msbuild" }

Write-Host '>> Build dacpac (MSBuild)...' -ForegroundColor Cyan
& $msbuild $sqlproj /t:Build /p:Configuration=Debug /verbosity:minimal | Write-Host

$dacpac = Join-Path $here "bin\Debug\WeChooz.TechAssessment.Database.SqlServer.dacpac"
if (-not (Test-Path $dacpac)) { throw "dacpac introuvable : $dacpac (la build SSDT a échoué ?)" }

$targetCs = "Server=$Server,$Port;User ID=$User;Password=$Password;Database=$Database;Encrypt=True;TrustServerCertificate=True;"

$sqlPackage = Get-Command sqlpackage -ErrorAction SilentlyContinue
if (-not $sqlPackage) {
    throw @'
SqlPackage introuvable dans le PATH.
Installe-le : dotnet tool install -g microsoft.sqlpackage
Puis rouvre le terminal ou ajoute le dossier des outils globaux dotnet au PATH.
'@
}

Write-Host '>> Publish dacpac -> formation...' -ForegroundColor Cyan
& sqlpackage @(
    '/Action:Publish',
    "/SourceFile:$dacpac",
    "/TargetConnectionString:$targetCs",
    '/p:BlockOnPossibleDataLoss=false'
)

Write-Host '>> Terminé.' -ForegroundColor Green

# Copies SerratedJQ.js from this project's wwwroot and transforms for AMD/RequireJS (Uno WasmScripts).
# Wraps content in define(() => { ... }); and strips ES module exports, matching SerratedJSInterop pattern.
# Usage: .\CopySerratedJQToWasmScripts.ps1 -SourcePath <path> -DestPath <path>
param(
    [Parameter(Mandatory=$true)][string]$SourcePath,
    [Parameter(Mandatory=$true)][string]$DestPath
)
$content = [System.IO.File]::ReadAllText($SourcePath)
$content = "define(() => {`n`n" + $content
# Strip ES module exports and close the define callback (AMD/RequireJS for Uno WasmScripts)
$content = $content -replace 'export const LoadJQuery = SerratedJQ\.LoadJQuery;[\r\n]*', ''
$content = $content -replace 'export \{ SerratedJQ \};', '});'
$dir = [System.IO.Path]::GetDirectoryName($DestPath)
if (-not [System.IO.Directory]::Exists($dir)) {
    [System.IO.Directory]::CreateDirectory($dir) | Out-Null
}
[System.IO.File]::WriteAllText($DestPath, $content)

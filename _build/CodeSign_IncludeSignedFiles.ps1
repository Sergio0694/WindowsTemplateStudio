﻿param
(
	[Parameter(Mandatory=$true)]
	[string]$vsixFilePath,
    [Parameter(Mandatory=$true)]
	[string]$inputPath
)


Add-Type -Assembly System.IO.Compression
Add-Type -Assembly System.IO.Compression.FileSystem


$files= Get-ChildItem $inputPath -rec | where {!$_.PSIsContainer}

$vsix = [System.IO.Compression.ZipFile]::Open($vsixFilePath, [System.IO.Compression.ZipArchiveMode]::Update)
$inputDir = (get-Item "$inputPath\").Target

foreach ($file in $files)
{
    Write-Host "Processing file "$file.FullName
    $newFileName = $file.FullName.Replace("$inputDir\", '').Replace("\", "/")

    $entry = $vsix.Entries | where {$_.FullName -eq $newFileName}

    if ($entry)
    {
        Write-Host Deleted file $entry.FullName
        $entry.Delete()
    }
    else
    {
        Write-Error "$newFileName not in zip"
    }

    Write-Host Adding file $newFileName 
    Write-Host
    [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($vsix, $file.FullName, $newFileName) > $null 
}

#free object
$vsix.Dispose()

#From .CMD: powershell.exe -ExecutionPolicy Bypass -File "C:\path\to\ffmpeg_parallel.ps1" -inputFolder "C:\input" 
#-outputFolder "C:\output" -fileExtension ".ext" -ffmpegArgs "-codec:v copy -codec:a copy"
#From Powershell .\Ffmpegparallel.ps1 -inputFolder "F:\Videoer\20230827_DMSprint\B" -outputFolder "F:\Videoer\20230827_DMSprint\B" -fileExtension ".mp4" -ffmpegArgs "-c:v libx264 -crf 24 -c:a copy -r 25 -preset faster -pix_fmt yuvj420p -y" --- 25 fps script


param(
    [string]$inputFolder,
    [string]$outputFolder,
    [string]$fileExtension,
    [string]$ffmpegArgs
)

# Get a list of input files with the specified extension
$inputFiles = Get-ChildItem -Path $inputFolder -File | Where-Object { $_.Extension -eq $fileExtension }

# Function to process each file
function ProcessFile($file) {
    $outputFileName = Join-Path -Path $outputFolder -ChildPath ($file.BaseName + "_f" + $file.Extension)
    $ffmpegCommand = "ffmpeg.exe -i `"$($file.FullName)`" $ffmpegArgs `"$outputFileName`""
    
    Start-Process -FilePath "cmd.exe" -ArgumentList "/c $ffmpegCommand" # -WindowStyle Hidden
}

# Process each file in parallel
$inputFiles | ForEach-Object { ProcessFile $_ } #-Parallel { ProcessFile $_ }


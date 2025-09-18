#working
#Start-Process powershell -ArgumentList "dotnet run --project .\ProductService\ProductService.csproj"
#Start-Process powershell -ArgumentList "dotnet run --project .\OrderService\OrderService.csproj"
#Start-Process powershell -ArgumentList "dotnet run --project .\UserService\UserService.csproj"
#Start-Process powershell -ArgumentList "dotnet run --project .\ApiGateway\ApiGateway.csproj"


$services = @(
    "C:\PR\PR2\APISERVICE\EcommerceSolution\ProductService\ProductService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\OrderService\OrderService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\UserService\UserService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\ApiGateway\ApiGateway.csproj"
)

$pids = @()

foreach ($service in $services) {
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = "dotnet"
    $psi.Arguments = "run --project `"$service`""
    $psi.UseShellExecute = $false
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true
    $psi.CreateNoWindow = $true

    $process = [System.Diagnostics.Process]::Start($psi)
    $pids += $process.Id

    Write-Host "Started $service (PID: $($process.Id))"
}

# Save PIDs for stop-all.ps1
$pids | Out-File "service-pids.txt" -Encoding ascii

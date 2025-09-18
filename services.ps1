param (
    [Parameter(Mandatory=$true)]
    [ValidateSet("start","stop","restart")]
    [string]$action
)

# PID file path
$pidFile = "service-pids.txt"

# List of services
$services = @(
    "C:\PR\PR2\APISERVICE\EcommerceSolution\ProductService\ProductService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\OrderService\OrderService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\UserService\UserService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\ApiGateway\ApiGateway.csproj"
)

function Start-Services {
    $pids = @()
    foreach ($service in $services) {
        $process = Start-Process dotnet -ArgumentList "run --project `"$service`"" -PassThru
        Write-Host "🚀 Started $service (PID: $($process.Id))"
        $pids += $process.Id
    }
    $pids | Out-File $pidFile
    Write-Host "`n✅ All services started. PIDs saved to $pidFile"
}

function Stop-Services {
    if (Test-Path $pidFile) {
        $pids = Get-Content $pidFile
        foreach ($procId in $pids) {
            try {
                Stop-Process -Id $procId -Force -ErrorAction SilentlyContinue
                Write-Host "🛑 Stopped process PID: $procId"
            } catch {
                Write-Host "⚠ Process PID $procId not found (maybe already stopped)."
            }
        }
        Remove-Item $pidFile
        Write-Host "`n✅ All services stopped."
    } else {
        Write-Host "⚠ No PID file found. Run '.\services.ps1 start' first."
    }
}

switch ($action) {
    "start"   { Start-Services }
    "stop"    { Stop-Services }
    "restart" { Stop-Services; Start-Services }
}

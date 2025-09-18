

#Not working
# Stop ProductService
#Get-Process dotnet -ErrorAction SilentlyContinue | Where-Object { $_.Path -like "*ProductService*" } | Stop-Process -Force
# Stop OrderService
#Get-Process dotnet -ErrorAction SilentlyContinue | Where-Object { $_.Path -like "*OrderService*" } | Stop-Process -Force
# Stop UserService
#Get-Process dotnet -ErrorAction SilentlyContinue | Where-Object { $_.Path -like "*UserService*" } | Stop-Process -Force
# Stop ApiGateway
#Get-Process dotnet -ErrorAction SilentlyContinue | Where-Object { $_.Path -like "*ApiGateway*" } | Stop-Process -Force


if (Test-Path "service-pids.txt") {
    $pids = Get-Content "service-pids.txt"

    foreach ($pid in $pids) {
        try {
            Stop-Process -Id $pid -Force -ErrorAction SilentlyContinue
            Write-Host "Stopped process PID: $pid"
        } catch {
            Write-Host "Process PID $pid already stopped."
        }
    }

    Remove-Item "service-pids.txt"
} else {
    Write-Host "No PID file found. Did you run run-all.ps1?"
}

# List of service project paths
$services = @(
    "C:\PR\PR2\APISERVICE\EcommerceSolution\ProductService\ProductService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\OrderService\OrderService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\UserService\UserService.csproj",
    "C:\PR\PR2\APISERVICE\EcommerceSolution\ApiGateway\ApiGateway.csproj"
)

# Get all running dotnet processes
$dotnetProcesses = Get-WmiObject Win32_Process -Filter "Name='dotnet.exe'"

foreach ($service in $services) {
    foreach ($proc in $dotnetProcesses) {
        if ($proc.CommandLine -like "*$service*") {
            Write-Host "Stopping $service (PID: $($proc.ProcessId))"
            Stop-Process -Id $proc.ProcessId -Force
        }
    }
}

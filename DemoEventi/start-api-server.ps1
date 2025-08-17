# Start API Server for Mobile Development
# This script starts the API server with the correct configuration for mobile app connectivity

Write-Host "Starting DemoEventi API Server for Mobile Development..." -ForegroundColor Green
Write-Host "=======================================================" -ForegroundColor Green

# Navigate to API directory
Set-Location "DemoEventi.API"

# Check if port 5163 is already in use
$port5163 = netstat -an | findstr ":5163"
if ($port5163) {
    Write-Host "Port 5163 is already in use. Attempting to stop existing process..." -ForegroundColor Yellow
    # Try to stop any existing dotnet processes
    Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
}

Write-Host ""
Write-Host "Starting API server on:" -ForegroundColor Cyan
Write-Host "  HTTP:  http://localhost:5163" -ForegroundColor White
Write-Host "  HTTPS: https://localhost:7042" -ForegroundColor White
Write-Host ""
Write-Host "Mobile app will connect via: http://10.0.2.2:5163 (Android emulator magic IP)" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the server" -ForegroundColor Yellow
Write-Host "======================================" -ForegroundColor Green

# Start the server
dotnet run --urls "http://localhost:5163;https://localhost:7042"

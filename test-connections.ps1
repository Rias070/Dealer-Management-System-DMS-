#!/usr/bin/env pwsh
# PowerShell Script to test DMS System connections

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "   DMS System Connection Test" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$testResults = @()

# Test 1: PostgreSQL Database Connection
Write-Host "[1/4] Testing PostgreSQL Database..." -ForegroundColor Yellow
try {
    # Try using psql if available
    $env:PGPASSWORD = "12345"
    $testQuery = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'public';"
    $result = psql -h localhost -p 5432 -U postgres -d CompanyDealerDb -t -c $testQuery 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✅ PostgreSQL is running and accessible" -ForegroundColor Green
        Write-Host "     Database: CompanyDealerDb" -ForegroundColor Gray
        Write-Host "     Tables found: $($result.Trim())" -ForegroundColor Gray
        $testResults += @{Test="PostgreSQL"; Status="✅ PASS"}
    } else {
        throw "Connection failed"
    }
} catch {
    Write-Host "  ⚠️  Cannot test with psql command" -ForegroundColor Yellow
    Write-Host "     Please verify manually in pgAdmin 4:" -ForegroundColor Gray
    Write-Host "     - Server: localhost:5432" -ForegroundColor Gray
    Write-Host "     - Database: CompanyDealerDb" -ForegroundColor Gray
    Write-Host "     - User: postgres / Pass: 12345" -ForegroundColor Gray
    $testResults += @{Test="PostgreSQL"; Status="⚠️  MANUAL CHECK"}
}

Start-Sleep -Milliseconds 500

# Test 2: Backend API Server
Write-Host "`n[2/4] Testing Backend API Server..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5232/swagger/index.html" -Method GET -TimeoutSec 3 -ErrorAction Stop
    
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✅ Backend API is running" -ForegroundColor Green
        Write-Host "     URL: http://localhost:5232" -ForegroundColor Gray
        Write-Host "     Swagger: http://localhost:5232/swagger" -ForegroundColor Gray
        $testResults += @{Test="Backend API"; Status="✅ PASS"}
    }
} catch {
    Write-Host "  ❌ Backend API is not running" -ForegroundColor Red
    Write-Host "     To start: cd BE\CompanyDealer\CompanyDealer && dotnet run" -ForegroundColor Gray
    $testResults += @{Test="Backend API"; Status="❌ FAIL"}
}

Start-Sleep -Milliseconds 500

# Test 3: Backend Database Connection (via API)
Write-Host "`n[3/4] Testing Backend-to-Database Connection..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5232/api/Vehicle" -Method GET -TimeoutSec 5 -ErrorAction Stop
    
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✅ Backend successfully connects to Database" -ForegroundColor Green
        $data = $response.Content | ConvertFrom-Json
        Write-Host "     API Response: OK" -ForegroundColor Gray
        $testResults += @{Test="Backend-Database"; Status="✅ PASS"}
    }
} catch {
    if ($_.Exception.Response.StatusCode.value__ -eq 401 -or $_.Exception.Response.StatusCode.value__ -eq 403) {
        Write-Host "  ⚠️  Backend is connected but requires authentication" -ForegroundColor Yellow
        Write-Host "     This is expected behavior" -ForegroundColor Gray
        $testResults += @{Test="Backend-Database"; Status="⚠️  AUTH REQUIRED"}
    } else {
        Write-Host "  ❌ Cannot verify Backend-Database connection" -ForegroundColor Red
        Write-Host "     Error: $($_.Exception.Message)" -ForegroundColor Gray
        $testResults += @{Test="Backend-Database"; Status="❌ FAIL"}
    }
}

Start-Sleep -Milliseconds 500

# Test 4: Frontend Dev Server
Write-Host "`n[4/4] Testing Frontend Dev Server..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5173" -Method GET -TimeoutSec 3 -ErrorAction Stop
    
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✅ Frontend is running" -ForegroundColor Green
        Write-Host "     URL: http://localhost:5173" -ForegroundColor Gray
        Write-Host "     Proxy: /api -> http://localhost:5232" -ForegroundColor Gray
        $testResults += @{Test="Frontend"; Status="✅ PASS"}
    }
} catch {
    Write-Host "  ❌ Frontend is not running" -ForegroundColor Red
    Write-Host "     To start: cd FE\DMS dashboard && npm run dev" -ForegroundColor Gray
    $testResults += @{Test="Frontend"; Status="❌ FAIL"}
}

# Test Summary
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   Test Summary" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

foreach ($result in $testResults) {
    $status = $result.Status
    $test = $result.Test.PadRight(25)
    Write-Host "  $test : $status"
}

Write-Host "`n========================================" -ForegroundColor Cyan

# Additional Info
Write-Host "`n📋 Quick Commands:" -ForegroundColor Cyan
Write-Host "  Start Backend:  cd BE\CompanyDealer\CompanyDealer; dotnet run" -ForegroundColor Yellow
Write-Host "  Start Frontend: cd 'FE\DMS dashboard'; npm run dev" -ForegroundColor Yellow
Write-Host "  View Swagger:   http://localhost:5232/swagger" -ForegroundColor Yellow
Write-Host "  View Frontend:  http://localhost:5173" -ForegroundColor Yellow
Write-Host ""

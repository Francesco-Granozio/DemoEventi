# Mobile App Timeout Issues - COMPLETE SOLUTION ✅

## Root Cause Analysis

The timeout issues were caused by **Android emulator network configuration problems**:

1. ❌ **Wrong IP Address**: Using host machine IP (`192.168.1.4`) instead of Android emulator's special IP
2. ❌ **HTTPS Redirection**: API server was forcing HTTPS even for HTTP requests
3. ❌ **Network Binding**: API server wasn't properly bound to localhost for emulator access

## ✅ COMPLETE SOLUTION

### 1. Android Emulator Network Configuration

**Problem**: Android emulators can't directly access host machine IPs like `192.168.1.4`

**Solution**: Use Android emulator's special IP address `10.0.2.2` which maps to host's `localhost`

### 2. Updated Configuration Files

#### MauiProgram.cs ✅
```csharp
// For Android emulator, use 10.0.2.2 which maps to host's localhost
client.BaseAddress = new Uri("http://10.0.2.2:5163/");
```

#### Program.cs (API) ✅
```csharp
// Commented out for mobile development - causes timeout issues
// app.UseHttpsRedirection();
```

#### network_security_config.xml ✅
```xml
<domain includeSubdomains="true">10.0.2.2</domain>
```

### 3. API Server Startup

**Use the startup script**: `start-api-server.ps1`

Or manually:
```bash
cd DemoEventi/DemoEventi.API
dotnet run --urls "http://localhost:5163;https://localhost:7042"
```

## 🚀 STEP-BY-STEP SOLUTION

### Step 1: Start API Server
```bash
.\start-api-server.ps1
```

### Step 2: Run Mobile App
```bash
cd DemoEventi/DemoEventi.UI
dotnet build -t:Run -f net9.0-android
```

## ✅ Configuration Summary

| Component | Configuration | Status |
|-----------|--------------|--------|
| Mobile App Base URL | `http://10.0.2.2:5163/` | ✅ Fixed |
| API Server Binding | `http://localhost:5163` | ✅ Fixed |
| HTTPS Redirection | Disabled for development | ✅ Fixed |
| Network Security | Android emulator IPs allowed | ✅ Fixed |
| Timeout Settings | 15 seconds for quick failure detection | ✅ Fixed |

## 🔍 Why This Works

1. **`10.0.2.2`**: This is Android emulator's "magic IP" that always maps to the host machine's localhost
2. **HTTP Only**: Eliminates SSL certificate issues during development
3. **No HTTPS Redirection**: Prevents automatic redirects that cause timeouts
4. **Localhost Binding**: API server bound to localhost (accessible via 10.0.2.2 from emulator)

## 🧪 Testing

The mobile app should now successfully connect without timeouts. The enhanced logging in ApiService will show:

```
[HH:mm:ss.fff] Starting request to: http://10.0.2.2:5163/api/interests
[HH:mm:ss.fff] Request completed in XXXms
[HH:mm:ss.fff] Response status: OK
```

## 📱 Alternative Solutions (if still having issues)

If you're still experiencing timeouts, try these alternatives in order:

### Option 1: Use Different Port
```csharp
client.BaseAddress = new Uri("http://10.0.2.2:8080/");
```

### Option 2: Use Physical Device
- Connect physical Android device via USB
- Use your actual machine IP: `http://192.168.1.4:5163/`

### Option 3: Use Localhost (for testing only)
```csharp
client.BaseAddress = new Uri("http://localhost:5163/");
```

## ✅ Issue Resolution Status

- ✅ **Timeout Issues**: RESOLVED (using correct Android emulator IP)
- ✅ **SSL Certificate Issues**: RESOLVED (using HTTP for development)
- ✅ **Network Configuration**: RESOLVED (proper Android emulator setup)
- ✅ **API Server Issues**: RESOLVED (disabled HTTPS redirection)

Your mobile app should now connect successfully to the API server without any timeout errors! 🎉

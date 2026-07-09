# Installation Guide - Ragnarok Custom Patcher

---

## 📋 Prerequisites

- **.NET 6 SDK** (for building C# client)
  - Download: https://dotnet.microsoft.com/download
  
- **Node.js 14+** (for Login Server)
  - Download: https://nodejs.org/
  
- **Ragnarok Game Client** (Ragexe.exe)

- **Patch Files** (.thor files created by ThorGenerator)

---

## 🚀 Step 1: Setup Login Server

### 1.1 Navigate to LoginServer folder
```bash
cd LoginServer
```

### 1.2 Install dependencies
```bash
npm install
```

### 1.3 Create .env file (optional)
```bash
echo PORT=3000 > .env
echo JWT_SECRET=your_secret_key_here >> .env
```

### 1.4 Start the server
```bash
npm start
```

**Expected Output:**
```
✅ Connected to SQLite database
✅ Users table initialized
✅ Characters table initialized
✅ Default admin account created (username: admin, password: admin123)
🚀 Login Server running at http://localhost:3000
📝 API Endpoint: http://localhost:3000/api/login
⚕️  Health Check: http://localhost:3000/health
```

**Default Account:**
- Username: `admin`
- Password: `admin123`

---

## 🖥️ Step 2: Setup Patch Server

### 2.1 Create patch folder structure

**Windows (Command Prompt):**
```cmd
mkdir PatchServer\patches
mkdir PatchServer\videos
```

**Linux/Mac (Terminal):**
```bash
mkdir -p PatchServer/patches
mkdir -p PatchServer/videos
```

### 2.2 Place your .thor patch files

Copy your `.thor` files into `PatchServer/patches/` folder:
```
PatchServer/
├── patches/
│   ├── patch_1.thor
│   ├── patch_2.thor
│   ├── patch_3.thor
│   └── plist.txt  (already created)
└── videos/
    └── loading.mp4 (optional)
```

### 2.3 Update plist.txt

Edit `PatchServer/plist.txt` to list your patches:
```
1,patch_1.thor
2,patch_2.thor
3,patch_3.thor
```

### 2.4 Serve patches via HTTP

**Option A: Using Python (Recommended)**
```bash
cd PatchServer
python -m http.server 80
```

**Option B: Using Node.js**
```bash
cd PatchServer
npx http-server -p 80
```

**Option C: Using IIS (Windows)**
- Create a new IIS website pointing to `PatchServer` folder
- Set port to 80 (or any port you prefer)

**Expected Result:**
- Patches accessible at: `http://localhost/patches/plist.txt`
- Test: Open browser and visit `http://localhost/patches/plist.txt`

---

## 💻 Step 3: Build & Run Patcher Client

### 3.1 Navigate to PatcherClient
```bash
cd PatcherClient
```

### 3.2 Restore dependencies
```bash
dotnet restore
```

### 3.3 Build the project
```bash
dotnet build --configuration Release
```

### 3.4 Run the patcher
```bash
dotnet run
```

**Or run the compiled executable:**
```bash
.\bin\Release\net6.0-windows\PatcherClient.exe
```

---

## 🎮 Step 4: Test the Patcher

### 4.1 Login
- Username: `admin`
- Password: `admin123`
- Click "Login"

### 4.2 Check for Updates
- Patcher will automatically check for updates from `http://localhost/patches/plist.txt`
- If updates available, "Download Patches" button will be enabled

### 4.3 Download Patches
- Click "Download Patches"
- Patches will download to `./patches/` folder
- Progress bar shows download progress

### 4.4 Launch Game
- After patches are updated, click "Launch Game"
- Ragexe.exe will launch

---

## 🔧 Configuration

### PatcherClient Settings

Edit `PatcherClient/Config/Settings.json`:
```json
{
  "AppName": "Ragnarok Online",
  "GameExecutable": "Ragexe.exe",
  "Servers": {
    "LoginServer": "http://localhost:3000/api/login",
    "PatchServer": "http://localhost/patches/"
  }
}
```

**Change these if your servers are on different machines:**
- `LoginServer` → Your login server URL
- `PatchServer` → Your patch server URL
- `GameExecutable` → Path to your Ragnarok client

---

## 🧪 API Testing

### Test Login Server Health
```bash
curl http://localhost:3000/health
```

Expected Response:
```json
{
  "status": "OK",
  "timestamp": "2024-01-01T12:00:00.000Z"
}
```

### Test Login
```bash
curl -X POST http://localhost:3000/api/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

Expected Response:
```json
{
  "success": true,
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@ragnarok.local"
  }
}
```

### Test Patch Server
```bash
curl http://localhost/patches/plist.txt
```

Expected Response:
```
1,patch_1.thor
2,patch_2.thor
3,patch_3.thor
```

---

## 🐛 Troubleshooting

### Login Server won't start
- Check if port 3000 is available: `netstat -ano | findstr :3000` (Windows)
- Kill process if needed: `taskkill /PID <PID> /F`
- Change PORT in `.env` file

### Patcher can't connect to Login Server
- Check Login Server is running on port 3000
- Test: `curl http://localhost:3000/health`
- Check firewall settings

### Patcher can't download patches
- Verify Patch Server is running on port 80
- Check plist.txt is accessible: `curl http://localhost/patches/plist.txt`
- Ensure .thor files exist in patches folder

### Ragexe.exe won't launch
- Check if Ragexe.exe exists in the patcher directory
- Run from command line to see detailed error

---

## 📝 Notes

- Default admin account should be changed in production
- Use HTTPS URLs in production environment
- Set strong JWT_SECRET in LoginServer
- Regular backup of SQLite database (database.db)

---

## ✅ Checklist

- [ ] Login Server running on port 3000
- [ ] Patch Server running on port 80
- [ ] .thor patch files in PatchServer/patches/
- [ ] plist.txt properly configured
- [ ] Ragexe.exe in game directory
- [ ] Default admin account tested
- [ ] Patches downloadable from patcher

---

**Installation Complete! 🎉**

Enjoy your Ragnarok Patcher!

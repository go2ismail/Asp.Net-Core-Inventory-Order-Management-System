# Docker Deployment — Step-by-Step Guide (Ubuntu VPS)

This guide will walk you through deploying **Free WMS (Inventory Order Management System)** on an Ubuntu VPS using Docker, from scratch.  
Designed for .NET developers who are new to Ubuntu/Linux.

---

## 📋 System Requirements

- **VPS with Ubuntu** (22.04 LTS or newer recommended)
- **Minimum 2 GB RAM** (SQL Server Express needs ~1 GB)
- **Minimum 20 GB storage**
- **Root / sudo access**
- **Port 5000 open** (to access the application)

---

## 🚀 Step 1 — SSH into Your VPS

Connect to your VPS using SSH:

```bash
ssh username@your-vps-ip-address
```

Example:
```bash
ssh root@123.123.123.123
```

> ❓ **What is SSH?**  
> SSH is a way to access your VPS terminal remotely, like Remote Desktop but via text.

---

## 🚀 Step 2 — Update Package Manager

Update the Ubuntu package manager (apt):

```bash
sudo apt update
```

```bash
sudo apt upgrade -y
```

> This refreshes the list of available software and upgrades existing packages.

---

## 🚀 Step 3 — Install Docker

Docker runs your application inside isolated containers.

```bash
# Install Docker prerequisites
sudo apt install -y ca-certificates curl

# Add Docker's official GPG key
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

# Add Docker repository
echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

# Update again (because new repository was added)
sudo apt update

# Install Docker Engine, CLI, and Compose
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

> This installs Docker and Docker Compose. Docker Compose lets you run multiple containers at once (app + database).

---

## 🚀 Step 4 — Start & Enable Docker

```bash
# Start Docker now
sudo systemctl start docker

# Enable Docker to auto-start on system boot
sudo systemctl enable docker
```

---

## 🚀 Step 5 — Verify Docker Installation

```bash
# Check Docker version
sudo docker --version

# Check Docker Compose version
sudo docker compose version
```

**Expected output** (example):
```
Docker version 27.0.0, build abc123
Docker Compose version v2.28.0
```

---

## 🚀 Step 6 — Clone the Repository

Clone the Free WMS project from GitHub to your VPS:

```bash
# Go to home directory
cd ~

# Clone the repository
git clone https://github.com/go2ismail/Asp.Net-Core-Inventory-Order-Management-System.git

# Enter the project folder
cd Asp.Net-Core-Inventory-Order-Management-System
```

---

## 🚀 Step 7 — Check Project Files

Make sure the Docker files exist:

```bash
ls -la
```

**You should see these files:**
- `Dockerfile` ✅ — instructions to build the app container
- `docker-compose.yml` ✅ — configuration to run app + database
- `.dockerignore` ✅ — files excluded from the Docker build

---

## 🚀 Step 8 — Create the `.env` File (Required)

This guide deploys the **production stack**: `docker-compose.yml` sets `IsDemoVersion=false`, so the published demo credentials are rejected. The application does **not** ship any production secret; they must be supplied by the deployment. Docker Compose reads the `.env` file next to `docker-compose.yml`:

```bash
# Generate a random signing key and store it in .env
# PowerShell is available on Ubuntu as pwsh, otherwise use openssl.
# With pwsh, one command writes both secrets and IsDemoVersion=false:
#   pwsh scripts/new-secrets.ps1

echo "IsDemoVersion=false" > .env
echo "Jwt__Key=$(openssl rand -base64 48 | tr '+/' '-_' | tr -d '=')" >> .env

# Password of the administrator account (admin@root.com) that is created on the first start up.
# At least 12 characters. Do not reuse the password that earlier releases published.
echo "AspNetIdentity__DefaultAdmin__Password=$(openssl rand -base64 24 | tr -d '/+=' | cut -c1-20)" >> .env

# Make sure only your user can read the secrets
chmod 600 .env

# Verify that both values are present (do not paste them into chat or tickets)
grep -c '^Jwt__Key=' .env
grep -c '^AspNetIdentity__DefaultAdmin__Password=' .env
```

> **Why?** A signing key that is stored in the repository is public knowledge, and anybody could mint tokens with the `Users` role. The same is true for a published administrator password. The `.env` file is ignored by git and by Docker, so the secrets stay on the server.
> **Note:** `docker compose` stops with `Jwt__Key is not set` or `AspNetIdentity__DefaultAdmin__Password is not set` when this file is missing. Every deployment needs its own values; changing the key signs out all users.
> **Upgrading an existing deployment:** when the container's administrator account still uses the password that earlier releases published, the value in `.env` replaces it at the next start up (check `sudo docker logs wms-api` for the rotation warning).
> **Just want to try the app?** Do not use this production stack with demo values. Run `sudo docker compose -f docker-compose.demo.yml up -d --build` instead; it starts with `IsDemoVersion=true` and the documented `admin@root.com` / `123456` demo login. Never store real data in it.

---

## 🚀 Step 9 — Run Docker Compose

This is the most important step — it starts the application and the database:

```bash
sudo docker compose up -d
```

> **What happens?**
> - `docker compose` = run based on `docker-compose.yml` configuration
> - `up` = build image (if not exists) + start containers
> - `-d` = run in background (detached), your terminal stays free

**Process:**
1. **Download SQL Server Express image** (~300 MB) — first time only
2. **Build the application image** from `Dockerfile` — every time code changes
3. **Start SQL Server container** — waits until SQL is ready (health check)
4. **Start API container** — after SQL is ready, the app connects to the database

**The download/build process may take 3-10 minutes depending on your VPS internet speed.**

---

## 🚀 Step 10 — Check Container Status

```bash
# List running containers
sudo docker ps
```

**Expected output:**
```
CONTAINER ID   IMAGE                      STATUS         PORTS                    NAMES
abc123         free-wms-api:latest        Up 2 minutes   0.0.0.0:5000->5000/tcp   wms-api
def456         mcr.microsoft.com/mssql/server:2022-express   Up 2 minutes   1433/tcp                 wms-sqlserver
```

> Both containers must show status **"Up"** (not "Exited" or "Restarting").

---

## 🚀 Step 11 — View Logs (if there are issues)

If something goes wrong, check the logs:

```bash
# Application logs
sudo docker logs wms-api

# SQL Server logs
sudo docker logs wms-sqlserver

# Follow real-time logs (Ctrl+C to exit)
sudo docker logs -f wms-api
```

---

## 🚀 Step 12 — Access the Application

Open a browser on your computer and go to:

```
http://your-vps-ip-address:5000
```

Example:
```
http://123.123.123.123:5000
```

> ⚠️ **IMPORTANT:** Make sure port 5000 is allowed in your **VPS firewall** (usually configured in your cloud provider panel like DigitalOcean, Linode, AWS, etc.).

**Login credentials for the administrator account:**
- **Email:** `admin@root.com` (or the `AspNetIdentity__DefaultAdmin__Email` you set)
- **Password:** the `AspNetIdentity__DefaultAdmin__Password` value from your `.env` file

> ⚠️ Never publish this password: the repository does not ship one, and a deployment that still uses the password of an earlier release rotates it on the next start up.

---

## 🛑 Step 13 — Stop / Start the Application

```bash
# Stop containers (app & database still exist, data is safe)
sudo docker compose stop

# Start again
sudo docker compose start

# Stop + remove containers (data stays safe in volumes)
sudo docker compose down

# Stop + remove containers + remove volumes (⚠️ DATA LOST)
sudo docker compose down -v
```

---

## 🔄 Step 14 — Update to a New Version

When there are code changes on GitHub, update the app:

```bash
# Go to project folder
cd ~/Asp.Net-Core-Inventory-Order-Management-System

# Pull latest changes from GitHub
git pull

# Rebuild the app image & restart containers
sudo docker compose up -d --build
```

> `--build` tells Docker to rebuild the application image with the latest code.

---

## 💾 Step 15 — Backup the Database

To backup the SQL Server database:

```bash
# Backup to .bak file inside the container
sudo docker exec wms-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong!Passw0rd" -C \
  -Q "BACKUP DATABASE [WHMS-LTE-FS] TO DISK = N'/var/opt/mssql/backup.bak'"

# Copy the backup file from container to VPS host
sudo docker cp wms-sqlserver:/var/opt/mssql/backup.bak ~/backup-wms.bak

# Download backup to your local computer (via SCP)
# From your local computer, run:
# scp root@123.123.123.123:~/backup-wms.bak .
```

---

## 🧹 Step 16 — Uninstall (remove everything)

```bash
# Stop & remove containers + volumes (⚠️ DATA LOST)
cd ~/Asp.Net-Core-Inventory-Order-Management-System && sudo docker compose down -v

# Remove Docker image
sudo docker rmi free-wms-api

# Remove project folder
cd ~ && rm -rf ~/Asp.Net-Core-Inventory-Order-Management-System

# Uninstall Docker (if no longer needed)
sudo apt remove -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
```

---

## ❓ Troubleshooting

### **1. Container keeps restarting**
```bash
sudo docker logs wms-api
```
Usually because SQL Server isn't ready yet. The health check is already configured, so wait 30-60 seconds.

### **2. Port 5000 is already in use**
```bash
# Check what's using port 5000
sudo lsof -i :5000

# Stop the process or change the port in docker-compose.yml
```

### **3. "Cannot connect to SQL Server"**
Make sure SQL Server is ready:
```bash
sudo docker logs wms-sqlserver
```
Wait until you see: `SQL Server is now ready for client connections.`

### **4. VPS is running out of disk space**
```bash
# Check disk usage
df -h

# Clean up unused Docker data
sudo docker system prune -a
```

### **5. Permission denied with Docker**
Always use `sudo` before Docker commands, or add your user to the docker group:
```bash
sudo usermod -aG docker $USER
```
Then log out and log back in.

---

## 📁 Docker-Related File Structure

```
Asp.Net-Core-Inventory-Order-Management-System/
├── Dockerfile                     ← Build instructions for the app image
├── docker-compose.yml             ← Production stack (IsDemoVersion=false)
├── docker-compose.demo.yml        ← Self-contained demo stack (IsDemoVersion=true)
├── .dockerignore                  ← Files excluded from the build
├── README_DOCKER_DEPLOYMENT.md    ← This guide
└── Presentation/
    └── ASPNET/
        ├── appsettings.json       ← Dev config (SQL via localhost, demo mode by default)
        └── appsettings.Docker.json ← Docker config (SQL via service name, production mode)
```

---

## ✅ Quick Command Reference

| Command | Description |
|----------|-------------|
| `sudo docker compose up -d` | Start app + database |
| `sudo docker compose down` | Stop & remove containers |
| `sudo docker compose logs -f wms-api` | View real-time app logs |
| `sudo docker compose up -d --build` | Rebuild & restart after code update |
| `sudo docker ps` | Check container status |
| `sudo docker compose stop` | Stop containers (data safe) |
| `sudo docker compose start` | Start containers again |

---

**Good luck! 🚀**  
If you have questions, please open an issue on the GitHub repository.
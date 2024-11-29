# SmartWeather
Personal project that aims to create a complete IOT environment for my own gardening system

## Table of content

1. [Install the pre-requisites](#installation)
2. [Prepare the application](#prepare)
3. [Helm](#3-helm)
5. [Final Verification](#final-verification)

---

## Install the pre-requisites <a name="installation"></a>

Follow these steps to install **Docker**, **Minikube**, **Helm**, and **Make**.

1. [Docker](#1-docker)
   - [Windows](#docker-windows)
   - [Linux](#docker-linux)
2. [Minikube](#2-minikube)
   - [Windows](#minikube-windows)
   - [Linux](#minikube-linux)
3. [Helm](#3-helm)
   - [Windows](#helm-windows)
   - [Linux](#helm-linux)
4. [Make](#4-make)
   - [Windows](#make-windows)
   - [Linux](#make-linux)
5. [Final Verification](#final-verification)

---

### 1. Docker <a name="1-docker"></a>

#### Windows <a name="docker-windows"></a>
1. **Download Docker Desktop**: [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop/).
2. **Install Docker Desktop**:
   - Run the installer.
   - Follow the on-screen instructions.
   - Enable **WSL2** if prompted.
3. **Test the installation**:
   ```bash
   docker --version
   ```

#### Linux <a name="docker-linux"></a>
1. **Install Docker**:
   - Update your packages:
     ```bash
     sudo apt update && sudo apt upgrade -y
     ```
   - Install Docker:
     ```bash
     sudo apt install -y docker.io
     ```
2. **Enable Docker on startup**:
   ```bash
   sudo systemctl enable docker
   sudo systemctl start docker
   ```
3. **Test the installation**:
   ```bash
   docker --version
   ```

---

### 2. Minikube <a name="2-minikube"></a>

#### Windows <a name="minikube-windows"></a>
1. **Download Minikube**:
   - Using **choco** (if Chocolatey is installed):
     ```bash
     choco install minikube
     ```
   - Or download from: [Minikube Releases](https://github.com/kubernetes/minikube/releases).
2. **Test the installation**:
   ```bash
   minikube version
   ```

#### Linux <a name="minikube-linux"></a>
1. **Install Minikube**:
   - Download the binary:
     ```bash
     curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
     ```
   - Make it executable and move it to `/usr/local/bin`:
     ```bash
     chmod +x minikube-linux-amd64
     sudo mv minikube-linux-amd64 /usr/local/bin/minikube
     ```
2. **Test the installation**:
   ```bash
   minikube version
   ```

---

### 3. Helm <a name="3-helm"></a>

#### Windows <a name="helm-windows"></a>
1. **Install Helm**:
   - Using **choco**:
     ```bash
     choco install kubernetes-helm
     ```
   - Or download from: [Helm Releases](https://github.com/helm/helm/releases).
2. **Test the installation**:
   ```bash
   helm version
   ```

#### Linux <a name="helm-linux"></a>
1. **Install Helm**:
   - Download the installation script:
     ```bash
     curl https://raw.githubusercontent.com/helm/helm/main/scripts/get-helm-3 | bash
     ```
2. **Test the installation**:
   ```bash
   helm version
   ```

---

### 4. Make <a name="4-make"></a>

#### Windows <a name="make-windows"></a>
1. **Download Make**:
   - Go to the [GNU Make for Windows](http://gnuwin32.sourceforge.net/packages/make.htm) page.
2. **Install Make**:
   - Download the setup or zip file and extract it to a folder like `C:\Program Files\make`.
3. **Add Make to PATH**:
   - Open **Advanced System Settings** > **Environment Variables**.
   - In `Path`, add the path to the `bin` folder inside the extracted directory (e.g., `C:\Program Files\make\bin`).
4. **Test the installation**:
   ```bash
   make --version
   ```

#### Linux <a name="make-linux"></a>
1. **Install Make**:
   - On Debian-based systems:
     ```bash
     sudo apt update
     sudo apt install -y make
     ```
   - On Red Hat-based systems:
     ```bash
     sudo yum install -y make
     ```
2. **Test the installation**:
   ```bash
   make --version
   ```

---

### Final Verification <a name="final-verification"></a>

1. **Docker**:
   ```bash
   docker --version
   ```
2. **Minikube**:
   ```bash
   minikube version
   ```
3. **Helm**:
   ```bash
   helm version
   ```
4. **Make**:
   ```bash
   make --version
   ```

## Prepare the application <a name="prepare"></a>

Follow these steps to **build local images** and **setup kubernetes env**.

1. [Build local images](#1-local-images)
2. [Setup kubernetes env](#2-setup-kube)

---

### 1. Build local images <a name="1-local-images"></a>

**A Makefile is here to automate the process** :
```bash
make gen-imgs
```
_This command will automatically generate all necessary images for your cluster to run_

Additionnaly, if you want to generate only specific images or specific layer images, go check :
```bash
make gen-help
```

### 2. Setup kubernetes env <a name="2-setup-kube"></a>

**A Makefile is here to automate the process** :
```bash
make kube-init
```
_This command will automatically generate all necessary parameters for your minikube cluster_

Additionnaly, if you want to look at the specific actions, go check :
```bash
make kube-help
```


kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/deploy.yaml

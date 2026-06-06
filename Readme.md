# 🎯 AR Body Tracking Demo
> Real-time upper body pose detection overlaid on a live webcam feed, built with Unity 6 and MediaPipe.

![Unity](https://img.shields.io/badge/Unity-6000.x-black?logo=unity)
![Python](https://img.shields.io/badge/Python-3.10+-blue?logo=python)
![MediaPipe](https://img.shields.io/badge/MediaPipe-0.10.9-orange)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey?logo=windows)

---

## 📸 Overview

This application tracks a user's upper body in real-time using a webcam. A Python backend runs MediaPipe pose estimation and streams joint coordinates over WebSocket to a Unity frontend, which renders visual markers over a live webcam feed.

**Tracked Joints:**
- 👤 Head (Nose)
- 💪 Left & Right Shoulder
- 💪 Left & Right Elbow
- 🤚 Left & Right Wrist

---

## 🏗️ Architecture

> *(Insert Eraser.io diagram here)*

| Layer | Technology | Role |
|-------|-----------|------|
| Capture | OpenCV | Webcam frame grabbing |
| Estimation | MediaPipe | Pose landmark detection |
| Transport | WebSocket | Real-time JSON data stream |
| Rendering | Unity 6 URP | Webcam display + joint markers |

---

## 🛠️ Requirements

### System
- Windows 10/11
- Webcam (built-in or external)
- Python 3.10+
- Unity 6 (6000.x)

### Python Dependencies
- `mediapipe==0.10.9`
- `opencv-python`
- `websockets`

### Unity Packages
- NativeWebSocket
- TextMeshPro
- Universal Render Pipeline (URP)

---

## ⚙️ Setup & Installation

### Step 1 — Clone the Repository
```bash
git clone https://github.com/yourusername/ARBodyTracking.git
cd ARBodyTracking
```

### Step 2 — Python Setup

**Install Python 3.10+** from [python.org](https://python.org) if not already installed.

**Install dependencies:**
```bash
pip install mediapipe==0.10.9 opencv-python websockets
```

**Verify installation:**
```bash
python -c "import mediapipe; import cv2; import websockets; print('All good!')"
```

### Step 3 — Run the Python Server

Navigate to the scripts folder and run:
```bash
python pose_server.py
```

You should see:
Starting WebSocket server on ws://localhost:8765
INFO: Created TensorFlow Lite XNNPACK delegate for CPU.

An OpenCV preview window will open showing your webcam feed with skeleton overlay.

> ⚠️ **Keep this terminal open** while using the Unity app.

### Step 4 — Unity Setup

1. Open **Unity Hub**
2. Click **Open** → select the cloned project folder
3. Let Unity import all assets
4. Open scene: `Assets/Scenes/SampleScene`
5. Hit **Play**

> ✅ The Unity window will show your webcam feed with green joint markers overlaid on your body.

---

## 🎮 How to Use

1. Run `pose_server.py` first
2. Launch Unity and hit Play
3. Stand in front of your webcam
4. Make sure your **upper body is fully visible**
5. Green markers will appear on your joints in real-time

**Tips for best tracking:**
- Good lighting improves detection accuracy
- Step back so shoulders and elbows are fully in frame
- Avoid busy backgrounds if possible

---

## 📁 Project Structure

ARBodyTracking/
├── Assets/
│   ├── Scripts/
│   │   ├── WebcamDisplay.cs       # Webcam feed → RawImage
│   │   ├── WebSocketClient.cs     # Receives joint data from Python
│   │   ├── PoseManager.cs         # Maps joints → Canvas positions
│   │   └── JointMarker.cs         # UI marker behaviour
│   ├── Prefabs/
│   │   └── JointMarkerUI.prefab   # Green UI dot marker
│   ├── Scenes/
│   │   └── SampleScene.unity
│   └── Materials/
├── pose_server.py                 # Python MediaPipe + WebSocket server
└── README.md
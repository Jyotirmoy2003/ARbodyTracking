import cv2
import mediapipe as mp
from mediapipe.tasks import python
from mediapipe.tasks.python import vision
import asyncio
import websockets
import json
import threading
# MediaPipe setup
mp_pose = mp.solutions.pose
pose = mp_pose.Pose(
    static_image_mode=False,
    model_complexity=1,
    smooth_landmarks=True,
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5
)

TRACKED_JOINTS = {
    "nose":           0,
    "left_shoulder":  11,
    "right_shoulder": 12,
    "left_elbow":     13,
    "right_elbow":    14,
    "left_wrist":     15,
    "right_wrist":    16,
}

latest_joints = []
clients = set()

def capture_loop():
    global latest_joints
    cap = cv2.VideoCapture(0)
    cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
    cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720)

    while True:
        ret, frame = cap.read()
        if not ret:
            continue

        rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = pose.process(rgb)

        joints = []
        if results.pose_landmarks:
            landmarks = results.pose_landmarks.landmark
            for name, idx in TRACKED_JOINTS.items():
                lm = landmarks[idx]
                joints.append({
                    "name": name,
                    "x": lm.x,
                    "y": lm.y,
                    "visibility": lm.visibility
                })

        latest_joints = joints

        # Optional: show preview window
        mp.solutions.drawing_utils.draw_landmarks(
            frame, results.pose_landmarks, mp_pose.POSE_CONNECTIONS
        )
        cv2.imshow("Pose Preview", frame)
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()

async def send_joints(websocket):
    clients.add(websocket)
    print(f"Client connected: {websocket.remote_address}")
    try:
        while True:
            if latest_joints:
                payload = json.dumps({"joints": latest_joints})
                await websocket.send(payload)
            await asyncio.sleep(1/30)  # 30fps send rate
    except websockets.exceptions.ConnectionClosed:
        print("Client disconnected")
    finally:
        clients.discard(websocket)

async def main():
    print("Starting WebSocket server on ws://localhost:8765")
    async with websockets.serve(send_joints, "localhost", 8765):
        await asyncio.Future()

if __name__ == "__main__":
    # Run capture in background thread
    thread = threading.Thread(target=capture_loop, daemon=True)
    thread.start()

    # Run websocket server
    asyncio.run(main())
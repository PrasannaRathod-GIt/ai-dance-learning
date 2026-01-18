import cv2
import mediapipe as mp
import json
import os

class PoseExtractor:
    def __init__(self):
        self.mp_pose = mp.solutions.pose
        self.pose = self.mp_pose.Pose(
            static_image_mode=False,
            model_complexity=1,
            min_detection_confidence=0.5
        )

    def extract_from_video(self, video_path: str, output_path: str):
        if not os.path.exists(video_path):
            return {"error": f"Video not found at {video_path}"}

        cap = cv2.VideoCapture(video_path)
        all_frames = []

        while cap.isOpened():
            ret, frame = cap.read()
            if not ret:
                break

            # Convert to RGB for MediaPipe
            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = self.pose.process(rgb_frame)

            if results.pose_landmarks:
                # Extract 33 landmarks: x, y, z (depth), and visibility
                landmarks = [
                    {"x": lm.x, "y": lm.y, "z": lm.z, "v": lm.visibility}
                    for lm in results.pose_landmarks.landmark
                ]
                all_frames.append(landmarks)

        cap.release()

        # Ensure output folder exists
        os.makedirs(os.path.dirname(output_path), exist_ok=True)
        with open(output_path, 'w') as f:
            json.dump(all_frames, f)

        return {"frames_processed": len(all_frames), "saved_to": output_path}
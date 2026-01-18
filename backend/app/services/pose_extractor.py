import cv2
import json
import os
import mediapipe as mp

class PoseExtractor:
    def __init__(self):
        # Direct access to the solution
        self.mp_pose = mp.solutions.pose
        self.pose_tracker = self.mp_pose.Pose(
            static_image_mode=False,
            model_complexity=1,
            min_detection_confidence=0.5,
            min_tracking_confidence=0.5
        )

    def extract_pose(self, video_path, output_json_path):
        if not os.path.exists(video_path):
            return False

        cap = cv2.VideoCapture(video_path)
        all_frames_data = []
        frame_idx = 0

        while cap.isOpened():
            success, frame = cap.read()
            if not success:
                break

            rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = self.pose_tracker.process(rgb_frame)

            if results.pose_landmarks:
                landmarks = [{"id": i, "x": float(l.x), "y": float(l.y), "z": float(l.z)} 
                             for i, l in enumerate(results.pose_landmarks.landmark)]
                all_frames_data.append({"frame": frame_idx, "landmarks": landmarks})

            frame_idx += 1

        cap.release()
        os.makedirs(os.path.dirname(output_json_path), exist_ok=True)
        with open(output_json_path, 'w') as f:
            json.dump(all_frames_data, f)
        return True
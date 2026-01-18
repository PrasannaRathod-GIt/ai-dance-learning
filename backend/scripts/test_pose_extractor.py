import cv2
import mediapipe as mp
import json
import os

def extract_pose_from_video(video_path, output_path):
    mp_pose = mp.solutions.pose
    pose = mp_pose.Pose(static_image_mode=False, min_detection_confidence=0.5)
    
    cap = cv2.VideoCapture(video_path)
    all_frames_data = []

    while cap.isOpened():
        ret, frame = cap.read()
        if not ret: break

        # Convert BGR to RGB for MediaPipe
        image_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = pose.process(image_rgb)

        if results.pose_landmarks:
            # Map the 33 landmarks into a dictionary
            frame_landmarks = []
            for lm in results.pose_landmarks.landmark:
                frame_landmarks.append({"x": lm.x, "y": lm.y, "z": lm.z, "v": lm.visibility})
            all_frames_data.append(frame_landmarks)

    cap.release()

    # Ensure directory exists and save JSON
    os.makedirs(os.path.dirname(output_path), exist_ok=True)
    with open(output_path, 'w') as f:
        json.dump(all_frames_data, f)
    
    return True
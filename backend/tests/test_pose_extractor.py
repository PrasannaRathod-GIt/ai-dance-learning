from backend.app.services.pose_extractor import PoseExtractor

extractor = PoseExtractor()

extractor.extract_from_video(
    video_path="backend/samples/sample_video.mp4",
    output_path="backend/samples/output_pose.json"
)

print("Pose extraction completed")

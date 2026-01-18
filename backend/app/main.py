from fastapi import FastAPI
from app.services.pose_extractor import PoseExtractor
import os

app = FastAPI()
extractor = PoseExtractor()

@app.get("/")
def home():
    return {"status": "AI Dance Backend is Running"}

@app.get("/test-process")
def test_process():
    # Adjusted paths to match your root structure
    video_path = "../samples/sample_video.mp4"
    output_path = "data/pose_json/sample_pose_json.json"
    
    result = extractor.extract_from_video(video_path, output_path)
    return result
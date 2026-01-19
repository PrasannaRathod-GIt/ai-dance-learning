# endpoints.py
# API v1 endpoints (minimal stubs + small test helpers).

from fastapi import APIRouter, HTTPException
from fastapi.responses import FileResponse, JSONResponse
from pydantic import BaseModel
from typing import Dict
from pathlib import Path
import json
import traceback

router = APIRouter()

@router.get("/health", tags=["health"])
def health() -> Dict[str, str]:
    """Simple health check endpoint."""
    return {"status": "ok", "service": "ai-dance-learning backend (minimal)"}

class SiteInfo(BaseModel):
    message: str

@router.get("/info", response_model=SiteInfo, tags=["info"])
def info():
    """Short info endpoint."""
    return SiteInfo(message="ai-dance-learning backend (test server)")

#
# --- Test endpoints to run extractor on a local sample video and fetch JSON ---
#
@router.post("/extract_sample", tags=["test"])
def extract_sample():
    """
    Run the PoseExtractor on samples/sample_video.mp4 (project-root/samples).
    Saves output to backend/data/pose_json/sample_pose.json and returns a small summary.
    """
    try:
        # locate project root reliably: endpoints.py is at backend/app/api/v1/
        project_root = Path(__file__).resolve().parents[4]
        sample_video = project_root / "samples" / "sample_video.mp4"
        output_dir = project_root / "backend" / "data" / "pose_json"
        output_dir.mkdir(parents=True, exist_ok=True)
        output_file = output_dir / "sample_pose.json"

        if not sample_video.exists():
            raise HTTPException(status_code=404, detail=f"Sample video not found at {sample_video}")

        # Import the extractor lazily so the app can start without mediapipe if not installed
        try:
            from backend.app.services.pose_extractor import PoseExtractor
        except Exception as e:
            # propagate useful message
            raise HTTPException(status_code=500, detail=f"Could not import PoseExtractor: {e}")

        extractor = PoseExtractor()
        success = extractor.extract_pose(str(sample_video), str(output_file))
        if not success:
            raise HTTPException(status_code=500, detail="Pose extraction failed (extractor returned False)")

        # return a brief summary
        # try to read some info (frame count) from JSON if possible
        try:
            with open(output_file, "r", encoding="utf-8") as f:
                data = json.load(f)
            frames = len(data) if isinstance(data, list) else (data.get("frames") and len(data["frames"])) or 0
        except Exception:
            frames = None

        return JSONResponse({"ok": True, "output": str(output_file), "frame_count_estimate": frames})
    except HTTPException:
        raise
    except Exception as exc:
        traceback_str = traceback.format_exc()
        raise HTTPException(status_code=500, detail=f"Unhandled error: {exc}\n{traceback_str}")

@router.get("/pose/sample", tags=["test"])
def get_sample_pose():
    """
    Return the generated sample pose JSON file (if present).
    """
    project_root = Path(__file__).resolve().parents[4]
    output_file = project_root / "backend" / "data" / "pose_json" / "sample_pose.json"
    if not output_file.exists():
        raise HTTPException(status_code=404, detail="Pose JSON not found. Run POST /api/v1/extract_sample first.")
    # return raw JSON file
    return FileResponse(path=str(output_file), media_type="application/json", filename="sample_pose.json")

# pose_schema.py
# Pydantic schemas / data shapes for pose and step JSON (placeholder).

from pydantic import BaseModel
from typing import List, Optional

class Keypoint(BaseModel):
    x: float
    y: float
    z: Optional[float] = None
    visibility: Optional[float] = None

class PoseFrame(BaseModel):
    timestamp_ms: int
    keypoints: List[Keypoint]

class PoseSequence(BaseModel):
    source: str
    duration_s: float
    frames: List[PoseFrame]

# AI Dance Learning/ with mediapipe backend

Design and build a web-based application that helps users learn dance step-by-step by converting a real dance video
into a structured tutorial using AI-driven pose analysis and a 3D humanoid animation. The system prioritizes smooth 
performance, clarity of movement, and explainable AI over perfect choreography replication.

🧩 Core Features -

1.Video Input

  a.Accepts YouTube video links only (30 seconds max)
  b.No direct video uploads
  c.Videos are restricted to:
    -Single dancer
    -Full body visible
    -Minimal camera movement

2.AI Pose Analysis

  a.Uses MediaPipe Pose for human pose estimation
  b.Extracts full-body keypoints (excluding fingers)
  c.Processes video at reduced FPS for performance
  d.Outputs time-stamped joint positions

3.Dance Step Segmentation

  a.Segments the dance into medium-granularity steps
  b.Uses motion-based rules:
   - Joint velocity changes
   - Movement pauses
   - Direction shifts
  c.Produces step intervals with associated pose data

4.Instruction Generation

  a.Generates dual-mode instructions:
    -Human-friendly (“Lift left leg”)
    -Technical (“Left hip shifts laterally”)
  b.Rule-based (no heavy LLM dependency
    Unity Humanoid Animation

5.Unity Humanoid Animation

  a.Uses Mixamo-style humanoid model
  b.Rigged with Unity Mecanim (Humanoid)
  c.Partial Inverse Kinematics (arms + legs)
  d.Replays extracted poses as smooth animations
  e.First shows full dance, then step-by-step tutorial

6.User Controls

  a.Slow-motion playback
  b.Repeat individual steps
  c.Step navigation

🛠️ Technology Stack

1.Frontend / Visualization-
Unity (WebGL build)
Unity Mecanim humanoid system
Partial IK for motion realism.

2.Backend / AI-
Python + FastAPI
MediaPipe Pose
yt-dlp for video fetching
Rule-based step segmentation

3.Data Exchange-
JSON-based pose and step data
Decoupled ML and animation layers

4.Hosting (Free Tier)-
Backend: Render / Railway
Assets & code: GitHub
Unity Personal License

⚙️Processing Flow

1.User submits YouTube link
2.Backend downloads video
3.MediaPipe extracts pose keypoints
4.Dance is segmented into steps
5.Instructions are generated
6.Unity loads pose data
7.Humanoid performs:
 -Full dance preview
 -Step-by-step tutorial.

🚀 Performance & Design Decisions
1.Video length capped at 30 seconds
2.Backend preprocessing (up to 4 minutes allowed)
3.Animation runs locally for smooth UX
4.Pose smoothing applied to reduce jitter
5.Focus on visual correctness, not biomechanical perfection
- working on Demo for starting

- ## 👩‍💻 Author

-Prasanna Rathod (student) pursuing bachelore's degree
Built as a learning-focused and portfolio-ready project.

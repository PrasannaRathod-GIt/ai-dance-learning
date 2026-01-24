using System;
using UnityEngine;

[Serializable]
public class Landmark {
    public int id;
    public float x;
    public float y;
    public float z;
    public float visibility;
}

[Serializable]
public class FrameData {
    public int frame;
    public Landmark[] landmarks;
}

[Serializable]
public class PoseData {
    public int fps = 30;
    public FrameData[] frames;
}

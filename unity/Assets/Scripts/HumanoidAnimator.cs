using UnityEngine;
using System.Collections;

public class PosePlayback : MonoBehaviour
{
    [Header("Settings")]
    public float delay = 0.05f;
    public float positionScale = 1.0f;
    
    [Header("References")]
    public Animator animator;
    public PoseImporter importer; // Ensure your importer class is accessible
    
    [HideInInspector] public bool playing;
    [HideInInspector] public int frameIndex;

    IEnumerator PlaybackLoop() {
        // Corrected: Added check for importer and poseData to prevent null errors
        while (playing && importer != null && importer.poseData != null && frameIndex < importer.poseData.frames.Length) {
            ApplyFrame(importer.poseData.frames[frameIndex]);
            frameIndex++;
            yield return new WaitForSeconds(delay);
        }
        playing = false;
    }

    public void ApplyFrame(FrameData frame) {
        if (frame == null || frame.landmarks == null) return;

        // For each mapped landmark, compute a target direction and set bone rotation.
        foreach (var lm in frame.landmarks) {
            Transform bone = PoseMapper.GetBoneTransform(animator, lm.id);
            if (bone == null) continue;

            // Convert normalized landmark (x,y) into a local-space position relative to model root.
            Vector3 localPos = new Vector3((lm.x - 0.5f) * positionScale, (0.5f - lm.y) * positionScale, lm.z * positionScale);

            // If the bone has a parent, derive a direction vector from parent to this target and set rotation.
            if (bone.parent != null) {
                Vector3 targetWorld = bone.root.TransformPoint(localPos);
                Vector3 dir = (targetWorld - bone.position);
                
                if (dir.sqrMagnitude > 1e-6f) {
                    Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    bone.rotation = Quaternion.Slerp(bone.rotation, targetRot, 0.5f); // smooth partially
                }
            } else {
                // fallback: nudge local position
                bone.localPosition = localPos;
            }
        }
    }

    // helper to step to a specific frame (used by UI controller)
    public void GotoFrame(int idx) {
        if (importer == null || importer.poseData == null) return;
        idx = Mathf.Clamp(idx, 0, importer.poseData.frames.Length - 1);
        ApplyFrame(importer.poseData.frames[idx]);
    }
}
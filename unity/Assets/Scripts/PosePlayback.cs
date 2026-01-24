using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PosePlayback_TEMP : MonoBehaviour

{
    [Header("Settings")]
    public float delay = 0.05f;
    public float positionScale = 1.0f;

    [Header("References")]
    public Animator animator;
    public PoseImporter importer; // Ensure your importer class is accessible

    [HideInInspector] public bool playing;
    [HideInInspector] public int frameIndex;

    private Coroutine playbackCoroutine;

    // Public control methods for UI
    public void StartPlayback(bool fromBeginning = true)
    {
        if (importer == null || importer.poseData == null || importer.poseData.frames == null) return;
        if (fromBeginning) frameIndex = 0;
        if (playing) return;
        playing = true;
        if (playbackCoroutine != null) StopCoroutine(playbackCoroutine);
        playbackCoroutine = StartCoroutine(PlaybackLoop());
    }

    public void StopPlayback()
    {
        playing = false;
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }
    }

    IEnumerator PlaybackLoop()
    {
        while (playing && importer != null && importer.poseData != null && frameIndex < importer.poseData.frames.Length)
        {
            ApplyFrame(importer.poseData.frames[frameIndex]);
            frameIndex++;
            yield return new WaitForSeconds(delay);
        }
        playing = false;
    }

    public void ApplyFrame(FrameData frame)
    {
        if (frame == null || frame.landmarks == null) return;

        foreach (var lm in frame.landmarks)
        {
            Transform bone = PoseMapper.GetBoneTransform(animator, lm.id);
            if (bone == null) continue;

            Vector3 localPos = new Vector3((lm.x - 0.5f) * positionScale, (0.5f - lm.y) * positionScale, lm.z * positionScale);

            if (bone.parent != null)
            {
                Vector3 targetWorld = bone.root.TransformPoint(localPos);
                Vector3 dir = (targetWorld - bone.position);

                if (dir.sqrMagnitude > 1e-6f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
                    bone.rotation = Quaternion.Slerp(bone.rotation, targetRot, 0.5f);
                }
            }
            else
            {
                bone.localPosition = localPos;
            }
        }
    }

    public void GotoFrame(int idx)
    {
        if (importer == null || importer.poseData == null) return;
        idx = Mathf.Clamp(idx, 0, importer.poseData.frames.Length - 1);
        ApplyFrame(importer.poseData.frames[idx]);
        frameIndex = idx;
    }
}

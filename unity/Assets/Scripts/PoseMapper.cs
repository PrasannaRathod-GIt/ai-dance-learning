using System.Collections.Generic;
using UnityEngine;


public static class PoseMapper {
// MediaPipe Pose landmark indices -> Unity HumanBodyBones
// This is a minimal mapping for core limbs (add more as needed)
public static readonly Dictionary<int, HumanBodyBones> IndexToBone = new Dictionary<int, HumanBodyBones>() {
// shoulders
{11, HumanBodyBones.LeftUpperArm}, // left_shoulder (mp index 11)
{12, HumanBodyBones.RightUpperArm}, // right_shoulder (mp index 12)
// elbows
{13, HumanBodyBones.LeftLowerArm}, // left_elbow
{14, HumanBodyBones.RightLowerArm}, // right_elbow
// wrists / hands
{15, HumanBodyBones.LeftHand}, // left_wrist
{16, HumanBodyBones.RightHand}, // right_wrist
// hips
{23, HumanBodyBones.LeftUpperLeg}, // left_hip
{24, HumanBodyBones.RightUpperLeg}, // right_hip
// knees
{25, HumanBodyBones.LeftLowerLeg}, // left_knee
{26, HumanBodyBones.RightLowerLeg}, // right_knee
// ankles / feet
{27, HumanBodyBones.LeftFoot}, // left_ankle
{28, HumanBodyBones.RightFoot} // right_ankle
};


public static Transform GetBoneTransform(Animator animator, int landmarkIndex) {
if (!IndexToBone.ContainsKey(landmarkIndex)) return null;
return animator.GetBoneTransform(IndexToBone[landmarkIndex]);
}
}
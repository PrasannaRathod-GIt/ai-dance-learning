using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PoseImporter : MonoBehaviour
{
    [Tooltip("Name of the JSON file inside StreamingAssets/poses/")]
    public string jsonFileName = "sample_pose.json";

    [HideInInspector]
    public PoseData poseData;

    public bool autoLoadOnStart = true;

    void Start()
    {
        if (autoLoadOnStart) StartCoroutine(LoadFromStreamingAssets());
    }

    public IEnumerator LoadFromStreamingAssets()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "poses", jsonFileName);

#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_IOS)
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            try
            {
                poseData = JsonUtility.FromJson<PoseData>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed parsing pose JSON: " + e);
            }
            yield break;
        }
#endif
        // For WebGL or other platforms, use UnityWebRequest
        using (UnityWebRequest uwr = UnityWebRequest.Get(path))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to load pose json: " + uwr.error + " path: " + path);
            }
            else
            {
                string json = uwr.downloadHandler.text;
                try
                {
                    poseData = JsonUtility.FromJson<PoseData>(json);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed parsing pose JSON: " + e);
                }
            }
        }
    }
}

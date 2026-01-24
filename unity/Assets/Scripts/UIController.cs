using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public PosePlayback_TEMP animatorController;

    public PoseImporter importer;

    public Button playPauseButton;
    public Button prevButton;
    public Button nextButton;
    public Slider frameSlider;
    public Text frameLabel;

    private bool isPlaying = false;

    void Start()
    {
        if (importer != null && importer.poseData != null && importer.poseData.frames != null)
        {
            frameSlider.maxValue = importer.poseData.frames.Length - 1;
            frameSlider.minValue = 0;
            frameSlider.wholeNumbers = true;
            UpdateFrameLabel(0);
        }

        if (playPauseButton != null) playPauseButton.onClick.AddListener(OnPlayPause);
        if (prevButton != null) prevButton.onClick.AddListener(OnPrev);
        if (nextButton != null) nextButton.onClick.AddListener(OnNext);
        if (frameSlider != null) frameSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnPlayPause()
    {
        if (!isPlaying)
        {
            animatorController?.StartPlayback(true);
            isPlaying = true;
            if (playPauseButton != null) playPauseButton.GetComponentInChildren<Text>().text = "Pause";
        }
        else
        {
            animatorController?.StopPlayback();
            isPlaying = false;
            if (playPauseButton != null) playPauseButton.GetComponentInChildren<Text>().text = "Play";
        }
    }

    void OnPrev()
    {
        animatorController?.StopPlayback();
        int idx = Mathf.Max(0, (int)frameSlider.value - 1);
        frameSlider.value = idx;
        animatorController?.GotoFrame(idx);
        UpdateFrameLabel(idx);
        isPlaying = false;
    }

    void OnNext()
    {
        animatorController?.StopPlayback();
        int idx = Mathf.Min((int)frameSlider.maxValue, (int)frameSlider.value + 1);
        frameSlider.value = idx;
        animatorController?.GotoFrame(idx);
        UpdateFrameLabel(idx);
        isPlaying = false;
    }

    void OnSliderChanged(float val)
    {
        int idx = (int)val;
        animatorController?.StopPlayback();
        animatorController?.GotoFrame(idx);
        UpdateFrameLabel(idx);
        isPlaying = false;
    }

    void UpdateFrameLabel(int idx)
    {
        if (frameLabel != null) frameLabel.text = $"Frame: {idx}";
    }
}

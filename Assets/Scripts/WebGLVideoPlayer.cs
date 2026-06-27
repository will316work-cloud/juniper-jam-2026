using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] private string _videoFileName;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, _videoFileName);
        videoPlayer.url = videoPath;
        videoPlayer.Prepare();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class WebcamDisplay : MonoBehaviour
{
    public RawImage displayImage;
    private WebCamTexture webcamTexture;

    void Start()
    {
        webcamTexture = new WebCamTexture(1280, 720, 30);
        displayImage.texture = webcamTexture;
        displayImage.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void OnDestroy()
    {
        if (webcamTexture != null)
            webcamTexture.Stop();
    }

    public WebCamTexture GetWebcamTexture() => webcamTexture;
}
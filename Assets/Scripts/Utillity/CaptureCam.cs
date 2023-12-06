using Capture;
using System.Collections;
using UnityEngine;
using System.IO;

public class CaptureCam : MonoBehaviour
{
    public Camera TargetCam;

    private Texture2D _capturedImage;

    public IEnumerator capture()
    {
        Rect lRect = new Rect(0f, 0f, Screen.width, Screen.height);
        if (_capturedImage)
            Destroy(_capturedImage);

        yield return new WaitForEndOfFrame();

        _capturedImage = zzTransparencyCapture.capture(lRect, TargetCam);

        byte[] textureByte = _capturedImage.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "Test.png", textureByte);
    }
}

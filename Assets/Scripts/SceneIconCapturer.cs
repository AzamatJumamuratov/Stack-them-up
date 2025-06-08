using UnityEngine;
using System.IO;

public class SceneIconCapturer : MonoBehaviour
{
    public Camera captureCamera;
    public RenderTexture renderTexture;
    public string fileName = "SceneIcon";

    [ContextMenu("Capture Icon")]
    public void Capture()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        captureCamera.Render();

        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        string path = Application.dataPath + "/" + fileName + ".png";
        File.WriteAllBytes(path, bytes);

        Debug.Log("Saved to: " + path);

        RenderTexture.active = currentRT;

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}

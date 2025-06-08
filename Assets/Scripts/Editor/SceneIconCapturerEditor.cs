using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneIconCapturer))]
public class SceneIconCapturerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SceneIconCapturer capturer = (SceneIconCapturer)target;
        if (GUILayout.Button("📸 Capture Icon"))
        {
            capturer.Capture();
        }
    }
}

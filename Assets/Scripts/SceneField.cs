using UnityEngine;

[System.Serializable]
public class SceneField
{
    [SerializeField] private Object m_SceneAsset;
    [SerializeField] private string m_SceneName = "";

    public string SceneName => m_SceneName;

    public static implicit operator string(SceneField sceneField)
    {
        return sceneField.SceneName;
    }
}

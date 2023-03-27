#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConvTextureMover))]
public class ConvTextureMoverEditor : Editor
{
    void OnSceneGUI()
    {
        DrawDefaultInspector();
        var player = (ConvTextureMover)target;
        player.RemakeMesh();
    }
}
#endif
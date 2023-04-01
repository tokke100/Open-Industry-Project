using UnityEngine;
using libplctag;
public class PLC : MonoBehaviour
{
    public string Gateway = string.Empty;

    public string Path = string.Empty;

    public int ScanTime = 0;

    public PlcType PlcType = new();

    public Protocol Protocol = new();

    private void Awake()
    {
#if UNITY_EDITOR

        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));

#endif
    }

}



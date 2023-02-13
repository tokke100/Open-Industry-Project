using libplctag.DataTypes;
using libplctag;
using UnityEngine;
using libplctag.NativeImport;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class Photoeye : MonoBehaviour
{
    new readonly Tag<SintPlcMapper, sbyte> tag = new();
    public string tagName;
    public float distance = 6.0f;
    int scanTime = 0;

    void Start()
    {
        plctag.ForceExtractLibrary = false;

        var _plc = GameObject.Find("PLC").GetComponent<PLC>();

        tag.Name = tagName;
        tag.Gateway = _plc.Gateway;
        tag.Path = _plc.Path;
        tag.PlcType = _plc.PlcType;
        tag.Protocol = _plc.Protocol;
        tag.Timeout = TimeSpan.FromSeconds(1);

        scanTime = _plc.ScanTime;

        try
        {
            InvokeRepeating(nameof(ScanTag), 0, (float)scanTime / 1000f);
        }
        catch (Exception)
        {
            Debug.LogError($"Failed to write to tag for object: {gameObject.name} check PLC object settings or Tag Name");
        }


    }
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out RaycastHit hit, distance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            tag.Value = 1;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 6, Color.red);
            tag.Value = 0;
        }
    }

    async Task ScanTag()
    {
        await tag.WriteAsync();
    }
}

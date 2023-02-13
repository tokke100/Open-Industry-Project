using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport; 
using System;
using UnityEditor;
using System.Threading.Tasks;

public class Belt : MonoBehaviour
{
    public string tagName;
    Vector3 startPos = new();
    Rigidbody rb;
    int speed = 0;
    int scantime = 0;
    new readonly Tag<DintPlcMapper, int> tag = new();

    void Start()
    {
        plctag.ForceExtractLibrary = false;

        var _plc = GameObject.Find("PLC").GetComponent<PLC>();

        tag.Name = tagName;
        tag.Gateway = _plc.Gateway;
        tag.Path= _plc.Path;
        tag.PlcType= _plc.PlcType;
        tag.Protocol= _plc.Protocol;
        tag.Timeout = TimeSpan.FromSeconds(1);

        scantime = _plc.ScanTime;

        rb = GetComponentInChildren<Rigidbody>();

        startPos = rb.GetComponent<Transform>().transform.position;

        InvokeRepeating(nameof(ScanTag), 0, (float)scantime/1000f);
    }
    void Update()
    {
        rb.velocity = rb.GetComponent<Transform>().transform.TransformDirection(Vector3.left) * speed;
        rb.GetComponent<Transform>().transform.position = startPos;

    }

    async Task ScanTag()
    {
        try
        {
            speed = await tag.ReadAsync();
        }
        catch (Exception)
        {
            CancelInvoke(nameof(ScanTag));
            Debug.LogError($"Failed to read tag for object: {gameObject.name} check PLC object settings or Tag Name");
        }
    }
}

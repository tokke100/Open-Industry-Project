using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport; 
using System;
using UnityEditor;
using System.Threading.Tasks;

public class Conveyor : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public int speed = 0;
    Vector3 startPos = new();
    Rigidbody rb;
    int scantime = 0;
    new readonly Tag<DintPlcMapper, int> tag = new();
    int failCount = 0;

    void Start()
    {
        if (enablePLC)
        {
            try { plctag.ForceExtractLibrary = false; } catch { };

            var _plc = GameObject.Find("PLC").GetComponent<PLC>();

            tag.Name = tagName;
            tag.Gateway = _plc.Gateway;
            tag.Path = _plc.Path;
            tag.PlcType = _plc.PlcType;
            tag.Protocol = _plc.Protocol;
            tag.Timeout = TimeSpan.FromSeconds(5);

            scantime = _plc.ScanTime;

            InvokeRepeating(nameof(ScanTag), 0, (float)scantime / 1000f);
        }

        rb = GetComponent<Rigidbody>();

        startPos = rb.GetComponent<Transform>().transform.position;
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
        catch (Exception e)
        {
            if(failCount > 0)
            {
                CancelInvoke(nameof(ScanTag));
                Debug.LogError($"Failed to read tag for object: {gameObject.name} check PLC object settings or Tag Name");
            }
            failCount++;
        }
    }
}

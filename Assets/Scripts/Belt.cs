using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport; 
using System;
using UnityEditor;
using System.Threading.Tasks;

public class Belt : MonoBehaviour
{
    private Vector3 startPos = new();
    private Rigidbody rb;
    public int speed = 0;
    public new string name;

    int scantime = 0;

    new readonly Tag<DintPlcMapper, int> tag = new();

    void Start()
    {
        plctag.ForceExtractLibrary = false;

        var _plc = GameObject.Find("PLC").GetComponent<PLC>();

        tag.Name = name;
        tag.Gateway = _plc.Gateway;
        tag.Path= _plc.Path;
        tag.PlcType= _plc.PlcType;
        tag.Protocol= _plc.Protocol;

        rb = GetComponentInChildren<Rigidbody>();

        startPos = rb.GetComponent<Transform>().transform.position;

        scantime = _plc.ScanTime;

        InvokeRepeating(nameof(ScanTag), 0, (float)scantime/1000f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = rb.GetComponent<Transform>().transform.TransformDirection(Vector3.left) * speed;
        rb.GetComponent<Transform>().transform.position = startPos;

    }

    async Task ScanTag()
    {
        speed = await tag.ReadAsync();
    }
}

using libplctag.DataTypes;
using libplctag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libplctag.NativeImport;
using System.Threading.Tasks;

public class Photoeye : MonoBehaviour
{
    new readonly Tag<SintPlcMapper, sbyte> tag = new();

    public new string name;

    int scanTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        plctag.ForceExtractLibrary = false;

        var _plc = GameObject.Find("PLC").GetComponent<PLC>();

        tag.Name = name;
        tag.Gateway = _plc.Gateway;
        tag.Path = _plc.Path;
        tag.PlcType = _plc.PlcType;
        tag.Protocol = _plc.Protocol;

        scanTime= _plc.ScanTime;

        InvokeRepeating(nameof(ScanTag), 0, (float)scanTime/1000f);
    }

    // Update is called once per frame
    void Update()
    {
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out RaycastHit hit, 6))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            tag.Value = 1;
            //Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 6, Color.red);
            tag.Value = 0;
            //Debug.Log("Did not Hit");
        }
    }

    async Task ScanTag()
    {
        await tag.WriteAsync();
    }
}

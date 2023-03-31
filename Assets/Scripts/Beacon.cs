using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Beacon : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public bool lightBeacon = false;
    int scantime = 0;
    new readonly Tag<SintPlcMapper, sbyte> tag = new();
    Material material;
    void Start()
    {
        if (enablePLC)
        {
            plctag.ForceExtractLibrary = false;

            var _plc = GameObject.Find("PLC").GetComponent<PLC>();

            tag.Name = tagName;
            tag.Gateway = _plc.Gateway;
            tag.Path = _plc.Path;
            tag.PlcType = _plc.PlcType;
            tag.Protocol = _plc.Protocol;
            tag.Timeout = TimeSpan.FromSeconds(1);

            scantime = _plc.ScanTime;

            InvokeRepeating(nameof(ScanTag), 0, (float)scantime / 1000f);
        }

        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (lightBeacon)
        {
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }
    }

    async Task ScanTag()
    {
        lightBeacon = Convert.ToBoolean(await tag.ReadAsync());
    }
}

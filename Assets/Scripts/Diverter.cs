using libplctag.DataTypes;
using libplctag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libplctag.NativeImport;
using System.Threading.Tasks;

public class Diverter : MonoBehaviour
{


    Rigidbody rb;

    float time = 0;

    public string tagName;

    public float divertTime = 0;
    public float divertSpeed = 0;

    new readonly Tag<SintPlcMapper, sbyte> tag = new();

    Vector3 startPos = new();

    sbyte FireDivert = 0;

    bool divert = false;
    bool cycled = false;

    int scanTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        plctag.ForceExtractLibrary = false;

        var _plc = GameObject.Find("PLC").GetComponent<PLC>();

        tag.Name = tagName;
        tag.Gateway = _plc.Gateway;
        tag.Path = _plc.Path;
        tag.PlcType = _plc.PlcType;
        tag.Protocol = _plc.Protocol;

        scanTime = _plc.ScanTime;

        rb = GetComponent<Rigidbody>();

        startPos = transform.position;

        InvokeRepeating(nameof(ScanTag), 0, (float)scanTime/1000f);
    }

    // Update is called once per frame
    void Update()
    {
        if(FireDivert == 1 && !cycled)
        {
            divert= true;
        }
        else if (FireDivert == 0)
        {
            cycled = false;
        }

        if (divert && !cycled)
        {
            time += Time.deltaTime;

            if (time < divertTime)
            {
                rb.velocity = Vector3.forward * divertSpeed;
            }
            else
            {

                rb.velocity = Vector3.back * divertSpeed;

                if (time > divertTime * 2)
                {
                    rb.velocity = Vector3.zero;
                    transform.position = startPos;
                    time = 0;
                    divert = false;
                    cycled = true;
                }
            }
        }

    }

    async Task ScanTag()
    {
        FireDivert = await tag.ReadAsync();
    }

}

using libplctag.DataTypes;
using libplctag;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libplctag.NativeImport;
using System.Threading.Tasks;
using System;

public class Diverter : MonoBehaviour
{
    public bool enablePLC = true;

    Rigidbody rb;

    float time = 0;

    public string tagName;

    public bool fireDivert = false;
    public float divertTime = 0;
    public float divertSpeed = 0;

    new readonly Tag<SintPlcMapper, sbyte> tag = new();

    Vector3 startPos = new();

    bool divert = false;
    bool cycled = false;

    int scanTime = 0;

    // Start is called before the first frame update
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

            scanTime = _plc.ScanTime;

            InvokeRepeating(nameof(ScanTag), 0, (float)scanTime / 1000f);
        }
        
        //Set new rigidbody
        rb = GetComponent<DiverterAnimator>().GetDiverterRigidbody();

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!divert && startPos != transform.position)
        {
            startPos = transform.position;
        }

        if(fireDivert && !cycled)
        {
            //This is manual testing
            fireDivert = false;
            divert= true;
        }
        else if (fireDivert == false)
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
        fireDivert = Convert.ToBoolean(await tag.ReadAsync());
    }

}

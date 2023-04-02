using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System;
using UnityEditor;
using System.Threading.Tasks;

public class RetExtConveyor : MonoBehaviour
{

    public bool Extend;
    public bool retract;
    public float ExtendSize;
    public float retractSize;

    public bool enablePLC = false;
    public string tagName;
    public int speed;
    int scantime = 0;
    new readonly Tag<DintPlcMapper, int> tag = new();
    int failCount = 0;

    float moveTime = 0.0f;


    private void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Extend)
        {
            moveTime += 1f * Time.deltaTime;
            if (moveTime >= 1 || transform.localScale.x >= ExtendSize)
            {
                transform.localScale = new Vector3(ExtendSize, 1f, 1f);
                Extend = false;
                moveTime = 0;
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Lerp(retractSize, ExtendSize, moveTime), 1f, 1f);
            }
            
            
        }

        if (retract)
        {
            moveTime += 1f * Time.deltaTime;
            if (moveTime >= 1 || transform.localScale.x <= retractSize)
            {
                transform.localScale = new Vector3(retractSize, 1f, 1f);
                retract = false;
                moveTime = 0;
            }
            else
            {
                transform.localScale = new Vector3(Mathf.Lerp(ExtendSize, retractSize, moveTime), 1f, 1f);
            }
        }
    }

    async Task ScanTag()
    {
        try
        {
            speed = await tag.ReadAsync();
        }
        catch (Exception)
        {
            if (failCount > 0)
            {
                CancelInvoke(nameof(ScanTag));
                Debug.LogError($"Failed to read tag for object: {gameObject.name} check PLC object settings or Tag Name");
            }
            failCount++;
        }
    }
}

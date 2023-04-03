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

    public SiemensPLC plc;

    void Start()
    {
        if (plc == null)
        {
            plc = GameObject.Find("SiemensPLC").GetComponent<SiemensPLC>();
        }
        
    }
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out RaycastHit hit, distance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            tag.Value = 1;
            plc.output[0] = true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * distance, Color.red);
            tag.Value = 0;
            plc.output[0] = false;
        }
    }
}

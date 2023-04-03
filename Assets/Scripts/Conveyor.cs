using UnityEngine;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport; 
using System;
using UnityEditor;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

public class Conveyor : MonoBehaviour
{
    public bool enablePLC = false;
    public string tagName;
    public int speed = 0;
    Vector3 startPos = new();
    Rigidbody rb;
    new readonly Tag<DintPlcMapper, int> tag = new();
    public bool conveyorRunning = false;
    public SiemensPLC plc;

    public string speedVariable;
    public string enableVariable;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        if (plc == null)
        {
            plc = GameObject.Find("SiemensPLC").GetComponent<SiemensPLC>();
        }
    }
    void Update()
    {
        conveyorRunning = plc.boolInputDict[enableVariable];


        if (conveyorRunning)
        {
            
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            rb.velocity = transform.TransformDirection(Vector3.left) * plc.floatInputDict[speedVariable];
            transform.position = startPos;
        }
        else
        {
            if(rb.velocity != Vector3.zero && rb.velocity.y == 0)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                transform.position = startPos;
                rb.velocity = Vector3.zero;
            }
            //when you stop, keep updating startPos for elevator
            startPos = transform.position;
        }
    }
}

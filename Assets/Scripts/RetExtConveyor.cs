using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetExtConveyor : MonoBehaviour
{

    public bool Extend;
    public bool retract;
    public float ExtendSize;
    public float retractSize;

    // Update is called once per frame
    void Update()
    {
        if (Extend)
        {
            Extend = false;
            transform.localScale = new Vector3(3.5f, 1f, 1f);
        }

        if (retract)
        {
            retract = false;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}

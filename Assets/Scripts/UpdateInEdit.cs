using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class UpdateInEdit : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var goL = GameObject.FindGameObjectsWithTag("RetConv");

        foreach(var g in goL)
        {
            g.GetComponent<ConvTextureMover>().RemakeMesh();
        }

    }
}

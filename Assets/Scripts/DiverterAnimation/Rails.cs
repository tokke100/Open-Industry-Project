using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rails : MonoBehaviour
{
    [SerializeField] private Transform childTransform;
    public float lengthMod = 0.5f;
   
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, childTransform.localPosition.z * lengthMod);
    }
}

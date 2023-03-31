using System.ComponentModel.Design;
using UnityEngine;

public class DiverterAnimator : MonoBehaviour
{
    private Diverter _diverter;
    public Rigidbody endPartRigidbody;

    private Vector3 startPosition;
    private float endPartMinimumZPosition = 0f;
    private float endPartMaximumZPosition = -5.9f;

    private MeshRenderer _meshRenderer;
    private int greenLampMaterialIndex = 4;
    private int redLampMaterialIndex = 3;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        startPosition = endPartRigidbody.transform.localPosition;
        _diverter = GetComponent<Diverter>();

        _meshRenderer.materials[greenLampMaterialIndex].DisableKeyword("_EMISSION");
        _meshRenderer.materials[redLampMaterialIndex].DisableKeyword("_EMISSION");
    }

    void Update()
    {
        if (endPartRigidbody.transform.localPosition.z >= endPartMinimumZPosition) 
            endPartRigidbody.transform.localPosition = new Vector3(startPosition.x,startPosition.y,endPartMinimumZPosition + 0.005f);
        else if(endPartRigidbody.transform.localPosition.z < endPartMaximumZPosition)
            endPartRigidbody.transform.localPosition = new Vector3(startPosition.x,startPosition.y,endPartMaximumZPosition - 0.005f);

        if (endPartRigidbody.velocity.z > 0.1f)
        {
            DiverterMove(true);
        }
        else if (endPartRigidbody.velocity.z < -0.1f)
        {
            DiverterMove(false);
        }
        else
        {
            SetLampLight(greenLampMaterialIndex, false);
            SetLampLight(redLampMaterialIndex, false);
        }
    }

    public void DiverterMove(bool forward)
    {
        if (forward)
        {
            SetLampLight(greenLampMaterialIndex, true);
            SetLampLight(redLampMaterialIndex, false);
        }
        else
        {
            SetLampLight(greenLampMaterialIndex, false);
            SetLampLight(redLampMaterialIndex, true);
        }
    }

    void SetLampLight(int materialIndex, bool enable)
    {
        if(enable) _meshRenderer.materials[materialIndex].EnableKeyword("_EMISSION");
        else _meshRenderer.materials[materialIndex].DisableKeyword("_EMISSION");
    }

    public Rigidbody GetDiverterRigidbody()
    {
        return endPartRigidbody;
    }
}

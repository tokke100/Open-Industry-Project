using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject myPrefab;

    public float time = 0;

    float period = 0;

    void Update()
    {
        if (period > time)
        {
            Spawn();
            period = 0;
        }
        period += Time.deltaTime;
    }

    void Spawn()
    {
        Instantiate(myPrefab,transform.position, myPrefab.transform.rotation);
    }
}

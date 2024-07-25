using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiater : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefabToInstantiate, transform.position, Quaternion.identity);
    }

    
}

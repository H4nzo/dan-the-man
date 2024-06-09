using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSpeed;

    void Start()
    {

    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
    }
}

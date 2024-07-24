using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;
    public UnityEvent OnTriggerStay;
    public UnityEvent OnTriggerExit;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnTrigger?.Invoke();
        }
    }
}

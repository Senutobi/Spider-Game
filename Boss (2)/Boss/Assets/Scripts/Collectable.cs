using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action OnCollected;
    public static int total;

    private void Awake() => total++;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(0, Time.time * 100f , 0); // rotation
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}

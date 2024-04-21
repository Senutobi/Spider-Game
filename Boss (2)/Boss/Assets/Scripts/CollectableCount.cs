using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableCount : MonoBehaviour
{
    private TMPro.TMP_Text text;
    private int count;
    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    private void Start() => UpdateCount();

    private void OnEnable() => Collectable.OnCollected += OnCollectibleCollected;
    private void OnDisable() => Collectable.OnCollected -= OnCollectibleCollected;
    
    void OnCollectibleCollected()
    {
        ++count;
        UpdateCount();

    }

    void UpdateCount()
    {
        text.text = $"Babies: {count} / {Collectable.total} Left";
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(!GameValues.IsGameStart) return;
        var mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.LogWarning("ok1");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning("ok2");
    }
}
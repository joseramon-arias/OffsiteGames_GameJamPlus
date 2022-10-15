using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraToCanvas : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = Camera.main;
    }
}

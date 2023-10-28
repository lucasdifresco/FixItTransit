using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntero : MonoBehaviour
{
    private RectTransform _puntero;

    private void Awake()
    {
        _puntero = gameObject.GetComponent<RectTransform>();
    }
    private void Update()
    {
        _puntero.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

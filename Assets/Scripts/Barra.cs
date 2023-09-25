using UnityEngine;
using UnityEngine.UI;

public class Barra : MonoBehaviour
{
    [SerializeField] private RectTransform _barra;

    public void SetProgreso(float progreso) 
    {
        _barra.localScale = new Vector3(progreso, _barra.localScale.y, _barra.localScale.z);
    }
}

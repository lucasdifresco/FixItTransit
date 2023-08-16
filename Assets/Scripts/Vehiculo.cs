using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehiculo : MonoBehaviour
{
    public float Velocidad;
    public Transform EsquinaDerecha;
    public Transform EsquinaIzquierda;

    public List<Bloque> _ruta;
    private bool _enMovimiento = false;
    private int _indice = 0;
    private Vector3 _origen;
    private Vector3 _destino;

    private float _temporizador;
    private float DeltaTime { get { return Time.deltaTime * Velocidad; } }

    public bool Transitar;
    private void OnValidate()
    {
        if (Transitar) { Transitar = false; TransitarRuta(); }
    }

    private void Awake()
    {
        //_ruta = new List<Bloque>();
    }
    private void Start()
    {
        TransitarRuta();
    }
    private void Update()
    {
        if (!_enMovimiento) { return; }
        _temporizador += DeltaTime;

        if (_temporizador >= 1) 
        {
            _temporizador = 0;
            transform.position = _destino;
            _indice++;

            if (_indice >= _ruta.Count) { _enMovimiento = false; return; }

            _origen = transform.position;
            _destino = _ruta[_indice].transform.position;
            SetAngle();

            return;
        }

        Avanzar(_origen, _destino, _temporizador);
    }

    public void TransitarRuta(List<Bloque> ruta) 
    {
        if (_ruta == null || _ruta.Count <= 2 || _enMovimiento) { return; }
        _ruta = ruta;
        _indice = 1;
        _origen = _ruta[0].transform.position;
        _destino = _ruta[1].transform.position;
        SetAngle();
        _enMovimiento = true;
    }
    public void TransitarRuta() { TransitarRuta(_ruta); }

    private void Avanzar(Vector3 origen, Vector3 destino, float progreso) 
    {
        transform.position = Vector3.Lerp(origen, destino, progreso);
    }
    private void Girar(Vector3 punto) 
    {
        transform.RotateAround(punto, Vector3.forward, 1);
    }

    private void SetAngle() 
    {
        Vector3 direction = _destino - _origen;
        if (direction.x > 0) { transform.localEulerAngles = new Vector3(0, 0, 0); }
        if (direction.y > 0) { transform.localEulerAngles = new Vector3(0, 0, 90); }
        if (direction.x < 0) { transform.localEulerAngles = new Vector3(0, 0, 180); }
        if (direction.y < 0) { transform.localEulerAngles = new Vector3(0, 0, 270); }
    }
}

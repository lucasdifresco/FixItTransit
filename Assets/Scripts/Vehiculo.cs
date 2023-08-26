using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehiculo : MonoBehaviour
{
    public float Velocidad;
    public Action<bool> OnRecorridoTerminado;
    public TextPopUpManager GananciaPopUp;
    public TextPopUpManager MultaPopUp;


    public List<Bloque> _ruta;
    private bool _enMovimiento = false;
    private int _indice = 0;
    private Vector3 _origen;
    private Vector3 _destino;

    private float _temporizador;
    private float DeltaTime { get { return Time.deltaTime * Velocidad; } }
    public Bloque BloqueActual { get { return _ruta[_indice]; } }

    public bool Transitar;
    private void OnValidate()
    {
        if (Transitar) { Transitar = false; TransitarRuta(); }
    }

    private void Update()
    {
        if (!_enMovimiento) { return; }
        _temporizador += DeltaTime;

        if (_temporizador >= 1) 
        {
            _temporizador = 0;
            transform.position = _destino;

            if (!SePuedeTransitar(_ruta[_indice])) { NoPuedeTransitar(); return; }


            if (_indice >= _ruta.Count - 1) 
            {
                if (!SePuedeEstacionar(_ruta[_indice])) { NoPuedeEstacionar(); }
                else { LlegoADestino(); }               
                return; 
            }

            _indice++;
            _origen = transform.position;
            _destino = _ruta[_indice].transform.position;
            SetAngle();

            return;
        }

        Avanzar(_origen, _destino, _temporizador);
    }

    public void Preparar()
    {
        _indice = 1;
        _origen = _ruta[0].transform.position;
        _destino = _ruta[1].transform.position;
        Avanzar(_origen, _destino, 0);
        SetAngle();
    }
    public void TransitarRuta(List<Bloque> ruta) 
    {
        if (_ruta == null || _ruta.Count <= 2 || _enMovimiento) { return; }
        _ruta = ruta;
        Preparar();
        _enMovimiento = true;
    }
    public void TransitarRuta() { TransitarRuta(_ruta); }


    private bool SePuedeTransitar(Bloque bloque) 
    {
        if (bloque.Obstaculo == null) { return true; }
        if (!bloque.Obstaculo.EsPeaton) { return true; }
        if (bloque.Herramienta != null && bloque.Herramienta.EsSenda) { return true; }

        return false; 
    }
    private bool SePuedeEstacionar(Bloque bloque) 
    {
        if (bloque.Obstaculo == null) { return true; }
        if (!bloque.Obstaculo.EsEscuela) { return true; }
        if (bloque.Herramienta != null && bloque.Herramienta.EsProhibidoEstacionar) { return true; }

        return false; 
    }

    private void NoPuedeTransitar() 
    {
        TerminarRecorrido(false);
    }
    private void NoPuedeEstacionar() 
    {
        TerminarRecorrido(false);
    }
    private void LlegoADestino() 
    {
        TerminarRecorrido(true);
    }

    private void TerminarRecorrido(bool llegoADestino) 
    {
        _enMovimiento = false;
        OnRecorridoTerminado?.Invoke(llegoADestino);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    private void Avanzar(Vector3 origen, Vector3 destino, float progreso) 
    {
        transform.position = Vector3.Lerp(origen, destino, progreso);
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

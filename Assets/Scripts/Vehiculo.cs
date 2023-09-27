using System;
using System.Collections.Generic;
using UnityEngine;

public class Vehiculo : MonoBehaviour
{
    [SerializeField] private float VelocidadBaja = 2;
    [SerializeField] private float VelocidadAlta = 4;
    [SerializeField] private float VelocidadMax = 4;
    [SerializeField] private GameObject ImagenVelocidadBaja;
    [SerializeField] private GameObject ImagenVelocidadAlta;

    private float _velocidad;
    private Action<Vehiculo, bool> OnRecorridoTerminado;
    private List<Bloque> _ruta;
    private bool _enMovimiento = false;
    private int _indice = 0;
    private Vector3 _origen;
    private Vector3 _destino;
    private float _temporizador;
    private bool _velocidadActualizada = false;
    private float DeltaTime { get { return Time.deltaTime * ((_velocidad == VelocidadMax)? VelocidadAlta : VelocidadBaja); } }
    
    private void Update()
    {
        if (!_enMovimiento) { return; }
        _temporizador += DeltaTime;

        if (_temporizador + 0.5f >= 1 && !_velocidadActualizada) 
        {
            _velocidadActualizada = true;

            if (_velocidad < VelocidadMax) { _velocidad++; }

            if (_ruta[_indice].Herramienta != null && _ruta[_indice].Herramienta.Tipo == Herramienta.HERRAMIENTA.Senda) { _velocidad -= 1; }
            if (_ruta[_indice].Herramienta != null && _ruta[_indice].Herramienta.Tipo == Herramienta.HERRAMIENTA.Loma) { _velocidad -= 3; }
            if (_ruta[_indice].Herramienta != null && _ruta[_indice].Herramienta.Tipo == Herramienta.HERRAMIENTA.Semaforo) { _velocidad -= 5; }

            if (_velocidad < VelocidadMax)
            {
                ImagenVelocidadBaja.SetActive(true);
                ImagenVelocidadAlta.SetActive(false);
            }
            else
            {
                ImagenVelocidadBaja.SetActive(false);
                ImagenVelocidadAlta.SetActive(true);
            }
        }

        if (_temporizador >= 1)
        {
            _temporizador = 0;
            _velocidadActualizada = false;

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

    public int LongitudRuta { get { return _ruta.Count; } }
    public Bloque BloqueActual { get { return _ruta[_indice]; } }
    public void TransitarRuta(List<Bloque> ruta, Action<Vehiculo, bool> onRecorridoTerminado = null) 
    {
        _ruta = ruta;
        if (_ruta == null || _ruta.Count <= 2 || _enMovimiento) { return; }
        OnRecorridoTerminado = onRecorridoTerminado;
        Preparar();
        _enMovimiento = true;
    }

    private void Preparar()
    {
        _indice = 1;
        _velocidad = 1;
        _origen = _ruta[0].transform.position;
        _destino = _ruta[1].transform.position;
        Avanzar(_origen, _destino, 0);
        SetAngle();
    }
    private bool SePuedeTransitar(Bloque bloque) 
    {
        if (bloque.Obstaculo == null) { return true; }
        if (bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Peaton) { return true; }
        if (bloque.Obstaculo.Tipo == Obstaculo.OBSTACULO.Peaton && _velocidad < VelocidadMax) { return true; }

        return false; 
    }
    private bool SePuedeEstacionar(Bloque bloque) 
    {
        if (bloque.Obstaculo == null) { return true; }
        if (bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Escuela) { return true; }
        if (bloque.Herramienta != null && bloque.Herramienta.Tipo == Herramienta.HERRAMIENTA.Estacionar) { return true; }

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
        OnRecorridoTerminado?.Invoke(this, llegoADestino);
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

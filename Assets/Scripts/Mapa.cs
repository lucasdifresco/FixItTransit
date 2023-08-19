using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    public Seccion prefab;

    private Seccion _centro;
    private Seccion _este;
    private Seccion _sur;
    private Seccion _sureste;

    public bool CrearMapa = false;
    private void OnValidate()
    {
        if (CrearMapa) { CrearMapa = false; CrearCentro(); }
    }

    private void Start()
    {
        CrearCentro();
    }

    private void CrearCentro() 
    {
        if (_centro != null) { Destroy(_centro.gameObject); }
        if (_este != null) { Destroy(_este.gameObject); }
        if (_sur != null) { Destroy(_sur.gameObject); }
        if (_sureste != null) { Destroy(_sureste.gameObject); }
        
        _centro = Instantiate(prefab, transform.position, transform.rotation, transform);
        _este = Instantiate(prefab, transform.position + new Vector3(4, 0, 0), transform.rotation, transform);
        _sur = Instantiate(prefab, transform.position + new Vector3(0, -4, 0), transform.rotation, transform);
        _sureste = Instantiate(prefab, transform.position + new Vector3(4, -4, 0), transform.rotation, transform);

        _centro.Este = _este;
        _centro.Sur = _sur;
        _este.Oeste = _centro;
        _este.Sur = _sureste;
        _sur.Norte = _centro;
        _sur.Este = _sureste;
        _sureste.Norte = _este;
        _sureste.Oeste = _sur;

        _centro.Inicializar();
        _este.Inicializar();
        _sur.Inicializar();
        _sureste.Inicializar();

        _centro.ActualizarConexiones();
        _este.ActualizarConexiones();
        _sur.ActualizarConexiones();
        _sureste.ActualizarConexiones();

        _centro.SetSeccion();
        _este.SetOeste();
        _sur.SetNorte();
        _sureste.SetNorteOeste();

        _centro.ActualizarImagenes();
        _este.ActualizarImagenes();
        _sur.ActualizarImagenes();
        _sureste.ActualizarImagenes();
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Partida : MonoBehaviour
{
    public Mapa Mapa;

    public Herramienta Senda;
    public Herramienta Estacionamiento;

    public int SeguridadMaxNivel0 = 3;
    public int SeguridadMaxNivel1 = 5;
    public int SeguridadMaxNivel2 = 8;
    public int MaxCantidadDeIncidentes = 8;

    public UnityEvent OnMapaCompletado;
    public UnityEvent OnPartidaTerminada;

    public TMP_Text Texto_Dias;
    public TMP_Text Texto_Puntos;
    public TMP_Text Texto_Incidentes;
    public TMP_Text Texto_Seguridad;

    public TextPopUpManager CostoPopUp;

    private Herramienta _herramienta;
    private int _dia;
    private bool Pausa = true;
    private bool Transitando;
    private int _puntos = 100;
    private int _incidentes;
    private int _seguridad;


    private int SeguridadNivel { get 
        {
            if (Mapa.Nivel == 0) { return SeguridadMaxNivel0; }
            else if (Mapa.Nivel == 1) { return SeguridadMaxNivel1; }
            else { return SeguridadMaxNivel2; }
        }
    }
    private void Update()
    {
        if (Transitando) { return; }
        if (Pausa) { return; }

        if (Input.GetMouseButtonDown(0)) { CrearHerramienta(); }
    }

    public void IniciarPartida()
    {
        Pausa = false;
        Transitando = false;
        _dia = 0;
        _puntos = 100;
        _incidentes = 0;
        _seguridad = 0;
        Mapa.CrearMapa();
        Mapa.ReiniciarNivel();
        ActualizarTextos();
    }
    public void CrearNuevoMapa()
    {
        Pausa = false;
        Transitando = false;
        _incidentes = 0;
        _seguridad = 0;
        Mapa.CrearMapa();
        ActualizarTextos();
    }
    public void NuevoDía()
    {
        if (Transitando) { return; }
        if (Pausa) { return; }

        _dia++;
        Vehiculo vehiculo = Mapa.CrearCamino();
        ActualizarTextos();
        vehiculo.TransitarRuta();

        vehiculo.GananciaPopUp.SetParent(transform);
        vehiculo.MultaPopUp.SetParent(transform);

        vehiculo.OnRecorridoTerminado += (llegoADestino) =>
        {
            Transitando = false;
            if (llegoADestino)
            {
                _puntos += vehiculo._ruta.Count * 10;
                vehiculo.GananciaPopUp.GetInstance().GetComponent<TextPopUpController>().SetText($"${vehiculo._ruta.Count * 10}");
                _seguridad++;
                
                if (_seguridad >= SeguridadNivel) 
                {
                    if (Mapa.SiguienteNivel())
                    {
                        Pausa = true;
                        OnMapaCompletado?.Invoke();
                    }
                    _seguridad = 0;
                }
            }
            else
            {
                _seguridad = 0;
                _incidentes++;
                if (_incidentes >= MaxCantidadDeIncidentes)
                {
                    Pausa = true;
                    OnPartidaTerminada?.Invoke();
                }
                _puntos -= vehiculo.BloqueActual.Obstaculo.Multa;
                vehiculo.MultaPopUp.GetInstance().GetComponent<TextPopUpController>().SetText($"${vehiculo.BloqueActual.Obstaculo.Multa}");
                if (_puntos < 0) { _puntos = 0; }
            }
            ActualizarTextos();
        };

        Transitando = true;
    }
    public void ActualizarTextos()
    {
        Texto_Dias.text = $"DÍA {_dia}";
        Texto_Puntos.text = $"${_puntos}";
        Texto_Incidentes.text = $"{_incidentes}/{MaxCantidadDeIncidentes}";
        Texto_Seguridad.text = $"{_seguridad}/{SeguridadNivel}";
    }

    public void SetEstacionamiento() { if (Pausa) { return; } _herramienta = Estacionamiento; }
    public void SetSenda() { if (Pausa) { return; } _herramienta = Senda; }
    public void SetNulo() { if (Pausa) { return; } _herramienta = null; }

    private void CrearHerramienta()
    {
        if (_herramienta == null) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider == null) { return; }

        if (!hit.collider.TryGetComponent<Bloque>(out var bloque)) { return; }
        if (!bloque.Calle) { return; }
        if (_herramienta.Costo > _puntos) { print($"{_herramienta.name}: {_herramienta.Costo} --> Puntos Actuales: {_puntos}"); return; }
        _puntos -= _herramienta.Costo;
        bloque.GenerarHerramienta(_herramienta);
        bloque.ActualizarImagen();
        ActualizarTextos();

        TextPopUpController popup = CostoPopUp.GetInstance().GetComponent<TextPopUpController>();
        popup.SetText($"${_herramienta.Costo}");
        popup.gameObject.transform.position = bloque.transform.position;
    }
}

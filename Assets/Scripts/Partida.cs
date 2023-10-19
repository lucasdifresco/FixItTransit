using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Partida : MonoBehaviour
{
    [SerializeField] private Ciudad _ciudad;
    [SerializeField] private Vehiculo _vehiculo;
    [SerializeField] private Cuestionario _cuestionario;

    [SerializeField] private int _bonoPorCiudad = 300;
    [SerializeField] private int _puntosIniciales = 300;
    [SerializeField] private int _multiplicadorDePuntosPorLongitud = 15;
    [SerializeField] private int _puntosMinimos = 50;

    [SerializeField] private int _incidentesPorCuestionario = 1;
    [SerializeField] private int _bonoPorCuestionario = 100;

    [SerializeField] private int _maxCantidadDeSeguridad = 3;
    [SerializeField] private int _maxCantidadDeIncidentes = 3;
    [SerializeField] private int _frecuenciaCuestionario = 3;

    [SerializeField] private UnityEvent OnIniciarPartida;
    [SerializeField] private UnityEvent OnMapaCompletado;
    [SerializeField] private UnityEvent OnPartidaTerminada;

    [SerializeField] private UnityEvent OnNuevoDia;
    [SerializeField] private UnityEvent OnNuevaCiudad;
    [SerializeField] private UnityEvent OnViajeSeguro;
    [SerializeField] private UnityEvent OnIncidente;
    [SerializeField] private UnityEvent OnComprarHerramienta;
    [SerializeField] private UnityEvent OnRecompensa;

    [SerializeField] private TMP_Text Texto_Dias;
    [SerializeField] private TMP_Text Texto_Puntos;
    [SerializeField] private TMP_Text Texto_Incidentes;
    [SerializeField] private TMP_Text Texto_Seguridad;

    [SerializeField] private TMP_Text Texto_DiasTotales;
    [SerializeField] private TMP_Text Texto_IncidentesTotales;
    [SerializeField] private TMP_Text Texto_SeguridadTotales;
    [SerializeField] private TMP_Text Texto_RespuestasCorrectas;
    [SerializeField] private TMP_Text Texto_RespuestasIncorrectas;
    [SerializeField] private TMP_Text Texto_RespuestasTotales;
    [SerializeField] private TMP_Text Texto_Nota;

    [SerializeField] private Barra BarraSeguridad;
    [SerializeField] private Barra BarraIncidentes;

    [SerializeField] private GameObject BotonNuevoDia;
    [SerializeField] private GameObject BotonPregunta;
    [SerializeField] private Marcador MarcadorDeOrigen;

    [SerializeField] private TextPopUpManager CostoPopUp;
    [SerializeField] private TextPopUpManager BonoPopUp;
    [SerializeField] private TextPopUpManager IncidentePopUp;
    [SerializeField] private TextPopUpManager MensajePopUp;

    private Herramienta _herramienta;
    private int _dia;
    private bool _pausa = true;
    private bool _transitando;
    private int _puntos = 100;
    private int _incidentes;
    private int _seguridad;
    private int _diasDesdeUltimoCuestionario = 0;

    private int _incidentesTotales;
    private int _seguridadTotal;

    private Bloque _origen;
    private Vehiculo _vehiculoActual;

    private void Awake() { _cuestionario.Inicializar(); }
    private void Update()
    {
        if (_pausa) { return; }
        if (_transitando) { return; }

        if (Input.GetMouseButtonDown(0)) { CrearHerramienta(); }
    }

    public void IniciarPartida()
    {
        _pausa = false;
        _transitando = false;
        _dia = 0;
        _puntos = _puntosIniciales;
        _incidentes = 0;
        _seguridad = 0;
        _diasDesdeUltimoCuestionario = 0;
        DesactivarBotonCuestionario();

        _incidentesTotales = 0;
        _seguridadTotal = 0;
        _cuestionario.Reiniciar();
        _ciudad.ReiniciarNivel();

        _ciudad.CrearCiudad();
        ActualizarTextos();
        _origen = _ciudad.ObtenerCalleAleatoria();
        MarcadorDeOrigen.Posicionar(_origen);

        if (_vehiculoActual != null) { Destroy(_vehiculoActual); }

        OnIniciarPartida?.Invoke();
    }
    public void CrearNuevoMapa()
    {
        _pausa = false;
        _transitando = false;
        _seguridad = 0;
        _ciudad.CrearCiudad();
        ActualizarTextos();
        _origen = _ciudad.ObtenerCalleAleatoria();
        MarcadorDeOrigen.Posicionar(_origen);
        OnNuevaCiudad?.Invoke();
    }
    public void NuevoDía()
    {
        if (_pausa) { return; }
        if (_transitando) { return; }

        _dia++;
        
        if (_vehiculo == null) { print("No hay vehiculo"); return; }
        _vehiculoActual = Instantiate(_vehiculo, transform.position, transform.rotation, transform);
        _vehiculoActual.TransitarRuta(_ciudad.CrearRuta(_origen), VerificarRecorrido);
        OnNuevoDia?.Invoke();

        ActualizarTextos();
        _transitando = true;
    }
    public void SetHerramienta(Herramienta herramienta) { if (_pausa) { return; } _herramienta = herramienta; }
    public void SetNulo() { if (_pausa) { return; } _herramienta = null; }
    public void RespuestaCorrecta(Vector3 posicion)
    {
        int reduccion = ReducirIncidentes(_incidentesPorCuestionario);
        if (reduccion > 0) { IncidentePopUp.GetInstance(posicion, $"-{reduccion}"); }
        else 
        {
            _puntos += _bonoPorCuestionario;
            BonoPopUp.GetInstance(posicion, $"${_bonoPorCuestionario}"); 
        }
        ActualizarTextos();
    }

    public void PausarPartida() 
    {
        _pausa = true;
        if (_vehiculoActual != null) { _vehiculoActual.Pausar(); }
    }
    public void ReanudarPartida() 
    {
        _pausa = false;
        if (_vehiculoActual != null) { _vehiculoActual.Reanudar(); }
    }
    public void TerminarPartida() 
    {
        _pausa = true;
        if (_vehiculoActual != null) { Destroy(_vehiculoActual); }
        OnPartidaTerminada?.Invoke();
    }
    public void Salir() { Application.Quit(); }

    public void RecibirRecompensa(int puntosGanados) 
    {
        _puntos += puntosGanados;
        BonoPopUp.GetInstance($"${puntosGanados}");
        ActualizarTextos();
        OnRecompensa?.Invoke();
    }

    public void ActivarBotonCuestionario() { BotonNuevoDia.SetActive(false); BotonPregunta.SetActive(true); }
    public void DesactivarBotonCuestionario() { BotonNuevoDia.SetActive(true); BotonPregunta.SetActive(false); }

    private int ReducirIncidentes(int cantidad)
    {
        cantidad = Mathf.Abs(cantidad);
        if (_incidentes == 0) { cantidad = 0; }
        _incidentes -= cantidad;
        if (_incidentes < 0) { cantidad = _incidentes; _incidentes = 0; }
        return cantidad;
    }
    private void VerificarRecorrido(Vehiculo vehiculo, bool llegoADestino) 
    {
        _transitando = false;
        if (llegoADestino)
        {
            int puntosGanados = vehiculo.LongitudRuta * _multiplicadorDePuntosPorLongitud + _puntosMinimos;
            _puntos += puntosGanados;
            BonoPopUp.GetInstance(vehiculo.transform.position, $"${puntosGanados}");
            _seguridad++;
            _seguridadTotal++;

            _origen = vehiculo.BloqueActual;
            MarcadorDeOrigen.Posicionar(_origen);

            if (_seguridad >= _maxCantidadDeSeguridad)
            {
                _ciudad.SiguienteNivel();
                _pausa = true;

                if (_bonoPorCiudad != 0)
                {
                    _puntos += _bonoPorCiudad;
                    BonoPopUp.GetInstance($"${_bonoPorCiudad}");
                }

                OnMapaCompletado?.Invoke();
                _seguridad = 0;
            }
            OnViajeSeguro?.Invoke();
        }
        else
        {
            _incidentes++;
            _incidentesTotales++;
            IncidentePopUp.GetInstance(vehiculo.transform.position, "+1");

            OnIncidente?.Invoke();

            if (_incidentes >= _maxCantidadDeIncidentes) { TerminarPartida(); }
        }

        _diasDesdeUltimoCuestionario++;
        if (_diasDesdeUltimoCuestionario >= _frecuenciaCuestionario)
        {
            _diasDesdeUltimoCuestionario = 0;
            ActivarBotonCuestionario();
        }

        ActualizarTextos();
    }
    private void CrearHerramienta()
    {
        if (_herramienta == null) { return; }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider == null) { return; }

        if (!hit.collider.TryGetComponent<Bloque>(out var bloque)) { return; }
        if (!bloque.Calle) { return; }
        if (bloque.Herramienta != null && bloque.Herramienta.Tipo == _herramienta.Tipo) { return; }

        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Estacionar && (bloque.Obstaculo == null || bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Escuela)) 
        {
            MensajePopUp.GetInstance(bloque.transform.position, "Bloque Incompatible");
            print("Solo se pueden crear en las escuelas."); 
            return; 
        }
        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Loma && bloque.Obstaculo != null) 
        {
            MensajePopUp.GetInstance(bloque.transform.position, "Bloque Incompatible");
            print("La loma de burro solo se puede crear en las calles vacías."); 
            return; 
        }
        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Senda && (bloque.Obstaculo == null || bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Peaton)) 
        {
            MensajePopUp.GetInstance(bloque.transform.position, "Bloque Incompatible");
            print("Las Sendas Peatonales solo pueden ubicarse sobre peatones."); 
            return; 
        }
        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Semaforo && (bloque.Obstaculo == null || bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Trafico)) 
        {
            MensajePopUp.GetInstance(bloque.transform.position, "Bloque Incompatible");
            print("El semáforo solo se puede crear en las calles vacías o con peatones."); 
            return; 
        }

        if (_herramienta.Costo > _puntos) 
        {
            MensajePopUp.GetInstance(bloque.transform.position, "Fondos Insuficientes");
            print($"Fondos Insuficientes {_herramienta.name}: {_herramienta.Costo} --> Puntos Actuales: {_puntos}"); return; 
        }
        _puntos -= _herramienta.Costo;
        bloque.GenerarHerramienta(_herramienta);
        bloque.ActualizarImagen();
        ActualizarTextos();

        CostoPopUp.GetInstance(bloque.transform.position, $"${_herramienta.Costo}");
        OnComprarHerramienta?.Invoke();
    }
    private void ActualizarTextos()
    {
        Texto_Dias.text = $"DÍA {_dia}";
        Texto_Puntos.text = $"${_puntos}";
        Texto_Incidentes.text = $"{_incidentes}/{_maxCantidadDeIncidentes}";
        Texto_Seguridad.text = $"{_seguridad}/{_maxCantidadDeSeguridad}";

        Texto_DiasTotales.text = $"DÍAS: {_dia}";
        Texto_IncidentesTotales.text = $"INCIDENTES: {_incidentesTotales}";
        Texto_SeguridadTotales.text = $"VIAJES SEGUROS: {_seguridadTotal}";
        Texto_RespuestasCorrectas.text = $"CORRECTAS: {_cuestionario.RespuestasCorrectas}";
        Texto_RespuestasIncorrectas.text = $"INCORRECTAS: {_cuestionario.RespuestasIncorrectas}";
        Texto_RespuestasTotales.text = $"TOTALES: {_cuestionario.RespuestasTotales}";
        Texto_Nota.text = $"{((float.IsNaN(_cuestionario.Nota)) ? 0: _cuestionario.Nota):#0.#}";

        BarraSeguridad.SetProgreso((float)_seguridad / _maxCantidadDeSeguridad);
        BarraIncidentes.SetProgreso((float)_incidentes / _maxCantidadDeIncidentes);
    }
}

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
    [SerializeField] private int _seguridadMaxNivel0 = 3;
    [SerializeField] private int _seguridadMaxNivel1 = 5;
    [SerializeField] private int _seguridadMaxNivel2 = 5;
    [SerializeField] private int _maxCantidadDeIncidentes = 10;
    [SerializeField] private int _frecuenciaCuestionario = 3;

    [SerializeField] private UnityEvent OnMapaCompletado;
    [SerializeField] private UnityEvent OnPartidaTerminada;

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

    private Herramienta _herramienta;
    private int _dia;
    private bool Pausa = true;
    private bool Transitando;
    private int _puntos = 100;
    private int _incidentes;
    private int _seguridad;
    private int _diasDesdeUltimoCuestionario = 0;
    private int SeguridadNivel { get 
        {
            if (_ciudad.Nivel == 0) { return _seguridadMaxNivel0; }
            else if (_ciudad.Nivel == 1) { return _seguridadMaxNivel1; }
            else { return _seguridadMaxNivel2; }
        }
    }

    private int _incidentesTotales;
    private int _seguridadTotal;

    private Bloque _origen;

    private void Awake() { _cuestionario.Inicializar(); }
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
        _puntos = _puntosIniciales;
        _incidentes = 0;
        _seguridad = 0;

        _incidentesTotales = 0;
        _seguridadTotal = 0;
        _cuestionario.Reiniciar();

        _ciudad.CrearCiudad();
        _ciudad.ReiniciarNivel();
        ActualizarTextos();
        _origen = _ciudad.ObtenerCalleAleatoria();
        MarcadorDeOrigen.Posicionar(_origen);
    }
    public void CrearNuevoMapa()
    {
        Pausa = false;
        Transitando = false;
        _seguridad = 0;
        _ciudad.CrearCiudad();
        ActualizarTextos();
        _origen = _ciudad.ObtenerCalleAleatoria();
        MarcadorDeOrigen.Posicionar(_origen);
    }
    public void NuevoDía()
    {
        if (Transitando) { return; }
        if (Pausa) { return; }

        _diasDesdeUltimoCuestionario++;
        if (_diasDesdeUltimoCuestionario >= _frecuenciaCuestionario) 
        {
            _diasDesdeUltimoCuestionario = 0;
            ActivarBotonCuestionario();
        }

        _dia++;
        
        if (_vehiculo == null) { print("No hay vehiculo"); return; }
        Vehiculo vehiculo = Instantiate(_vehiculo, transform.position, transform.rotation, transform);
        //vehiculo.TransitarRuta(_ciudad.CrearRuta(), VerificarRecorrido);
        vehiculo.TransitarRuta(_ciudad.CrearRuta(_origen), VerificarRecorrido);

        ActualizarTextos();
        Transitando = true;
    }
    public void SetHerramienta(Herramienta herramienta) { if (Pausa) { return; } _herramienta = herramienta; }
    public void SetNulo() { if (Pausa) { return; } _herramienta = null; }
    public void ReducirIncidentes(int cantidad) 
    {
        if (_incidentes - cantidad >= _maxCantidadDeIncidentes) { return; }
        _incidentes -= cantidad;
        if (_incidentes < 0) { _incidentes = 0; }
        ActualizarTextos();        
    }
    
    public void ActivarBotonCuestionario() { BotonNuevoDia.SetActive(false); BotonPregunta.SetActive(true); }
    public void DesactivarBotonCuestionario() { BotonNuevoDia.SetActive(true); BotonPregunta.SetActive(false); }

    private void VerificarRecorrido(Vehiculo vehiculo, bool llegoADestino) 
    {
        Transitando = false;
        if (llegoADestino)
        {
            _puntos += vehiculo.LongitudRuta * 10;
            BonoPopUp.GetInstance(vehiculo.transform.position).GetComponent<TextPopUpController>().SetText($"${vehiculo.LongitudRuta * 10}");
            _seguridad++;
            _seguridadTotal++;

            _origen = vehiculo.BloqueActual;
            MarcadorDeOrigen.Posicionar(_origen);

            if (_seguridad >= SeguridadNivel)
            {
                if (_ciudad.SiguienteNivel())
                {
                    Pausa = true;

                    if (_ciudad.Nivel >= 2 || _ciudad.Nivel == -1)
                    {
                        _puntos += _bonoPorCiudad;
                        BonoPopUp.GetInstance().GetComponent<TextPopUpController>().SetText($"${_bonoPorCiudad}");
                    }

                    OnMapaCompletado?.Invoke();
                }
                _seguridad = 0;
            }
        }
        else
        {
            _seguridad = 0;
            _incidentes++;
            _incidentesTotales++;

            if (_incidentes >= _maxCantidadDeIncidentes)
            {
                Pausa = true;
                OnPartidaTerminada?.Invoke();
            }
            _puntos -= vehiculo.BloqueActual.Obstaculo.Multa;
            CostoPopUp.GetInstance(vehiculo.transform.position).GetComponent<TextPopUpController>().SetText($"${vehiculo.BloqueActual.Obstaculo.Multa}");
            if (_puntos < 0) { _puntos = 0; }
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

        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Senda && (bloque.Obstaculo == null || bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Peaton)) { print("Las Sendas Peatonales solo pueden ubicarse sobre peatones."); return; }
        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Estacionar && (bloque.Obstaculo == null || bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Escuela)) { print("Solo se pueden crear en las escuelas."); return; }
        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Loma && bloque.Obstaculo != null) { print("La loma de burro solo se puede crear en las calles vacías."); return; }
        if (_herramienta.Tipo == Herramienta.HERRAMIENTA.Semaforo && bloque.Obstaculo != null && bloque.Obstaculo.Tipo != Obstaculo.OBSTACULO.Peaton) { print("El semáforo solo se puede crear en las calles vacías o con peatones."); return; }

        if (_herramienta.Costo > _puntos) { print($"Fondos Insuficientes {_herramienta.name}: {_herramienta.Costo} --> Puntos Actuales: {_puntos}"); return; }
        _puntos -= _herramienta.Costo;
        bloque.GenerarHerramienta(_herramienta);
        bloque.ActualizarImagen();
        ActualizarTextos();

        TextPopUpController popup = CostoPopUp.GetInstance().GetComponent<TextPopUpController>();
        popup.SetText($"${_herramienta.Costo}");
        popup.gameObject.transform.position = bloque.transform.position;
    }
    private void ActualizarTextos()
    {
        Texto_Dias.text = $"DÍA {_dia}";
        Texto_Puntos.text = $"${_puntos}";
        Texto_Incidentes.text = $"{_incidentes}/{_maxCantidadDeIncidentes}";
        Texto_Seguridad.text = $"{_seguridad}/{SeguridadNivel}";

        Texto_DiasTotales.text = $"DÍAS: {_dia}";
        Texto_IncidentesTotales.text = $"INCIDENTES: {_incidentesTotales}";
        Texto_SeguridadTotales.text = $"VIAJES SEGUROS: {_seguridadTotal}";
        Texto_RespuestasCorrectas.text = $"CORRECTAS: {_cuestionario.RespuestasCorrectas}";
        Texto_RespuestasIncorrectas.text = $"INCORRECTAS: {_cuestionario.RespuestasIncorrectas}";
        Texto_RespuestasTotales.text = $"TOTALES: {_cuestionario.RespuestasTotales}";
        Texto_Nota.text = $"{((float.IsNaN(_cuestionario.Nota)) ? 0: _cuestionario.Nota):#0.#}";

        BarraSeguridad.SetProgreso((float)_seguridad / SeguridadNivel);
        BarraIncidentes.SetProgreso((float)_incidentes / _maxCantidadDeIncidentes);
    }
}

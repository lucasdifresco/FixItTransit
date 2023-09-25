using System.Collections.Generic;
using UnityEngine;

public class Ciudad : MonoBehaviour
{
    [SerializeField] private Seccion _seccion;
    [SerializeField] private Obstaculo Peaton;
    [SerializeField] private Obstaculo Escuela;

    [SerializeField] private int _maxIteraciones = 15;
    [SerializeField] private int _minDistanciaDeRuta = 3;
    [SerializeField] private bool _pintarExtremos = false;
    [SerializeField] private bool _pintarCamino = false;

    private Seccion[] _secciones;
    private Seccion _centro;
    private Seccion _norte;
    private Seccion _sur;
    private Seccion _oeste;
    private Seccion _este;
    private Seccion _norteOeste;
    private Seccion _surOeste;
    private Seccion _norteEste;
    private Seccion _surEste;
    private int _ultimaSeccion = -1;    
    private bool TirarMoneda { get { return Random.Range(0f, 1f) >= 0.5f; } }

    public int Nivel { get; private set; } = -1;
    public void CrearCiudad()
    {
        Limpiar();
        Instanciar();
        Conectar();
        Inicializar();
        ActualizarConexiones();
        GenerarSecciones();
        GenerarObstaculos();
        ActualizarImagenes();
        ActualizarNivel();
    }
    public List<Bloque> CrearRuta()
    {
        List<Bloque> ruta;
        int distanciaMinima = _minDistanciaDeRuta;
        if (Nivel == 0) { distanciaMinima = 3; }

        int iteracion = 0;
        do
        {
            ruta = Nodo.CalcularRuta(_norteOeste.NO.NO, ObtenerCalleAleatoria(), ObtenerCalleAleatoria());
            iteracion++;
        }
        while ((ruta == null || ruta.Count <= distanciaMinima) && iteracion < _maxIteraciones);

        return ruta;
    }
    public void ReiniciarNivel()
    {
        Nivel = -1;
        ActualizarNivel();
    }
    public bool SiguienteNivel() 
    {
        return true;
        Nivel++;
        if (Nivel > 2) { return true; }
        ActualizarNivel();
        return false;
    }

    private void Limpiar() 
    {
        if (_centro != null) { Destroy(_centro.gameObject); }
        
        if (_norte != null) { Destroy(_norte.gameObject); }
        if (_sur != null) { Destroy(_sur.gameObject); }
        if (_oeste != null) { Destroy(_oeste.gameObject); }
        if (_este != null) { Destroy(_este.gameObject); }

        if (_norteOeste != null) { Destroy(_norteOeste.gameObject); }
        if (_surOeste != null) { Destroy(_surOeste.gameObject); }
        if (_norteEste != null) { Destroy(_norteEste.gameObject); }
        if (_surEste != null) { Destroy(_surEste.gameObject); }
    }
    private void Instanciar() 
    {
        _centro = Instantiate(_seccion, transform.position, transform.rotation, transform);

        _norte = Instantiate(_seccion, transform.position + new Vector3( 0,  4, 0), transform.rotation, transform);
        _sur   = Instantiate(_seccion, transform.position + new Vector3( 0, -4, 0), transform.rotation, transform);
        _oeste = Instantiate(_seccion, transform.position + new Vector3(-4,  0, 0), transform.rotation, transform);
        _este  = Instantiate(_seccion, transform.position + new Vector3( 4,  0, 0), transform.rotation, transform);

        _norteOeste = Instantiate(_seccion, transform.position + new Vector3(-4,  4, 0), transform.rotation, transform);
        _surOeste   = Instantiate(_seccion, transform.position + new Vector3(-4, -4, 0), transform.rotation, transform);
        _norteEste  = Instantiate(_seccion, transform.position + new Vector3( 4,  4, 0), transform.rotation, transform);
        _surEste    = Instantiate(_seccion, transform.position + new Vector3( 4, -4, 0), transform.rotation, transform);

        _secciones = new[] { _centro, _norte, _sur, _oeste, _este, _norteOeste, _norteEste, _surOeste, _surEste };
    }
    private void Conectar()
    {
        _centro.Norte = _norte;
        _centro.Sur = _sur;
        _centro.Oeste = _oeste;
        _centro.Este = _este;

        _norte.Sur = _centro;
        _norte.Oeste = _norteOeste;
        _norte.Este = _norteEste;

        _sur.Norte = _centro;
        _sur.Oeste = _surOeste;
        _sur.Este = _surEste;

        _oeste.Norte = _norteOeste;
        _oeste.Sur = _surOeste;
        _oeste.Este = _centro;

        _este.Norte = _norteEste;
        _este.Sur = _surEste;
        _este.Oeste = _centro;

        _norteOeste.Sur = _oeste;
        _norteOeste.Este = _norte;

        _surOeste.Norte = _oeste;
        _surOeste.Este = _sur;

        _norteEste.Sur = _este;
        _norteEste.Oeste = _norte;

        _surEste.Norte = _este;
        _surEste.Oeste = _sur;
    }
    private void Inicializar()
    {
        _centro.Inicializar();

        _norte.Inicializar();
        _sur.Inicializar();
        _oeste.Inicializar();
        _este.Inicializar();

        _norteOeste.Inicializar();
        _surOeste.Inicializar();
        _norteEste.Inicializar();
        _surEste.Inicializar();
    }
    private void ActualizarConexiones() 
    {
        _centro.ActualizarConexiones();

        _norte.ActualizarConexiones();
        _sur.ActualizarConexiones();
        _oeste.ActualizarConexiones();
        _este.ActualizarConexiones();

        _norteOeste.ActualizarConexiones();
        _surOeste.ActualizarConexiones();
        _norteEste.ActualizarConexiones();
        _surEste.ActualizarConexiones();
    }
    private void GenerarSecciones()
    {
        int iteracion;

        iteracion = 0; do { _centro.SetSeccion(); iteracion++; } while (_centro.TieneRotonda && iteracion < _maxIteraciones);
        
        iteracion = 0; do { _norte.SetSur(); iteracion++; } while (_norte.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _sur.SetNorte(); iteracion++; } while (_sur.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _oeste.SetEste(); iteracion++; } while (_oeste.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _este.SetOeste(); iteracion++; } while (_este.TieneRotonda && iteracion < _maxIteraciones);
        
        iteracion = 0; do { _norteOeste.SetSurEste(); iteracion++; } while (_norteOeste.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _surOeste.SetNorteEste(); iteracion++; } while (_surOeste.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _norteEste.SetSurOeste(); iteracion++; } while (_norteEste.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _surEste.SetNorteOeste(); iteracion++; } while (_surEste.TieneRotonda && iteracion < _maxIteraciones);
    }
    private void ActualizarImagenes()
    {
        _centro.ActualizarImagenes();

        _norte.ActualizarImagenes();
        _sur.ActualizarImagenes();
        _oeste.ActualizarImagenes();
        _este.ActualizarImagenes();

        _norteOeste.ActualizarImagenes();
        _surOeste.ActualizarImagenes();
        _norteEste.ActualizarImagenes();
        _surEste.ActualizarImagenes();
    }
    private void ActualizarNivel() 
    {
        if (Nivel == 0)
        {
            if (_centro != null) { _centro.Activar(); }

            if (_norte != null) { _norte.Desactivar(); }
            if (_sur != null) { _sur.Desactivar(); }
            if (_oeste != null) { _oeste.Desactivar(); }
            if (_este != null) { _este.Desactivar(); }

            if (_norteOeste != null) { _norteOeste.Desactivar(); }
            if (_surOeste != null) { _surOeste.Desactivar(); }
            if (_norteEste != null) { _norteEste.Desactivar(); }
            if (_surEste != null) { _surEste.Desactivar(); }
        }
        else if (Nivel == 1)
        {
            if (_centro != null) { _centro.Activar(); }

            if (_norte != null) { _norte.Activar(); }
            if (_sur != null) { _sur.Activar(); }
            if (_oeste != null) { _oeste.Activar(); }
            if (_este != null) { _este.Activar(); }

            if (_norteOeste != null) { _norteOeste.Desactivar(); }
            if (_surOeste != null) { _surOeste.Desactivar(); }
            if (_norteEste != null) { _norteEste.Desactivar(); }
            if (_surEste != null) { _surEste.Desactivar(); }
        }
        else if (Nivel == 2)
        {
            if (_centro != null) { _centro.Activar(); }

            if (_norte != null) { _norte.Activar(); }
            if (_sur != null) { _sur.Activar(); }
            if (_oeste != null) { _oeste.Activar(); }
            if (_este != null) { _este.Activar(); }

            if (_norteOeste != null) { _norteOeste.Activar(); }
            if (_surOeste != null) { _surOeste.Activar(); }
            if (_norteEste != null) { _norteEste.Activar(); }
            if (_surEste != null) { _surEste.Activar(); }
        }
        else if (Nivel == -1) 
        {
            if (_centro != null) { _centro.Activar(); }
            if (_sur != null) { _sur.Activar(); }
            if (_este != null) { _este.Activar(); }
            if (_surEste != null) { _surEste.Activar(); }

            if (_norte != null) { _norte.Desactivar(); }
            if (_oeste != null) { _oeste.Desactivar(); }
            if (_norteOeste != null) { _norteOeste.Desactivar(); }
            if (_surOeste != null) { _surOeste.Desactivar(); }
            if (_norteEste != null) { _norteEste.Desactivar(); }
        }

        ActualizarImagenes();
    }
    private void GenerarObstaculos()
    {
        int cuadrante1;
        int cuadrante2;
        Cuadrante[] cuadrantes;

        foreach (Seccion seccion in _secciones) 
        {
            cuadrantes = seccion.Cuadrantes;

            cuadrante1 = Random.Range(0, 4);
            cuadrante2 = RandomRange(0, 4, cuadrante1);

            if (cuadrante1 == cuadrante2) { print("Error"); }

            //if (TirarMoneda)
            if (true)
            {
                seccion.Cuadrantes[cuadrante1].GenerarObstaculo(Peaton);
                seccion.Cuadrantes[cuadrante2].GenerarObstaculo(Escuela);
            }
            else if (TirarMoneda) { seccion.Cuadrantes[cuadrante1].GenerarObstaculo(Peaton); }
            else { seccion.Cuadrantes[cuadrante2].GenerarObstaculo(Escuela); }
        }
    }
    private Bloque ObtenerCalleAleatoria() 
    {
        int seccion;
        int cuadrante;

        if (Nivel == -1) { seccion = RandomRange(0, 9, _ultimaSeccion, 1, 3, 5, 6, 7); }
        else if (Nivel == 0) { seccion = 0; }
        else if (Nivel == 1) { seccion = RandomRange(0, 5, _ultimaSeccion); }
        else { seccion = RandomRange(0, 9, _ultimaSeccion); }
        cuadrante = Random.Range(0, 4);

        _ultimaSeccion = seccion;
        List<Bloque> calles = _secciones[seccion].Cuadrantes[cuadrante].Calles;

        Bloque bloque = calles[Random.Range(0, calles.Count)];
        return bloque;
    }
    private int RandomRange(int min, int max, params int[] indicesParaIgnorar) 
    {
        if (min > max) { (min, max) = (max, min); }
        if (indicesParaIgnorar.Length == max - min) { return 0; }

        int indice = Random.Range(min, max - indicesParaIgnorar.Length);

        foreach (int indiceIgnorado in indicesParaIgnorar)
        {
            if (indiceIgnorado == -1) { continue; }
            if (indice >= indiceIgnorado) { indice++; }
        }

        return indice;
    }
}

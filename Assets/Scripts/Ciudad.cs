using System.Collections.Generic;
using UnityEngine;

public class Ciudad : MonoBehaviour
{
    [SerializeField] private Seccion _seccion;
    [SerializeField] private Obstaculo Peaton;
    [SerializeField] private Obstaculo Trafico;
    [SerializeField] private Obstaculo Escuela;

    [SerializeField] private int _maxIteraciones = 15;
    [SerializeField] private int _minDistanciaDeRuta = 3;
    [SerializeField] private bool _pintarExtremos = false;
    [SerializeField] private bool _pintarCamino = false;

    private Seccion[] _secciones;
    private Seccion _NO;
    private Seccion _SO;
    private Seccion _NE;
    private Seccion _SE;

    private Seccion[] _seccionesDesordenadas 
    {
        get 
        {
            Seccion[] secciones = _secciones;
            int n = secciones.Length;
            int k;
            while (n > 1)
            {
                k = Random.Range(0, n--);
                (secciones[k], secciones[n]) = (secciones[n], secciones[k]);
            }
            return secciones;
        }
    }
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
    public List<Bloque> CrearRuta() { return CrearRuta(ObtenerCalleAleatoria()); }
    public List<Bloque> CrearRuta(Bloque origen)
    {
        List<Bloque> ruta;
        int distanciaMinima = _minDistanciaDeRuta;
        
        int iteracion = 0;
        do
        {
            ruta = Nodo.CalcularRuta(_NO.NO.NO, origen, ObtenerCalleAleatoria());
            iteracion++;
        }
        while ((ruta == null || ruta.Count <= distanciaMinima) && iteracion < _maxIteraciones);

        return ruta;
    }
    public void ReiniciarNivel() { Nivel = -1; }
    public void SiguienteNivel() { Nivel++; }

    private void Limpiar() 
    {
        if (_NO != null) { Destroy(_NO.gameObject); }
        if (_SO != null) { Destroy(_SO.gameObject); }
        if (_NE != null) { Destroy(_NE.gameObject); }
        if (_SE != null) { Destroy(_SE.gameObject); }
    }
    private void Instanciar() 
    {
        _NO = Instantiate(_seccion, transform.position + new Vector3(-2, 2, 0), transform.rotation, transform);
        _SO = Instantiate(_seccion, transform.position + new Vector3(-2, -2, 0), transform.rotation, transform);
        _NE = Instantiate(_seccion, transform.position + new Vector3(2, 2, 0), transform.rotation, transform);
        _SE = Instantiate(_seccion, transform.position + new Vector3(2, -2, 0), transform.rotation, transform);

        _secciones = new[] { _NO, _NE, _SO, _SE };
    }
    private void Conectar()
    {
        _NO.Sur = _SO;
        _NO.Este = _NE;

        _SO.Norte = _NO;
        _SO.Este = _SE;

        _NE.Sur = _SE;
        _NE.Oeste = _NO;

        _SE.Norte = _NE;
        _SE.Oeste = _SO;
    }
    private void Inicializar()
    {
        _NO.Inicializar();
        _SO.Inicializar();
        _NE.Inicializar();
        _SE.Inicializar();
    }
    private void ActualizarConexiones() 
    {
        _NO.ActualizarConexiones();
        _SO.ActualizarConexiones();
        _NE.ActualizarConexiones();
        _SE.ActualizarConexiones();
    }
    private void GenerarSecciones()
    {
        int iteracion;
        
        iteracion = 0; do { _NO.SetSeccion(); iteracion++; } while (_NO.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _SO.SetNorte(); iteracion++; } while (_SO.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _NE.SetOeste(); iteracion++; } while (_NE.TieneRotonda && iteracion < _maxIteraciones);
        iteracion = 0; do { _SE.SetNorteOeste(); iteracion++; } while (_SE.TieneRotonda && iteracion < _maxIteraciones);
    }
    private void ActualizarImagenes()
    {
        _NO.ActualizarImagenes();
        _SO.ActualizarImagenes();
        _NE.ActualizarImagenes();
        _SE.ActualizarImagenes();
    }
    private void ActualizarNivel() 
    {
        if (_NO != null) { _NO.Activar(); }
        if (_SO != null) { _SO.Activar(); }
        if (_SE != null) { _SE.Activar(); }
        if (_NE != null) { _NE.Activar(); }

        ActualizarImagenes();
    }
    private void GenerarObstaculos()
    {
        int cuadrante1;
        int cuadrante2;
        int cuadrante3;
        Cuadrante[] cuadrantes;

        int cantidadPeatonesMax = 0;
        int cantidadEscuelasMax = 0;
        int cantidadTraficoMax = 0;

        int cantidadPeatones = 0;
        int cantidadEscuelas = 0;
        int cantidadTrafico = 0;

        if (Nivel == -1) { cantidadPeatonesMax = 1; cantidadEscuelasMax = 2; cantidadTraficoMax = 1; }
        if (Nivel == 0)  { cantidadPeatonesMax = 1; cantidadEscuelasMax = 2; cantidadTraficoMax = 1; }
        if (Nivel == 1)  { cantidadPeatonesMax = 2; cantidadEscuelasMax = 2; cantidadTraficoMax = 1; }
        if (Nivel == 2)  { cantidadPeatonesMax = 2; cantidadEscuelasMax = 3; cantidadTraficoMax = 1; }
        if (Nivel == 3)  { cantidadPeatonesMax = 2; cantidadEscuelasMax = 3; cantidadTraficoMax = 2; }
        if (Nivel >= 4)  { cantidadPeatonesMax = 2; cantidadEscuelasMax = 4; cantidadTraficoMax = 2; }
        //if (Nivel == 5)  { cantidadPeatonesMax = 3; cantidadEscuelasMax = 4; cantidadTraficoMax = 2; }
        //if (Nivel >= 6)  { cantidadPeatonesMax = 3; cantidadEscuelasMax = 4; cantidadTraficoMax = 3; }
        
        foreach (Seccion seccion in _seccionesDesordenadas) 
        {
            cuadrantes = seccion.Cuadrantes;

            cuadrante1 = Random.Range(0, 4);
            cuadrante2 = RandomRange(0, 4, cuadrante1);
            cuadrante3 = RandomRange(0, 4, cuadrante1, cuadrante2);

            if (cantidadEscuelas < cantidadEscuelasMax) { seccion.Cuadrantes[cuadrante1].GenerarObstaculo(Escuela); cantidadEscuelas++; }
            if (cantidadPeatones < cantidadPeatonesMax) { seccion.Cuadrantes[cuadrante2].GenerarObstaculo(Peaton); cantidadPeatones++; }
            if (cantidadTrafico < cantidadTraficoMax) { seccion.Cuadrantes[cuadrante3].GenerarObstaculo(Trafico); cantidadTrafico++; }            
        }
    }
    public Bloque ObtenerCalleAleatoria() 
    {
        int seccion = RandomRange(0, 4);
        int cuadrante = Random.Range(0, 4);

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

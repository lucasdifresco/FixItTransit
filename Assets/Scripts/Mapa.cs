using System.Collections.Generic;
using UnityEngine;

public class Mapa : MonoBehaviour
{
    public Seccion prefab;
    public Vehiculo prefab_vehiculo;
    
    public Obstaculo Peaton;
    public Obstaculo Escuela;

    public Herramienta Senda;
    public Herramienta Estacionamiento;

    public int maxIteraciones = 10;
    public int MinDistanciaDeRuta = 8;
    public bool pintarExtremos = false;
    public bool pintarCamino = false;

    private Seccion[] secciones;

    private Seccion _centro;

    private Seccion _norte;
    private Seccion _sur;
    private Seccion _oeste;
    private Seccion _este;

    private Seccion _norteOeste;
    private Seccion _surOeste;
    private Seccion _norteEste;
    private Seccion _surEste;

    private int nivel = 0;

    public bool Crear = false;
    public bool siguienteNivel = false;
    public bool obtenerCalle = false;
    private int ultimaSeccion = -1;
    public bool calcularCamino = false;

    private void OnValidate()
    {
        if (Crear) { Crear = false; CrearMapa(); }
        if (obtenerCalle) { obtenerCalle = false; ObtenerCalleAleatoria(ultimaSeccion); }
        if (calcularCamino) 
        {
            calcularCamino = false;
            CrearCamino();
        }
        if (siguienteNivel) 
        {
            siguienteNivel = false; 
            nivel++;
            if (nivel > 2) { nivel = 0; }
            ActualizarNivel();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider == null) { return; }

            if (!hit.collider.TryGetComponent<Bloque>(out var bloque)) { return; }
            if (!bloque.Calle) { return; }
            bloque.GenerarHerramienta(Senda);
            bloque.ActualizarImagen();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider == null) { return; }

            if (!hit.collider.TryGetComponent<Bloque>(out var bloque)) { return; }
            if (!bloque.Calle) { return; }
            bloque.GenerarHerramienta(Estacionamiento);
            bloque.ActualizarImagen();
        }
    }

    private bool TirarMoneda { get { return Random.Range(0f, 1f) >= 0.5f; } }

    public void CrearMapa()
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
    public void CrearCamino()
    {
        List<Bloque> ruta;
        int distanciaMinima = MinDistanciaDeRuta;
        if (nivel == 0) { distanciaMinima = 3; }

        int iteracion = 0;
        do
        {
            ruta = Nodo.CalcularRuta(_norteOeste.NO.NO, ObtenerCalleAleatoria(ultimaSeccion), ObtenerCalleAleatoria(ultimaSeccion));
            iteracion++;
        }
        while ((ruta == null || ruta.Count <= distanciaMinima) && iteracion < maxIteraciones);

        PintarRuta(ruta);
        if (prefab_vehiculo == null) { print("No hay vehiculo"); return; }

        Vehiculo vehiculo = Instantiate(prefab_vehiculo, transform.position, transform.rotation, transform);
        vehiculo._ruta = ruta;
        vehiculo.Preparar();
        vehiculo.TransitarRuta();
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
        _centro = Instantiate(prefab, transform.position, transform.rotation, transform);

        _norte = Instantiate(prefab, transform.position + new Vector3( 0,  4, 0), transform.rotation, transform);
        _sur   = Instantiate(prefab, transform.position + new Vector3( 0, -4, 0), transform.rotation, transform);
        _oeste = Instantiate(prefab, transform.position + new Vector3(-4,  0, 0), transform.rotation, transform);
        _este  = Instantiate(prefab, transform.position + new Vector3( 4,  0, 0), transform.rotation, transform);

        _norteOeste = Instantiate(prefab, transform.position + new Vector3(-4,  4, 0), transform.rotation, transform);
        _surOeste   = Instantiate(prefab, transform.position + new Vector3(-4, -4, 0), transform.rotation, transform);
        _norteEste  = Instantiate(prefab, transform.position + new Vector3( 4,  4, 0), transform.rotation, transform);
        _surEste    = Instantiate(prefab, transform.position + new Vector3( 4, -4, 0), transform.rotation, transform);

        secciones = new[] { _centro, _norte, _sur, _oeste, _este, _norteOeste, _norteEste, _surOeste, _surEste };
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

        iteracion = 0; do { _centro.SetSeccion(); iteracion++; } while (_centro.TieneRotonda && iteracion < maxIteraciones);
        
        iteracion = 0; do { _norte.SetSur(); iteracion++; } while (_norte.TieneRotonda && iteracion < maxIteraciones);
        iteracion = 0; do { _sur.SetNorte(); iteracion++; } while (_sur.TieneRotonda && iteracion < maxIteraciones);
        iteracion = 0; do { _oeste.SetEste(); iteracion++; } while (_oeste.TieneRotonda && iteracion < maxIteraciones);
        iteracion = 0; do { _este.SetOeste(); iteracion++; } while (_este.TieneRotonda && iteracion < maxIteraciones);
        
        iteracion = 0; do { _norteOeste.SetSurEste(); iteracion++; } while (_norteOeste.TieneRotonda && iteracion < maxIteraciones);
        iteracion = 0; do { _surOeste.SetNorteEste(); iteracion++; } while (_surOeste.TieneRotonda && iteracion < maxIteraciones);
        iteracion = 0; do { _norteEste.SetSurOeste(); iteracion++; } while (_norteEste.TieneRotonda && iteracion < maxIteraciones);
        iteracion = 0; do { _surEste.SetNorteOeste(); iteracion++; } while (_surEste.TieneRotonda && iteracion < maxIteraciones);
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
        if (nivel == 0)
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
        else if (nivel == 1) 
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
        else if (nivel == 2)
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

        ActualizarImagenes();
    }
    
    private void GenerarObstaculos()
    {
        int cuadrante1;
        int cuadrante2;
        Cuadrante[] cuadrantes;

        foreach (Seccion seccion in secciones) 
        {
            cuadrantes = seccion.Cuadrantes;

            cuadrante1 = Random.Range(0, 4);
            cuadrante2 = RandomRange(0, 4, cuadrante1);

            if (cuadrante1 == cuadrante2) { print("Error"); }

            if (TirarMoneda)
            {
                seccion.Cuadrantes[cuadrante1].GenerarObstaculo(Peaton);
                seccion.Cuadrantes[cuadrante2].GenerarObstaculo(Escuela);
            }
            else if (TirarMoneda) { seccion.Cuadrantes[cuadrante1].GenerarObstaculo(Peaton); }
            else { seccion.Cuadrantes[cuadrante2].GenerarObstaculo(Escuela); }
        }
    }
    private Bloque ObtenerCalleAleatoria(int seccionProhibida = -1) 
    {
        int seccion;
        int cuadrante;

        if (nivel == 0)
        {
            seccion = 0;
            cuadrante = Random.Range(0, 4);
        }
        else if (nivel == 1)
        {
            seccion = RandomRange(0, 5, ultimaSeccion);
            cuadrante = Random.Range(0, 4);
        }
        else 
        {
            seccion = RandomRange(0, 9, ultimaSeccion);
            cuadrante = Random.Range(0, 4);
        }

        ultimaSeccion = seccion;
        List<Bloque> calles = secciones[seccion].Cuadrantes[cuadrante].Calles;

        Bloque bloque = calles[Random.Range(0, calles.Count)];
        bloque.Imagen.color = Color.grey;
        return bloque;
    }
    private void PintarRuta(List<Bloque> ruta)
    {
        if (pintarCamino)
        {
            foreach (Bloque bloque in ruta) { bloque.Imagen.color = Color.gray; }
        }

        if (pintarExtremos)
        {
            ruta[0].Imagen.color = Color.red;
            ruta[^1].Imagen.color = Color.green;
        }
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

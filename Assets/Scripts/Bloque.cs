using UnityEngine;

public class Bloque : MonoBehaviour
{
    public static bool EsEdificio(Bloque bloque) { return bloque == null || !bloque.Calle || !bloque.gameObject.activeSelf; }
    public static bool EsCalle(Bloque bloque) { return bloque != null && bloque.Calle && bloque.gameObject.activeSelf; }
    
    public HojaDeImagenes HojaDeImagenes;
    public SpriteRenderer Imagen;

    public bool Calle { get; set; }
    public Bloque Norte { get; set; }
    public Bloque Sur { get; set; }
    public Bloque Este { get; set; }
    public Bloque Oeste { get; set; }
    
    public int CantidadDeCallesLinderas { get { return ((EsEdificio(Norte)) ? 0 : 1) + ((EsEdificio(Sur)) ? 0 : 1) + ((EsEdificio(Este)) ? 0 : 1) + ((EsEdificio(Oeste)) ? 0 : 1); } }
    public int ObtenerIdImagen
    {
        get
        {
            if (!Calle) { return 0; }

            int calles = CantidadDeCallesLinderas;
            if (calles == 1)
            {
                if (EsCalle(Norte)) { return 1; }
                else if (EsCalle(Sur)) { return 2; }
                else if (EsCalle(Este)) { return 3; }
                else if (EsCalle(Oeste)) { return 4; }
            }
            else if (calles == 2)
            {
                if (EsCalle(Norte) && EsCalle(Este)) { return 5; }
                else if (EsCalle(Norte) && EsCalle(Oeste)) { return 6; }
                else if (EsCalle(Norte) && EsCalle(Sur)) { return 7; }
                else if (EsCalle(Sur) && EsCalle(Este)) { return 8; }
                else if (EsCalle(Sur) && EsCalle(Oeste)) { return 9; }
                else { return 10; }
            }
            else if (calles == 3)
            {
                if (EsCalle(Norte) && EsCalle(Este) && EsCalle(Sur)) { return 11; }
                else if (EsCalle(Este) && EsCalle(Sur) && EsCalle(Oeste)) { return 12; }
                else if (EsCalle(Sur) && EsCalle(Oeste) && EsCalle(Norte)) { return 13; }
                else { return 14; }
            }
            else if (calles == 4) { return 15; }

            return 0;
        }
    }
    public Bloque[] Vecinos { get { return new Bloque[] { Norte, Este, Sur, Oeste }; } }

    public Nodo Nodo { get; set; } = new Nodo();
    
    public Obstaculo Obstaculo { get; set; }
    public Herramienta Herramienta { get; set; }


    public void Activar() { gameObject.SetActive(true); if (Obstaculo != null) { Obstaculo.Activar(); } }
    public void Desactivar() { gameObject.SetActive(false); if (Obstaculo != null) { Obstaculo.Desactivar(); } }

    public void GenerarObstaculo(Obstaculo obstaculo)
    {
        if (Obstaculo != null) { Destroy(Obstaculo.gameObject); }

        Obstaculo = Instantiate(obstaculo, transform.position, Quaternion.identity, transform);
    }
    public void GenerarHerramienta(Herramienta herramienta) 
    {
        if (Herramienta != null) { Destroy(Herramienta.gameObject); }
        Herramienta = Instantiate(herramienta, transform.position, Quaternion.identity, transform);
    }
    public void ActualizarImagen() 
    {
        int idImagen = ObtenerIdImagen;

             if (idImagen ==  0) { Imagen.sprite = HojaDeImagenes.SinConexiones; }
        else if (idImagen ==  1) { Imagen.sprite = HojaDeImagenes.N; }
        else if (idImagen ==  2) { Imagen.sprite = HojaDeImagenes.S; }
        else if (idImagen ==  3) { Imagen.sprite = HojaDeImagenes.E; }
        else if (idImagen ==  4) { Imagen.sprite = HojaDeImagenes.O; }
        else if (idImagen ==  5) { Imagen.sprite = HojaDeImagenes.NE; }
        else if (idImagen ==  6) { Imagen.sprite = HojaDeImagenes.NO; }
        else if (idImagen ==  7) { Imagen.sprite = HojaDeImagenes.NS; }
        else if (idImagen ==  8) { Imagen.sprite = HojaDeImagenes.SE; }
        else if (idImagen ==  9) { Imagen.sprite = HojaDeImagenes.SO; }
        else if (idImagen == 10) { Imagen.sprite = HojaDeImagenes.EO; }
        else if (idImagen == 11) { Imagen.sprite = HojaDeImagenes.NES; }
        else if (idImagen == 12) { Imagen.sprite = HojaDeImagenes.ESO; }
        else if (idImagen == 13) { Imagen.sprite = HojaDeImagenes.SON; }
        else if (idImagen == 14) { Imagen.sprite = HojaDeImagenes.ONE; }
        else if (idImagen == 15) { Imagen.sprite = HojaDeImagenes.ONES; }

        if (Obstaculo != null) { Obstaculo.ActualizarImagen(idImagen); }
        if (Herramienta != null) { Herramienta.ActualizarImagen(idImagen); }
    }
}

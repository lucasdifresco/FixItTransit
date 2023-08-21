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
    public Nodo Nodo { get; set; } = new Nodo();
    public Bloque[] Vecinos { get { return new Bloque[] { Norte, Este, Sur, Oeste }; } }

    public void Activar() { gameObject.SetActive(true); }
    public void Desactivar() { gameObject.SetActive(false); }

    public void ActualizarImagen() 
    {
        if (!Calle) { Imagen.sprite = HojaDeImagenes.SinConexiones; return; }

        int calles = CantidadDeCallesLinderas;
        if (calles == 1) 
        {
            if (EsCalle(Norte)) { Imagen.sprite = HojaDeImagenes.N; }
            else if (EsCalle(Sur)) { Imagen.sprite = HojaDeImagenes.S; }
            else if (EsCalle(Este)) { Imagen.sprite = HojaDeImagenes.E; }
            else if (EsCalle(Oeste)) { Imagen.sprite = HojaDeImagenes.O; }
        }
        else if (calles == 2) 
        {
            if (EsCalle(Norte) && EsCalle(Este)) { Imagen.sprite = HojaDeImagenes.NE; }
            else if (EsCalle(Norte) && EsCalle(Oeste)) { Imagen.sprite = HojaDeImagenes.NO; }
            else if (EsCalle(Norte) && EsCalle(Sur)) { Imagen.sprite = HojaDeImagenes.NS; }
            else if (EsCalle(Sur) && EsCalle(Este)) { Imagen.sprite = HojaDeImagenes.SE; }
            else if (EsCalle(Sur) && EsCalle(Oeste)) { Imagen.sprite = HojaDeImagenes.SO; }
            else { Imagen.sprite = HojaDeImagenes.EO; }
        }
        else if (calles == 3) 
        {
            if (EsCalle(Norte) && EsCalle(Este) && EsCalle(Sur)) { Imagen.sprite = HojaDeImagenes.NES; }
            else if (EsCalle(Este) && EsCalle(Sur) && EsCalle(Oeste)) { Imagen.sprite = HojaDeImagenes.ESO; }
            else if (EsCalle(Sur) && EsCalle(Oeste) && EsCalle(Norte)) { Imagen.sprite = HojaDeImagenes.SON; }
            else { Imagen.sprite = HojaDeImagenes.ONE; }
        }
        else if (calles == 4) { Imagen.sprite = HojaDeImagenes.ONES; }
    }
}

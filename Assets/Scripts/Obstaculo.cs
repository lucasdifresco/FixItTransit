using UnityEngine;

public class Obstaculo : MonoBehaviour
{
    public HojaDeImagenes HojaDeImagenes;
    public SpriteRenderer Imagen;

    [field: SerializeField] public int Multa { get; private set; } = 10;
    [field: SerializeField] public bool EsPeaton { get; set; }
    [field: SerializeField] public bool EsEscuela { get; set; }

    public void Activar() { gameObject.SetActive(true); }
    public void Desactivar() { gameObject.SetActive(false); }

    public void ActualizarImagen(int idImagen)
    {
             if (idImagen ==  0) { if (HojaDeImagenes.SinConexiones == null) { return; } Imagen.sprite = HojaDeImagenes.SinConexiones; }
        
        else if (idImagen ==  1) { if (HojaDeImagenes.N  == null) { return; } Imagen.sprite = HojaDeImagenes.N; }
        else if (idImagen ==  2) { if (HojaDeImagenes.S  == null) { return; } Imagen.sprite = HojaDeImagenes.S; }
        else if (idImagen ==  3) { if (HojaDeImagenes.E  == null) { return; } Imagen.sprite = HojaDeImagenes.E; }
        else if (idImagen ==  4) { if (HojaDeImagenes.O  == null) { return; } Imagen.sprite = HojaDeImagenes.O; }

        else if (idImagen ==  5) { if (HojaDeImagenes.NE == null) { return; } Imagen.sprite = HojaDeImagenes.NE; }
        else if (idImagen ==  6) { if (HojaDeImagenes.NO == null) { return; } Imagen.sprite = HojaDeImagenes.NO; }
        else if (idImagen ==  7) { if (HojaDeImagenes.NS == null) { return; } Imagen.sprite = HojaDeImagenes.NS; }
        else if (idImagen ==  8) { if (HojaDeImagenes.SE == null) { return; } Imagen.sprite = HojaDeImagenes.SE; }
        else if (idImagen ==  9) { if (HojaDeImagenes.SO == null) { return; } Imagen.sprite = HojaDeImagenes.SO; }
        else if (idImagen == 10) { if (HojaDeImagenes.EO == null) { return; } Imagen.sprite = HojaDeImagenes.EO; }
        
        else if (idImagen == 11) { if (HojaDeImagenes.NES == null) { return; } Imagen.sprite = HojaDeImagenes.NES; }
        else if (idImagen == 12) { if (HojaDeImagenes.ESO == null) { return; } Imagen.sprite = HojaDeImagenes.ESO; }
        else if (idImagen == 13) { if (HojaDeImagenes.SON == null) { return; } Imagen.sprite = HojaDeImagenes.SON; }
        else if (idImagen == 14) { if (HojaDeImagenes.ONE == null) { return; } Imagen.sprite = HojaDeImagenes.ONE; }
        
        else if (idImagen == 15) { if (HojaDeImagenes.ONES == null) { return; } Imagen.sprite = HojaDeImagenes.ONES; }
    }
}

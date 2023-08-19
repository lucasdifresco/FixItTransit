using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hoja de Imagenes", menuName = "FixIt Transit/Hoja de Imagenes")]
public class HojaDeImagenes : ScriptableObject
{
    [Header("Sin Conexiones")]
    public Sprite SinConexiones;

    [Header("Una Conexión")]
    public Sprite O;
    public Sprite N;
    public Sprite E;
    public Sprite S;

    [Header("Dos Conexiones")]
    public Sprite NO;
    public Sprite NS;
    public Sprite NE;
    public Sprite SE;
    public Sprite EO;
    public Sprite SO;

    [Header("Tres Conexiones")]
    public Sprite SON;
    public Sprite ONE;
    public Sprite NES;
    public Sprite ESO;

    [Header("Cuatro Conexiones")]
    public Sprite ONES;
}

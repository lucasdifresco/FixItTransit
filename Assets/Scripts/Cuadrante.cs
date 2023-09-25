using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Cuadrante : MonoBehaviour
{
    [SerializeField] private Bloque _bloque;

    public Cuadrante Norte { get; set; }
    public Cuadrante Sur   { get; set; }
    public Cuadrante Este  { get; set; }
    public Cuadrante Oeste { get; set; }

    public Bloque NO { get; private set; }
    public Bloque NE { get; private set; }
    public Bloque SO { get; private set; }
    public Bloque SE { get; private set; }

    public bool SePuedeExpandirNorte { get { return NO.Calle || NE.Calle; } }
    public bool SePuedeExpandirSur   { get { return SO.Calle || SE.Calle; } }
    public bool SePuedeExpandirEste  { get { return NE.Calle || SE.Calle; } }
    public bool SePuedeExpandirOeste { get { return NO.Calle || SO.Calle; } }

    public List<Bloque> Calles 
    {
        get 
        {
            List<Bloque> calles = new();
            if (NO.Calle) { calles.Add(NO); }
            if (NE.Calle) { calles.Add(NE); }
            if (SO.Calle) { calles.Add(SO); }
            if (SE.Calle) { calles.Add(SE); }
            return calles;
        }
    }
    public List<Bloque> Edificios
    {
        get
        {
            List<Bloque> calles = new();
            if (!NO.Calle) { calles.Add(NO); }
            if (!NE.Calle) { calles.Add(NE); }
            if (!SO.Calle) { calles.Add(SO); }
            if (!SE.Calle) { calles.Add(SE); }
            return calles;
        }
    }

    public void Activar() 
    {
        if (NO != null) { NO.Activar(); }
        if (NE != null) { NE.Activar(); }
        if (SO != null) { SO.Activar(); }
        if (SE != null) { SE.Activar(); }
    }
    public void Desactivar() 
    {
        if (NO != null) { NO.Desactivar(); }
        if (NE != null) { NE.Desactivar(); }
        if (SO != null) { SO.Desactivar(); }
        if (SE != null) { SE.Desactivar(); }
    }
    public void Inicializar()
    {
        NO = Instantiate(_bloque, transform);
        NE = Instantiate(_bloque, transform);
        SO = Instantiate(_bloque, transform);
        SE = Instantiate(_bloque, transform);

        Posicionar();

        NO.Sur = SO;
        NE.Sur = SE;
        SO.Norte = NO;
        SE.Norte = NE;

        NO.Este = NE;
        SO.Este = SE;
        NE.Oeste = NO;
        SE.Oeste = SO;
    }
    public void ActualizarConexiones() 
    {
        if (Norte != null) 
        {
            NO.Norte = Norte.SO;
            NE.Norte = Norte.SE;
        }
        if (Sur != null) 
        {
            SO.Sur = Sur.NO;
            SE.Sur = Sur.NE;
        }
        if (Este != null) 
        {
            NE.Este = Este.NO;
            SE.Este = Este.SO;
        }
        if (Oeste != null) 
        {
            NO.Oeste = Oeste.NE;
            SO.Oeste = Oeste.SE;
        }
    }
    public void ActualizarImagenes() 
    {
        if (NO != null) { NO.ActualizarImagen(); }
        if (NE != null) { NE.ActualizarImagen(); }
        if (SO != null) { SO.ActualizarImagen(); }
        if (SE != null) { SE.ActualizarImagen(); }
    }
    public void SetCuadrante(params int[] formatosProhibidos) { SetFormato(formatosProhibidos.ToList()); }
    public bool SetNorte(params int[] formatosProhibidos)
    {
        if (Norte == null) { return false; }
        List<int> listaFormatosProhibidos = new();

        if (!Norte.SO.Calle && !Norte.SE.Calle) { return false; }
        else if (!Norte.SO.Calle &&  Norte.SE.Calle &&  Norte.NO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }
        else if (!Norte.SO.Calle &&  Norte.SE.Calle && !Norte.NO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8, 7 }); }
        else if ( Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 5, 6 }); }
        else if ( Norte.SO.Calle && !Norte.SE.Calle &&  Norte.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }
        else if ( Norte.SO.Calle && !Norte.SE.Calle && !Norte.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7, 8 }); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetSur(params int[] formatosProhibidos)
    {
        if (Sur == null) { return false; }
        List<int> listaFormatosProhibidos = new();

        if (!Sur.NO.Calle && !Sur.NE.Calle) { return false; }
        else if (!Sur.NO.Calle &&  Sur.NE.Calle &&  Sur.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8 }); }
        else if (!Sur.NO.Calle &&  Sur.NE.Calle && !Sur.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8, 7 }); }
        else if ( Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5, 6 }); }
        else if ( Sur.NO.Calle && !Sur.NE.Calle &&  Sur.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }
        else if ( Sur.NO.Calle && !Sur.NE.Calle && !Sur.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7, 8 }); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetOeste(params int[] formatosProhibidos)
    {
        if (Oeste == null) { return false; }
        List<int> listaFormatosProhibidos = new();

        if (!Oeste.NE.Calle && !Oeste.SE.Calle) { return false; }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle &&  Oeste.NO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle && !Oeste.NO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7, 5 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 7, 8 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle &&  Oeste.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle && !Oeste.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7, 6 }); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetEste(params int[] formatosProhibidos)
    {
        if (Este == null) { return false; }
        List<int> listaFormatosProhibidos = new();

        if (!Este.NO.Calle && !Este.SO.Calle) { return false; }
        else if (!Este.NO.Calle &&  Este.SO.Calle &&  Este.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8 }); }
        else if (!Este.NO.Calle &&  Este.SO.Calle && !Este.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8, 5 }); }
        else if ( Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7, 8 }); }
        else if ( Este.NO.Calle && !Este.SO.Calle &&  Este.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }
        else if ( Este.NO.Calle && !Este.SO.Calle && !Este.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8, 6 }); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetNorteOeste(params int[] formatosProhibidos)
    {
        if (Norte == null || Oeste == null) { return false; }
        List<int> listaFormatosProhibidos = new();

             if (!Oeste.NE.Calle && !Oeste.SE.Calle && !Norte.SO.Calle && !Norte.SE.Calle) { return false; }        
        else if (!Oeste.NE.Calle && !Oeste.SE.Calle && !Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }
        else if (!Oeste.NE.Calle && !Oeste.SE.Calle &&  Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 5, 6 }); }
        else if (!Oeste.NE.Calle && !Oeste.SE.Calle &&  Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }
        
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle && !Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle && !Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] {  }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle &&  Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 6 }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle &&  Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 7 }); }
        
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle && !Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 7, 8 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle && !Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 8 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle &&  Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 3, 6, 8 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle &&  Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 7, 8 }); }
        
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle && !Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle && !Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 5 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle &&  Norte.SO.Calle &&  Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 5, 6 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle &&  Norte.SO.Calle && !Norte.SE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }

        if (Oeste.NE.Calle && !Oeste.SE.Calle && !Oeste.SO.Calle && !listaFormatosProhibidos.Contains(6)) { listaFormatosProhibidos.Add(6); }
        if (Norte.SO.Calle && !Norte.SE.Calle && !Norte.NE.Calle && !listaFormatosProhibidos.Contains(8)) { listaFormatosProhibidos.Add(8); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetNorteEste(params int[] formatosProhibidos)
    {
        if (Norte == null || Este == null) { return false; }
        List<int> listaFormatosProhibidos = new();

             if (!Norte.SO.Calle && !Norte.SE.Calle && !Este.NO.Calle && !Este.SO.Calle) { return false; }
        else if (!Norte.SO.Calle &&  Norte.SE.Calle && !Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }
        else if ( Norte.SO.Calle &&  Norte.SE.Calle && !Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 5, 6 }); }
        else if ( Norte.SO.Calle && !Norte.SE.Calle && !Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }

        else if (!Norte.SO.Calle &&  Norte.SE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 8 }); }
        else if ( Norte.SO.Calle &&  Norte.SE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 6 }); }
        else if ( Norte.SO.Calle && !Norte.SE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] {  }); }
        else if (!Norte.SO.Calle && !Norte.SE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8 }); }

        else if (!Norte.SO.Calle &&  Norte.SE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7, 8 }); }
        else if ( Norte.SO.Calle &&  Norte.SE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 4, 6, 7 }); }
        else if ( Norte.SO.Calle && !Norte.SE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7 }); }
        else if (!Norte.SO.Calle && !Norte.SE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7, 8 }); }

        else if (!Norte.SO.Calle &&  Norte.SE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }
        else if ( Norte.SO.Calle &&  Norte.SE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 2, 5, 6 }); }
        else if ( Norte.SO.Calle && !Norte.SE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 5 }); }
        else if (!Norte.SO.Calle && !Norte.SE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }

        if (!Norte.NO.Calle && !Norte.SO.Calle && Norte.SE.Calle && !listaFormatosProhibidos.Contains(7)) { listaFormatosProhibidos.Add(7); }
        if (  Este.NO.Calle && !Este.SO.Calle  && !Este.SE.Calle && !listaFormatosProhibidos.Contains(6)) { listaFormatosProhibidos.Add(6); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetSurOeste(params int[] formatosProhibidos)
    {
        if (Sur == null || Oeste == null) { return false; }
        List<int> listaFormatosProhibidos = new();

             if (!Oeste.NE.Calle && !Oeste.SE.Calle && !Sur.NO.Calle && !Sur.NE.Calle) { return false; }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle && !Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle && !Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 6 }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle &&  Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5, 6 }); }
        else if (!Oeste.NE.Calle &&  Oeste.SE.Calle &&  Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }

        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle && !Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 7, 8 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle && !Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 8 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle &&  Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 4, 5, 8 }); }
        else if ( Oeste.NE.Calle &&  Oeste.SE.Calle &&  Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 3, 7, 8 }); }

        else if ( Oeste.NE.Calle && !Oeste.SE.Calle && !Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 4, 5, 7 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle && !Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] {  }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle &&  Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5 }); }
        else if ( Oeste.NE.Calle && !Oeste.SE.Calle &&  Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 7 }); }

        else if (!Oeste.NE.Calle && !Oeste.SE.Calle && !Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 7 }); }
        else if (!Oeste.NE.Calle && !Oeste.SE.Calle &&  Sur.NO.Calle &&  Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5, 6 }); }
        else if (!Oeste.NE.Calle && !Oeste.SE.Calle &&  Sur.NO.Calle && !Sur.NE.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }

        if (!Oeste.NO.Calle && !Oeste.NE.Calle && Oeste.SE.Calle && !listaFormatosProhibidos.Contains(5)) { listaFormatosProhibidos.Add(5); }
        if (   Sur.NO.Calle &&   !Sur.NE.Calle &&  !Sur.SE.Calle && !listaFormatosProhibidos.Contains(8)) { listaFormatosProhibidos.Add(8); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public bool SetSurEste(params int[] formatosProhibidos)
    {        
        if (Sur == null || Este == null) { return false; }
        List<int> listaFormatosProhibidos = new();

             if (!Sur.NO.Calle && !Sur.NE.Calle && !Este.NO.Calle && !Este.SO.Calle) { return false; }
        else if (!Sur.NO.Calle &&  Sur.NE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8 }); }
        else if ( Sur.NO.Calle &&  Sur.NE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5, 6 }); }
        else if ( Sur.NO.Calle && !Sur.NE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 6 }); }
        else if (!Sur.NO.Calle && !Sur.NE.Calle && !Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8 }); }

        else if (!Sur.NO.Calle &&  Sur.NE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7, 8 }); }
        else if ( Sur.NO.Calle &&  Sur.NE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 3, 4, 5, 7 }); }
        else if ( Sur.NO.Calle && !Sur.NE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7 }); }
        else if (!Sur.NO.Calle && !Sur.NE.Calle &&  Este.NO.Calle &&  Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 4, 7, 8 }); }

        else if (!Sur.NO.Calle &&  Sur.NE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 8 }); }
        else if ( Sur.NO.Calle &&  Sur.NE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5 }); }
        else if ( Sur.NO.Calle && !Sur.NE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] {  }); }
        else if (!Sur.NO.Calle && !Sur.NE.Calle &&  Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 5, 8 }); }

        else if (!Sur.NO.Calle &&  Sur.NE.Calle && !Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 1, 6, 8 }); }
        else if ( Sur.NO.Calle &&  Sur.NE.Calle && !Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 3, 4, 5, 6 }); }
        else if ( Sur.NO.Calle && !Sur.NE.Calle && !Este.NO.Calle && !Este.SO.Calle) { listaFormatosProhibidos.AddRange(new int[] { 2, 6, 7 }); }

        if ( !Sur.NO.Calle &&  !Sur.SO.Calle &&  Sur.NE.Calle && !listaFormatosProhibidos.Contains(7)) { listaFormatosProhibidos.Add(7); }
        if (!Este.NO.Calle && !Este.NE.Calle && Este.SO.Calle && !listaFormatosProhibidos.Contains(5)) { listaFormatosProhibidos.Add(5); }

        foreach (int formato in formatosProhibidos) { if (!listaFormatosProhibidos.Contains(formato)) { listaFormatosProhibidos.Add(formato); } }
        SetFormato(listaFormatosProhibidos);
        return true;
    }
    public void GenerarObstaculo(Obstaculo obstaculo) 
    {
        List<Bloque> calles = Calles;

        if (obstaculo.Tipo == Obstaculo.OBSTACULO.Escuela) 
        {
            List<Bloque> edificios = Edificios;
            edificios[Random.Range(0, edificios.Count)].GenerarObstaculo(obstaculo);
            foreach (Bloque calle in calles) { calle.GenerarObstaculo(obstaculo); }
        }
        else if (obstaculo.Tipo == Obstaculo.OBSTACULO.Peaton) { calles[Random.Range(0, calles.Count)].GenerarObstaculo(obstaculo); }
    }

    private void Posicionar(float desplazamiento = 0)
    {
        NO.transform.position = transform.position + new Vector3(-.5f - desplazamiento, .5f + desplazamiento, 0);
        NE.transform.position = transform.position + new Vector3(.5f + desplazamiento, .5f + desplazamiento, 0);
        SO.transform.position = transform.position + new Vector3(-.5f - desplazamiento, -.5f - desplazamiento, 0);
        SE.transform.position = transform.position + new Vector3(.5f + desplazamiento, -.5f - desplazamiento, 0);
    }
    private int ObtenerFormato(List<int> formatosProhibidos)
    {
        List<int> formatos = new() { 1, 2, 3, 4, 5, 6, 7, 8 };
        if (formatosProhibidos.Count == formatos.Count) { return 0; }
        
        for (int i = formatos.Count - 1; i >= 0; i--) 
        {
            foreach (int formatoProhibido in formatosProhibidos) 
            {
                if (formatos[i] == formatoProhibido) 
                {
                    formatos.RemoveAt(i); 
                    break;
                } 
            } 
        }

        return formatos[Random.Range(0, formatos.Count)];
    }
    private void SetFormato(List<int> formatosProhibidos) 
    {
        if (formatosProhibidos.Count == 8) { return; }
        int formato = ObtenerFormato(formatosProhibidos);
        SetFormato(formato);
    }
    private void SetFormato(int formato)
    {
        if (formato == 1) { NO.Calle = true; NE.Calle = true; SO.Calle = true; SE.Calle = false; }
        else if (formato == 2) { NO.Calle = true; NE.Calle = true; SO.Calle = false; SE.Calle = true; }
        else if (formato == 3) { NO.Calle = true; NE.Calle = false; SO.Calle = true; SE.Calle = true; }
        else if (formato == 4) { NO.Calle = false; NE.Calle = true; SO.Calle = true; SE.Calle = true; }
        else if (formato == 5) { NO.Calle = false; NE.Calle = false; SO.Calle = true; SE.Calle = true; }
        else if (formato == 6) { NO.Calle = true; NE.Calle = true; SO.Calle = false; SE.Calle = false; }
        else if (formato == 7) { NO.Calle = false; NE.Calle = true; SO.Calle = false; SE.Calle = true; }
        else if (formato == 8) { NO.Calle = true; NE.Calle = false; SO.Calle = true; SE.Calle = false; }
    }
}

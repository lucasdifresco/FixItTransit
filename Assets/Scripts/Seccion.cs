using UnityEngine;

public class Seccion : MonoBehaviour
{
    public Cuadrante Prefab;

    public Cuadrante[] Cuadrantes;

    public Seccion Norte { get; set; }
    public Seccion Sur { get; set; }
    public Seccion Este { get; set; }
    public Seccion Oeste { get; set; }

    public Cuadrante NO { get; private set; }
    public Cuadrante NE { get; private set; }
    public Cuadrante SO { get; private set; }
    public Cuadrante SE { get; private set; }

    public bool TieneRotonda 
    {
        get 
        {
            if (NO.SE.Calle && NE.SO.Calle && SE.NO.Calle && SO.NE.Calle) { return true; }

            if (Norte != null) { if (NO.NE.Calle && NE.NO.Calle && Norte.SO.SE.Calle && Norte.SE.SO.Calle) { return true; } }
            if (Sur != null)   { if (SO.SE.Calle && SE.SO.Calle &&   Sur.NO.NE.Calle &&   Sur.NE.NO.Calle) { return true; } }
            if (Este != null)  { if (NE.SE.Calle && SE.NE.Calle &&  Este.NO.SO.Calle &&  Este.SO.NO.Calle) { return true; } }
            if (Oeste != null) { if (NO.SO.Calle && SO.NO.Calle && Oeste.NE.SE.Calle && Oeste.SE.NE.Calle) { return true; } }

            if (Norte != null && Oeste != null && Norte.Oeste != null) { if (NO.NO.Calle && Norte.SO.SO.Calle && Oeste.NE.NE.Calle && Norte.Oeste.SE.SE.Calle) { return true; } }
            if (Norte != null &&  Este != null && Norte.Este  != null) { if (NE.NE.Calle && Norte.SE.SE.Calle &&  Este.NO.NO.Calle &&  Norte.Este.SO.SO.Calle) { return true; } }
            if (  Sur != null && Oeste != null &&   Sur.Oeste != null) { if (SO.SO.Calle &&   Sur.NO.NO.Calle && Oeste.SE.SE.Calle &&   Sur.Oeste.NE.NE.Calle) { return true; } }
            if (  Sur != null &&  Este != null &&   Sur.Este  != null) { if (SE.SE.Calle &&   Sur.NE.NE.Calle &&  Este.SO.SO.Calle &&    Sur.Este.NO.NO.Calle) { return true; } }

            return false; 
        }
    }
    private bool TirarMoneda { get { return Random.Range(0f, 1f) >= 0.5f; } }

    public void SetSeccion()
    {
        int index = Random.Range(1, 13);

        // NO
        if (index == 1)
        {
            NO.SetCuadrante(6, 8);
            NE.SetOeste(6);
            SO.SetNorte(8);
            SE.SetNorteOeste();
        }
        if (index == 2)
        {
            NO.SetCuadrante(8);
            NE.SetOeste(6);
            SE.SetNorte(7);
            SO.SetNorteEste();
        }
        if (index == 3)
        {
            NO.SetCuadrante(6);
            SO.SetNorte(8);
            SE.SetOeste(5);
            NE.SetSurOeste();
        }

        // NE
        if (index == 4)
        {
            NE.SetCuadrante(7, 6);
            NO.SetEste(6);
            SE.SetNorte(7);
            SO.SetNorteEste();
        }
        if (index == 5)
        {
            NE.SetCuadrante(6);
            SE.SetNorte(7);
            SO.SetEste(5);
            NO.SetSurEste();
        }
        if (index == 6)
        {
            NE.SetCuadrante(7);
            NO.SetEste(6);
            SO.SetNorte(8);
            SE.SetNorteOeste();
        }

        // SO
        if (index == 7)
        {
            SO.SetCuadrante(8, 5);
            NO.SetSur(8);
            SE.SetOeste(5);
            NE.SetSurOeste();
        }
        if (index == 8)
        {
            SO.SetCuadrante(5);
            NO.SetSur(8);
            NE.SetOeste(6);
            SE.SetNorteOeste();
        }
        if (index == 9)
        {
            SO.SetCuadrante(8);
            SE.SetOeste(5);
            NE.SetSur(7);
            NO.SetSurEste();
        }

        // SE
        if (index == 10)
        {
            SE.SetCuadrante(7, 5);
            SO.SetEste(5);
            NE.SetSur(7);
            NO.SetSurEste();
        }
        if (index == 11)
        {
            SE.SetCuadrante(7);
            SO.SetEste(5);
            NO.SetSur(8);
            NE.SetSurOeste();
        }
        if (index == 12)
        {
            SE.SetCuadrante(5);
            NE.SetSur(7);
            NO.SetEste(6);
            SO.SetNorteEste();
        }
    }
    public void SetNorte()
    {
        if ((Norte.SO.SePuedeExpandirSur && Norte.SE.SePuedeExpandirSur) ? TirarMoneda : Norte.SO.SePuedeExpandirSur)
        {
            if (TirarMoneda)
            {
                NO.SetNorte(6, 8); NE.SetNorteOeste(6);
                SO.SetNorte(8); SE.SetNorteOeste();
            }
            else
            {
                NO.SetNorte(8); NE.SetNorteOeste(6);
                SE.SetNorte(7); SO.SetNorteEste();
            }
        }
        else
        {
            if (TirarMoneda)
            {
                NE.SetNorte(6, 7); NO.SetNorteEste(6);
                SE.SetNorte(7); SO.SetNorteEste();
            }
            else
            {
                NE.SetNorte(7); NO.SetNorteEste(6);
                SO.SetNorte(8); SE.SetNorteOeste();
            }
        }
    }
    public void SetSur() 
    {        
        if ((Sur.NO.SePuedeExpandirNorte && Sur.NE.SePuedeExpandirNorte) ? TirarMoneda : Sur.NO.SePuedeExpandirNorte)
        {
            if (TirarMoneda) 
            {
                SO.SetSur(5, 8); SE.SetSurOeste(5);
                NO.SetSur(8); NE.SetSurOeste();
            }
            else 
            {
                SO.SetSur(8); SE.SetSurOeste(5);
                NE.SetSur(7); NO.SetSurEste();
            }
        }
        else
        {
            if (TirarMoneda) 
            {
                SE.SetSur(5, 7); SO.SetSurEste(5);
                NE.SetSur(7); NO.SetSurEste();
            }
            else 
            {
                SE.SetSur(7); SO.SetSurEste(5);
                NO.SetSur(8); NE.SetSurOeste();
            }
        }
    } 
    public void SetEste() 
    {
        if ((Este.NO.SePuedeExpandirOeste && Este.SO.SePuedeExpandirOeste) ? TirarMoneda : Este.NO.SePuedeExpandirOeste)
        {
            if (TirarMoneda)
            {
                NE.SetEste(6, 7); SE.SetNorteEste(7);
                NO.SetEste(6); SO.SetNorteEste();
            }
            else
            {
                NE.SetEste(6); SE.SetNorteEste(7);
                SO.SetEste(5); NO.SetSurEste();
            }
        }
        else
        {

            if (TirarMoneda) 
            {
                SE.SetEste(5, 7); NE.SetSurEste(7);
                SO.SetEste(5); NO.SetSurEste();
            }
            else 
            { 
                SE.SetEste(5); NE.SetSurEste(7);
                NO.SetEste(6); SO.SetNorteEste();
            }
        }
    }
    public void SetOeste() 
    {
        if ((Oeste.NE.SePuedeExpandirEste && Oeste.SE.SePuedeExpandirEste) ? TirarMoneda : Oeste.NE.SePuedeExpandirEste)
        {
            if (TirarMoneda) 
            {
                NO.SetOeste(6, 8); SO.SetNorteOeste(8);
                NE.SetOeste(6); SE.SetNorteOeste();
            }
            else 
            {
                NO.SetOeste(6); SO.SetNorteOeste(8);
                SE.SetOeste(5); NE.SetSurOeste();
            }
        }
        else
        {
            if (TirarMoneda)
            {
                SO.SetOeste(5, 8); NO.SetSurOeste(8);
                SE.SetOeste(5); NE.SetSurOeste();
            }
            else
            {
                SO.SetOeste(5); NO.SetSurOeste(8);
                NE.SetOeste(6); SE.SetNorteOeste();
            }
        }
    }

    public void SetNorteOeste() 
    {
        if (!Norte.SO.SePuedeExpandirSur && !Oeste.NE.SePuedeExpandirEste) 
        {
            NO.SetCuadrante(6, 8); NE.SetNorteOeste(6, 5);
            SO.SetNorteOeste(8, 7); SE.SetNorteOeste();
        }
        else 
        {
            NO.SetNorteOeste(6, 8);
            NE.SetNorteOeste(6);
            SO.SetNorteOeste(8);
            SE.SetNorteOeste();
        }
    }
    public void SetSurOeste() 
    {
        if (!Sur.NO.SePuedeExpandirNorte && !Oeste.SE.SePuedeExpandirEste) 
        {
            SO.SetCuadrante(5, 8); SE.SetSurOeste(5, 6);
            NO.SetSurOeste(8, 7); NE.SetSurOeste();
        }
        else 
        {
            SO.SetSurOeste(5, 8);
            SE.SetSurOeste(5);
            NO.SetSurOeste(8);
            NE.SetSurOeste();
        }
    }
    public void SetNorteEste()
    {
        if (!Norte.SE.SePuedeExpandirSur && !Este.NO.SePuedeExpandirOeste) 
        {
            NE.SetCuadrante(6, 7); NO.SetNorteEste(6, 5);
            SE.SetNorteEste(7, 8); SO.SetNorteEste();
        }
        else 
        {
            NE.SetNorteEste(6, 7);
            NO.SetNorteEste(6);
            SE.SetNorteEste(7);
            SO.SetNorteEste();
        }
    }
    public void SetSurEste() 
    {
        if (!Sur.NE.SePuedeExpandirNorte && !Este.SO.SePuedeExpandirOeste) 
        {
            SE.SetCuadrante(5, 7); NE.SetSurEste(7, 8);
            SO.SetSurEste(5, 6); NO.SetSurEste();
        }
        else 
        { 
            SE.SetSurEste(5, 7);
            NE.SetSurEste(7);
            SO.SetSurEste(5);
            NO.SetSurEste();
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
        NO = Instantiate(Prefab, transform);
        NE = Instantiate(Prefab, transform);
        SO = Instantiate(Prefab, transform);
        SE = Instantiate(Prefab, transform);

        Posisionar();

        NO.Inicializar();
        NE.Inicializar();
        SO.Inicializar();
        SE.Inicializar();

        NO.Sur = SO;
        NE.Sur = SE;
        SO.Norte = NO;
        SE.Norte = NE;

        NO.Este = NE;
        SO.Este = SE;
        NE.Oeste = NO;
        SE.Oeste = SO;

        Cuadrantes = new[] { NO, NE, SO, SE };
    }
    public void Posisionar(float desplazamiento = 0) 
    {
        NO.transform.position = transform.position + new Vector3(-1 - desplazamiento,  1 + desplazamiento, 0);
        NE.transform.position = transform.position + new Vector3( 1 + desplazamiento,  1 + desplazamiento, 0);
        SO.transform.position = transform.position + new Vector3(-1 - desplazamiento, -1 - desplazamiento, 0);
        SE.transform.position = transform.position + new Vector3( 1 + desplazamiento, -1 - desplazamiento, 0);
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

        NO.ActualizarConexiones();
        NE.ActualizarConexiones();
        SO.ActualizarConexiones();
        SE.ActualizarConexiones();
    }
    public void ActualizarImagenes() 
    {
        NO.ActualizarImagenes();
        NE.ActualizarImagenes();
        SO.ActualizarImagenes();
        SE.ActualizarImagenes();
    }
}

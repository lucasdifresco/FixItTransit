using System.Collections.Generic;
using UnityEngine;

public class Nodo
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool EsValido { get; set; } = false;

    public Bloque Anterior { get; set; }
    public int G { get; set; } = int.MaxValue;
    public int H { get; set; }
    public int F { get { return G + H; } }

    public int Distancia(Bloque destino) { return Mathf.Abs(Mathf.Abs(X - destino.Nodo.X) - Mathf.Abs(Y - destino.Nodo.Y)); }
    public int CostoTentativa(Bloque destino) { return G + Distancia(destino); }
    
    public static List<Bloque> CalcularRuta(Bloque origenDeCoordenadas, Bloque origen, Bloque destino)
    {
        List<Bloque> nodosAbiertos = new() { origen };
        List<Bloque> nodosCerrados = new();

        Bloque actual;
        int costoTentativo;

        InicializarNodos(origenDeCoordenadas);
        origen.Nodo.G = 0;
        origen.Nodo.H = origen.Nodo.Distancia(destino);

        while (nodosAbiertos.Count > 0)
        {
            actual = ObtenerNodoConMenorF(nodosAbiertos);
            if (actual == destino) { return GenerarRutaDesdeElDestino(destino); }

            nodosAbiertos.Remove(actual);
            nodosCerrados.Add(actual);

            foreach (Bloque vecino in actual.Vecinos)
            {
                if (vecino == null) { continue; }
                if (nodosCerrados.Contains(vecino)) { continue; }
                if (!vecino.Nodo.EsValido) { nodosCerrados.Add(vecino); continue; }

                costoTentativo = actual.Nodo.CostoTentativa(vecino);
                if (costoTentativo < vecino.Nodo.G)
                {
                    vecino.Nodo.Anterior = actual;
                    vecino.Nodo.G = costoTentativo;
                    vecino.Nodo.H = vecino.Nodo.Distancia(destino);
                    if (!nodosAbiertos.Contains(vecino)) { nodosAbiertos.Add(vecino); }
                }
            }
        }

        MonoBehaviour.print("Ruta no encontrada");
        return null;
    }
    public static Bloque ObtenerNodoConMenorF(List<Bloque> bloques)
    {
        Bloque bloqueMasBarato = bloques[0];

        for (int i = 0; i < bloques.Count; i++)
        {
            if (bloques[i].Nodo.F < bloqueMasBarato.Nodo.F)
            {
                bloqueMasBarato = bloques[i];
            }
        }

        return bloqueMasBarato;
    }
    private static void InicializarNodos(Bloque origen)
    {
        Bloque fila = origen;
        Bloque columna = fila;

        int x = 0;
        int y = 0;

        while (fila != null)
        {
            while (columna != null)
            {
                columna.Nodo.EsValido = Bloque.EsCalle(columna);
                columna.Nodo.X = x;
                columna.Nodo.Y = y;
                columna.Nodo.G = int.MaxValue;
                columna.Nodo.H = 0;
                columna.Nodo.Anterior = null;

                columna.Imagen.color = Color.white;

                x++;
                columna = columna.Este;
            }
            x = 0;
            y++;
            fila = fila.Sur;
            columna = fila;
        }
    }

    private static List<Bloque> GenerarRutaDesdeElDestino(Bloque destino)
    {
        List<Bloque> ruta = new() { destino };
        Bloque actual = destino;

        while (actual.Nodo.Anterior != null)
        {
            ruta.Add(actual.Nodo.Anterior);
            actual = actual.Nodo.Anterior;
        }

        ruta.Reverse();
        return ruta;
    }
}

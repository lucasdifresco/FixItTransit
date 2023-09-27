using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marcador : MonoBehaviour
{
    //[SerializeField] private Ciudad _ciudad;

    [SerializeField] private GameObject NN;
    [SerializeField] private GameObject NO;
    [SerializeField] private GameObject N;
    [SerializeField] private GameObject NE;
    [SerializeField] private GameObject OO;
    [SerializeField] private GameObject O;
    [SerializeField] private GameObject Centro;
    [SerializeField] private GameObject E;
    [SerializeField] private GameObject EE;
    [SerializeField] private GameObject SO;
    [SerializeField] private GameObject S;
    [SerializeField] private GameObject SE;
    [SerializeField] private GameObject SS;

    public void Posicionar(Bloque nuevaPosicion) 
    {
        transform.position = nuevaPosicion.transform.position;

        NN.SetActive(false);
        NO.SetActive(false);
         N.SetActive(false);
        NE.SetActive(false);
        OO.SetActive(false);
         O.SetActive(false);
         E.SetActive(false);
        EE.SetActive(false);
        SO.SetActive(false);
         S.SetActive(false);
        SE.SetActive(false);
        SS.SetActive(false);

        if (Bloque.EsCalle(nuevaPosicion.Norte)) { N.SetActive(true); }
        if (Bloque.EsCalle(nuevaPosicion.Sur)) { S.SetActive(true); }
        if (Bloque.EsCalle(nuevaPosicion.Este)) { E.SetActive(true); }
        if (Bloque.EsCalle(nuevaPosicion.Oeste)) { O.SetActive(true); }

        if (N.activeSelf && Bloque.EsCalle(nuevaPosicion.Norte.Norte)) { NN.SetActive(true); }
        if (S.activeSelf && Bloque.EsCalle(nuevaPosicion.Sur.Sur)) { SS.SetActive(true); }
        if (E.activeSelf && Bloque.EsCalle(nuevaPosicion.Este.Este)) { EE.SetActive(true); }
        if (O.activeSelf && Bloque.EsCalle(nuevaPosicion.Oeste.Oeste)) { OO.SetActive(true); }

        if ((N.activeSelf && Bloque.EsCalle(nuevaPosicion.Norte.Oeste)) || (O.activeSelf && Bloque.EsCalle(nuevaPosicion.Oeste.Norte)) ) { NO.SetActive(true); }
        if ((N.activeSelf && Bloque.EsCalle(nuevaPosicion.Norte.Este)) || (E.activeSelf && Bloque.EsCalle(nuevaPosicion.Este.Norte)) ) { NE.SetActive(true); }
        if ((S.activeSelf && Bloque.EsCalle(nuevaPosicion.Sur.Oeste)) || (O.activeSelf && Bloque.EsCalle(nuevaPosicion.Oeste.Sur))) { SO.SetActive(true); }
        if ((S.activeSelf && Bloque.EsCalle(nuevaPosicion.Sur.Este)) || (E.activeSelf && Bloque.EsCalle(nuevaPosicion.Este.Sur))) { SE.SetActive(true); }
    }
}

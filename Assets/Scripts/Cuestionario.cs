using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class Cuestionario : MonoBehaviour
{
    [SerializeField] private TextAsset _archivoPreguntas;
    [SerializeField] private TMP_Text _labelPregunta;
    [SerializeField] private Image _imagenContenedor;
    [SerializeField] private Image _imagen;
    [SerializeField] private Button _buttonOpcionA;
    [SerializeField] private Button _buttonOpcionB;
    [SerializeField] private Button _buttonOpcionC;
    [SerializeField] private GameObject _buttonCerrar;

    [SerializeField] private Color _colorNormal;
    [SerializeField] private Color _colorRespuestaCorrecta;
    [SerializeField] private Color _colorRespuestaIncorrecta;

    [SerializeField] private UnityEvent _onRespuestaCorrecta;
    [SerializeField] private UnityEvent _onRespuestaIncorrecta;

    [SerializeField] private List<Sprite> _imagenes;

    private List<Desafio> _preguntas;
    private Desafio _preguntaActual;

    public int RespuestasCorrectas { get; private set; }
    public int RespuestasIncorrectas { get; private set; }
    public int RespuestasTotales { get { return RespuestasCorrectas + RespuestasIncorrectas; } }
    public float Nota { get { return ((float)RespuestasCorrectas / RespuestasTotales) * 10; } }
    public bool RespuestaCorrecta { get; private set; } = false;

    public void Inicializar() 
    {
        _preguntas = new List<Desafio>();
        string[] filas = _archivoPreguntas.text.Split("\n");
        string[] columnas;
        foreach (string fila in filas)
        {
            columnas = fila.Split("\t");
            _preguntas.Add(new Desafio(columnas[0], columnas[1], columnas[2], columnas[3], columnas[4], columnas[5], columnas[6]));
        }
    }
    public void Reiniciar() 
    {
        RespuestasCorrectas = 0;
        RespuestasIncorrectas = 0;
    }
    public void SetPregunta(int indice)
    {
        RespuestaCorrecta = false;
        _buttonOpcionA.onClick.RemoveAllListeners();
        _buttonOpcionB.onClick.RemoveAllListeners();
        _buttonOpcionC.onClick.RemoveAllListeners();
     
        _buttonOpcionA.onClick.AddListener(() => { VerificarRespuesta("A"); });
        _buttonOpcionB.onClick.AddListener(() => { VerificarRespuesta("B"); });
        _buttonOpcionC.onClick.AddListener(() => { VerificarRespuesta("C"); });

        _preguntaActual = _preguntas[indice];
        _labelPregunta.text = _preguntaActual.Pregunta;
        _buttonOpcionA.GetComponentInChildren<TMP_Text>().text = _preguntaActual.OpcionA;
        _buttonOpcionB.GetComponentInChildren<TMP_Text>().text = _preguntaActual.OpcionB;
        _buttonOpcionC.GetComponentInChildren<TMP_Text>().text = _preguntaActual.OpcionC;

        _imagenContenedor.gameObject.SetActive(_preguntaActual.Imagen.Length > 1);
        if (_imagenContenedor.gameObject.activeSelf) { _imagen.sprite = _imagenes.Find(sprite => sprite.name.Trim().ToLower() == _preguntaActual.Imagen.Trim().ToLower()); }
        
        _buttonOpcionC.gameObject.SetActive(_preguntaActual.OpcionC.Length > 0);

        _buttonOpcionA.GetComponent<Image>().color = _colorNormal;
        _buttonOpcionB.GetComponent<Image>().color = _colorNormal;
        _buttonOpcionC.GetComponent<Image>().color = _colorNormal;

        print(_preguntaActual.Respuesta);
    }
    public void VerificarRespuesta(string opcion)
    {
        if (_preguntaActual.ValidarRespuesta(opcion)) 
        {
            _onRespuestaCorrecta?.Invoke();
            RespuestasCorrectas++;

            RespuestaCorrecta = true;
            if (opcion == "A") { _buttonOpcionA.GetComponent<Image>().color = _colorRespuestaCorrecta; }
            if (opcion == "B") { _buttonOpcionB.GetComponent<Image>().color = _colorRespuestaCorrecta; }
            if (opcion == "C") { _buttonOpcionC.GetComponent<Image>().color = _colorRespuestaCorrecta; }
        }
        else 
        {
            _onRespuestaIncorrecta?.Invoke();
            RespuestasIncorrectas++;

            if (opcion == "A") { _buttonOpcionA.GetComponent<Image>().color = _colorRespuestaIncorrecta; }
            if (opcion == "B") { _buttonOpcionB.GetComponent<Image>().color = _colorRespuestaIncorrecta; }
            if (opcion == "C") { _buttonOpcionC.GetComponent<Image>().color = _colorRespuestaIncorrecta; }

            if (_preguntaActual.Respuesta == "A") { _buttonOpcionA.GetComponent<Image>().color = _colorRespuestaCorrecta; }
            if (_preguntaActual.Respuesta == "B") { _buttonOpcionB.GetComponent<Image>().color = _colorRespuestaCorrecta; }
            if (_preguntaActual.Respuesta == "C") { _buttonOpcionC.GetComponent<Image>().color = _colorRespuestaCorrecta; }
        }

        _buttonOpcionA.onClick.RemoveAllListeners();
        _buttonOpcionB.onClick.RemoveAllListeners();
        _buttonOpcionC.onClick.RemoveAllListeners();

        _buttonCerrar.SetActive(true);
    }
    public void SetPreguntaAleatoria() { SetPregunta(UnityEngine.Random.Range(1, _preguntas.Count)); }

    public struct Desafio
    {
        public string Indice;
        public string Pregunta;
        public string OpcionA;
        public string OpcionB;
        public string OpcionC;
        public string Respuesta;
        public string Imagen;

        public Desafio(string indice, string pregunta, string opcionA, string opcionB, string opcionC, string respuesta, string imagen)
        {
            Indice = indice;
            Pregunta = pregunta;
            Imagen = imagen;
            OpcionA = opcionA;
            OpcionB = opcionB;
            OpcionC = opcionC;
            Respuesta = respuesta;
        }
        public bool ValidarRespuesta(string respuesta) { if (Respuesta == respuesta) { return true; } else { return false; } }
        public override string ToString() { return $"{Indice}: {Pregunta} = [{OpcionA}, {OpcionB}, {OpcionC}] -> {Respuesta} ({Imagen})"; }
    }
}

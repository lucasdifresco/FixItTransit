using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TextPopUpController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifespan;
    [SerializeField] private float _distance;
    [SerializeField] private bool _disapear = true;
    [SerializeField] private TMP_Text _component;
    [SerializeField] private Image _image;

    private float _timer = 0;
    private float _progress = 0;
    private Vector3 _initialPosition;
    private Vector3 _direction;
    private string _text = "";

    private void Awake()
    {
        if (_component == null) { _component = gameObject.GetComponent<TMP_Text>(); }
        if (_image == null) { _image = gameObject.GetComponent<Image>(); }
    }
    private void Start()
    {
        _component.text = _text;
        _initialPosition = transform.position;
        _direction = transform.position + (Vector3.up * _distance) - _initialPosition;
    }

    private void Update()
    {
        _timer += Time.deltaTime * _speed;
        if (_timer >= _lifespan) { Destroy(gameObject); }

        _progress = _timer / _lifespan;

        transform.position = _initialPosition + (_direction * _progress);

        if (!_disapear) { return; }
        if (_component != null) { _component.color = new Color(_component.color.r, _component.color.g, _component.color.b, 1 - _progress); }
        if (_image != null) { _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1 - _progress); }
    }

    public void SetText(string text) 
    {
        _text = text;
        if (_component != null) { _component.text = text; } 
    }
}

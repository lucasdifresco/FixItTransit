using UnityEngine;
using UnityEngine.Events;

public class MonoController : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAwake;
    [SerializeField] private UnityEvent OnStart;

    private void Awake()
    {
        OnAwake?.Invoke();
    }
    void Start()
    {
        OnStart?.Invoke();
    }
}

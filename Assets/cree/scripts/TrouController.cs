using UnityEngine;
using UnityEngine.Events;

public class TrouController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent trouAtteint;

    private void OnTriggerEnter(Collider other)
    {
        var balle = other.GetComponent<BalleController>();
        if (balle != null)
        {
            trouAtteint.Invoke();
        }
    }
}

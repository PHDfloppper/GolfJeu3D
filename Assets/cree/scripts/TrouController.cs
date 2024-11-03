using UnityEngine;
using UnityEngine.Events;

public class TrouController : MonoBehaviour
{
    private UnityEvent trouAtteint = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        var balle = other.GetComponent<BalleController>();
        if (balle != null)
        {
            trouAtteint.RemoveAllListeners();
            trouAtteint.AddListener(balle.ResetPartie);
            trouAtteint.Invoke();
        }
    }
}

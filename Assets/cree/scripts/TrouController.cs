using UnityEngine;
using UnityEngine.Events;

public class TrouController : MonoBehaviour
{
    private UnityEvent trouAtteint = new UnityEvent();

    /// <summary>
    /// quand la balle entre en collision avec le collider du trou, la parcour est fini.
    /// la unityevent appel donc Reset() de BalleController
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var balle = other.GetComponent<BalleController>();
        if (balle != null)
        {
            trouAtteint.RemoveAllListeners();
            trouAtteint.AddListener(balle.ResetPartie); //oblig� de faire �a car Trou est un prefab donc je ne peux pas lui assign� la balle dans l'inspecteur
            trouAtteint.Invoke();
        }
    }
}

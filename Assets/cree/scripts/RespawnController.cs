using UnityEngine;
using UnityEngine.Events;

public class RespawnController : MonoBehaviour
{
    //appel la fonction Respawn() de BalleControlleur
    [SerializeField]
    private UnityEvent outofBound;

    //si la balle entre dans le collider, la balle réapparait à son point de départ.
    //sert juste à détecter si la balle est sortie du parcour
    private void OnTriggerEnter(Collider other)
    {
        var balle = other.GetComponent<BalleController>();
        if (balle != null)
        {
            outofBound.Invoke();
        }
    }
}

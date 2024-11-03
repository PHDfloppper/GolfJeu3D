using UnityEngine;
using UnityEngine.Events;

public class RespawnController : MonoBehaviour
{

    [SerializeField]
    private UnityEvent outofBound;
    private void OnTriggerEnter(Collider other)
    {
        var balle = other.GetComponent<BalleController>();
        if (balle != null)
        {
            outofBound.Invoke();
        }
    }
}

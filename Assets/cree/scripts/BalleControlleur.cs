using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class BalleController : MonoBehaviour
{

    [SerializeField]
    private float vitesseRotation;
    private float rotation;

    private Vector2 deplacement;

    private Rigidbody balleRB;

    [SerializeField]
    private float forcePousse;

    private Vector3 directionInitiale;

    [SerializeField]
    private GameObject fleche;

    [SerializeField]
    public Slider jauge;

    [SerializeField]
    private float rapiditeJauge;
    private bool coupActif;
    private bool jaugeMonte;

    private Coroutine jaugeCoroutine;

    [SerializeField]
    private float coups;

    [SerializeField]
    private UnityEvent<float> afficherPoint;

    public static int longueurMap { private set; get; }

    private float currentTrou;
    [SerializeField]
    private TextMeshProUGUI trou1C;
    [SerializeField]
    private TextMeshProUGUI trou2C;
    [SerializeField]
    private TextMeshProUGUI trou3C;
    [SerializeField]
    private TextMeshProUGUI trou4C;

    private bool mapFini = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        afficherPoint.Invoke(coups);
        balleRB = gameObject.GetComponent<Rigidbody>();
        directionInitiale = transform.forward;
        longueurMap = 5;
        currentTrou = 0;
    }

    private void CoupJouer()
    {
        if(coups > 0) 
        {
            coups = coups - 1;
            afficherPoint.Invoke(coups);
        }
        else
        {
            ResetPartie();
        }
        afficherPoint.Invoke(coups);
    }

    public void ResetPartie() //je ne fait pas le respawn de la balle ici car unity se fache pis je suis tanné de chercher
    {
        currentTrou++;
        mapFini = true;
        coups = 20;
        longueurMap++;
        gameObject.transform.position = new Vector3(0, 1, -1);
    }

    public void Respawn()
    {
        gameObject.transform.position = new Vector3(0, 1, -1);
    }

    //code emprunté à Alex le prof (je crois, à vérifier. dans le doute, je dit que c'est emprunté)
    public void Rotater(InputAction.CallbackContext contexte)
    {
        rotation = contexte.action.ReadValue<float>();
    }

    public void Pousser(InputAction.CallbackContext contexte) 
    {
        if (contexte.performed)
        {
            //balleRB.AddForce(forcePousse * transform.forward, ForceMode.Impulse);
            if(!coupActif)
            {
                jaugeCoroutine = StartCoroutine(AjusterJaugeForce());
                coupActif = true;
            }
            else
            {
                StopCoroutine(jaugeCoroutine);
                balleRB.AddForce(forcePousse * transform.forward, ForceMode.Impulse);
                coupActif = false;
                CoupJouer();
            }
        }
    }

    private IEnumerator AjusterJaugeForce()
    {
        jaugeMonte = true;

        while (true) //je vis dangereusement
        {
            jauge.value = forcePousse;
            if (jaugeMonte) 
            {
                forcePousse += Time.deltaTime * rapiditeJauge;
                if(forcePousse >= jauge.maxValue)
                {
                    forcePousse = jauge.maxValue;
                    jaugeMonte= false;
                }
            }
            else
            {
                forcePousse -= Time.deltaTime * rapiditeJauge;
                if(forcePousse <= jauge.minValue)
                {
                    forcePousse = jauge.minValue;
                    jaugeMonte= true;
                }
            }
            yield return null; //empêche un minimum le jeu d'exploser à cause du while(true)
        }
    }


    /// <summary>
    /// PLUS UTILISÉ CAR FUCKIN INUTILE (I'M DUMB) À VOIR SI À GARDER OU ENLEVER
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Mur"))
        {
            // Forcer la direction apres la collision
            directionInitiale = Vector3.Reflect(balleRB.linearVelocity.normalized, collision.contacts[0].normal);
            balleRB.linearVelocity = directionInitiale * balleRB.linearVelocity.magnitude;
        }
    }

    void FixedUpdate()
    {
        // Rotation autour de l'axe des Y
        ////code emprunté à Alex le prof (je crois, à vérifier. dans le doute, je dit que c'est emprunté)
        balleRB.rotation = balleRB.rotation *
            Quaternion.AngleAxis(rotation * vitesseRotation * Time.deltaTime, Vector3.up);

        if (mapFini)
        {
            switch (currentTrou)
            {
                case 0:
                    break;
                case 1:
                    trou1C.SetText(coups.ToString());
                    break;
                case 2:
                    trou2C.SetText(coups.ToString());
                    break;
                case 3:
                    trou3C.SetText(coups.ToString());
                    break;
                case 4:
                    trou4C.SetText(coups.ToString());
                    break;
            }
            mapFini = false;
        }
    }
}

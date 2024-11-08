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

    //variables qui gère la rotation de la balle
    [SerializeField]
    private float vitesseRotation;
    private float rotation;

    //variable pour le rigidbody de la balle, est assigné dans le start
    private Rigidbody balleRB;

    //variable pour géré la force de pousse dans l'inspecteur
    [SerializeField]
    private float forcePousse;

    //varibale qui stock la jauge de puissance
    [SerializeField]
    public Slider jauge;

    //variable qui gère la rapidité de la jauge
    [SerializeField]
    private float rapiditeJauge;
    //variable qui détermine si il y a un coup actif ou non
    private bool coupActif;
    //variable qui détermince si la jauge va vers le haut ou le bas
    private bool jaugeMonte;

    //coroutine qui gère la jauge de puissance
    private Coroutine jaugeCoroutine;

    //variable qui stock le nombre de coup restant 
    [SerializeField]
    private float coups;

    //unityevent qui appel le HUD pour afficher le nombre de point restant
    [SerializeField]
    private UnityEvent<float> afficherPoint;

    //variable public get qui indique la longeur du parcour actuel
    public static int longueurMap { private set; get; }
    //variable qui indique le actuel
    private float currentTrou;

    //des TextMesh qui contiennent les éléments du HUD qui afficher les score des 4 premiers troues
    [SerializeField]
    private TextMeshProUGUI trou1C;
    [SerializeField]
    private TextMeshProUGUI trou2C;
    [SerializeField]
    private TextMeshProUGUI trou3C;
    [SerializeField]
    private TextMeshProUGUI trou4C;
    [SerializeField]
    private TextMeshProUGUI trou5C;
    [SerializeField]
    private TextMeshProUGUI trou6C;
    //affiche le score final
    [SerializeField]
    private TextMeshProUGUI scoreFinal;
    //stock le score final en attendant de le mettre dans scoreFinal
    private float scoreFinalCoups;
    //gameobject qui contient l'écran de fin
    [SerializeField]
    private GameObject ecranFin;

    //variable qui détermine si la balle a ateint le troue ou non, signifiant la fin du parcour actuel
    private bool mapFini = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        afficherPoint.Invoke(coups);
        balleRB = gameObject.GetComponent<Rigidbody>();
        longueurMap = 5;
        currentTrou = 0;
    }

    /// <summary>
    /// fonction qui sert à compter le nombre de coup joué par parcour par le joueur.
    /// si le compte atteint 0, le joueur passe au prochain parcour mais un score de 0.
    /// unityevent qui invoke la fonction AfficherCoupsRestant(float coups) de HUDController
    /// </summary>
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

    /// <summary>
    /// fonction qui sert à passer au prochain troue, donc génère une map plus longue et reset le compte de coup
    /// </summary>
    public void ResetPartie() //je ne fait pas le respawn de la balle ici car unity se fache pis je suis tanné de chercher
    {
        currentTrou++;
        mapFini = true;
        longueurMap++;
        gameObject.transform.position = new Vector3(0, 1, -1);
    }

    /// <summary>
    /// fonction sert à reset le position de la balle à 0 dans le même parcour. est appelé quand le joueur tombe hors du parcour.
    /// </summary>
    public void Respawn()
    {
        gameObject.transform.position = new Vector3(0, 1, -1);
    }
    /// <summary>
    /// code emprunté à Alex le prof (je crois, à vérifier. dans le doute, je dit que c'est emprunté)
    /// après vérification, c'est effectivement du code dans le jeu de dino
    /// fonction qui sert à rotationer la balle selon les input du joueur
    /// </summary>
    /// <param name="contexte"></param>
    public void Rotater(InputAction.CallbackContext contexte)
    {
        rotation = contexte.action.ReadValue<float>();
    }


    /// <summary>
    /// fonction qui sert à frapper la balle. le joueur appuie un première fois sur SPACE.
    /// le script "commence un coup" et appel la coroutine AjusterJaugeForce. 
    /// La jauge de puissance monte et descend jusqu'à ce que le joueur réappuie sur SPACE et met "fin au coup"
    /// quand le coup est fini, la fonction arrête la coroutine pour récupé la puissance de la jauge pour envoyer la force dans le rigidbody de la balle.
    /// la fonction appel ensuite CoupJouer() pour  mettre à jour le nombre de coup restant
    /// </summary>
    /// <param name="contexte"></param>
    public void Pousser(InputAction.CallbackContext contexte) 
    {
        if (contexte.performed)
        {
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

    /// <summary>
    /// coroutine de jauge de puissance
    /// utilise un while(true) parce que j'aime le danger (c'est juste que je veux que ça boucle à l'infini tant que le joueur à pas appuyé sur SPACE)
    /// le yield return empêche le while(true) de crash vue que ça attend la prochaine frame pis la coroutine arrête quand le joueur réappuie sur SPACE
    /// la boucle change la value de la jauge pour que le joueur aille un feedback visuel du choix de la force, et change aussi la valeur de la puissance elle-même
    /// </summary>
    /// <returns></returns>
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

    void FixedUpdate()
    {
        // Rotation autour de l'axe des Y
        ////code emprunté à Alex le prof (je crois, à vérifier. dans le doute, je dit que c'est emprunté) c'est emprunté
        balleRB.rotation = balleRB.rotation *
            Quaternion.AngleAxis(rotation * vitesseRotation * Time.deltaTime, Vector3.up);

        //case qui détermine dans quel parcour le joueur est, vue que les 4 premiers trou sont comptabilisés
        if (mapFini)
        {
            switch (currentTrou)
            {
                case 0:
                    break;
                case 1:
                    trou1C.SetText(coups.ToString());
                    scoreFinalCoups += (30 - coups);
                    break;
                case 2:
                    trou2C.SetText(coups.ToString());
                    scoreFinalCoups += (30 - coups);
                    break;
                case 3:
                    trou3C.SetText(coups.ToString());
                    scoreFinalCoups += (30 - coups);
                    break;
                case 4:
                    trou4C.SetText(coups.ToString());
                    scoreFinalCoups += (30 - coups);
                    break;
                case 5:
                    trou5C.SetText(coups.ToString());
                    scoreFinalCoups += (30 - coups);
                    break;
                case 6:
                    trou6C.SetText(coups.ToString());
                    scoreFinalCoups += (30 - coups);
                    ecranFin.SetActive(true);
                    scoreFinal.SetText(scoreFinalCoups.ToString());
                    break;
            }
            coups = 30;
            afficherPoint.Invoke(coups);
            mapFini = false;
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor.Animations;

public class HUDController : MonoBehaviour
{
    //gameobject qui contient tout le HUD affiché pendant le jeu
    [SerializeField]
    private GameObject inGameHUD;
    //gameObject qui contient le HUD quand le menu pause est activé
    [SerializeField]
    private GameObject pauseHUD;
    //gameobject qui contient le HUD du tutoriel
    [SerializeField]
    private GameObject tutoHUD;
    // TextMashPro qui contient le textfield qui affiche le nombre de coup restant du trou actuel
    [SerializeField]
    private TextMeshProUGUI nbCoupsRestant;
    //slider qui contient le silder qui gère le volume de la musique
    [SerializeField]
    public Slider volumeSlider;
    //audiosource qui contient la musique
    [SerializeField]
    private AudioSource musique;

    //Animator qui contient l'animator controller "MenuController"
    private Animator menuAnimator;

    //audiosource qui contient les sound effects du menu (quand on clique sur on bouton, ça fait un bruit)
    [SerializeField]
    private AudioSource menuSoundEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //inGameHUD.SetActive(true);
        //pauseHUD.SetActive(false);
        menuAnimator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// marche pas comme je voulais mais ça marche
    /// change le volume selon la valeur du slider dans le menu pause
    /// </summary>
    public void ChangerVolume()
    {
        musique.volume = volumeSlider.value;
    }

    /// <summary>
    /// affiche le nombre de coup restant pour le parcour actuel
    /// </summary>
    /// <param name="coups"></param>
    public void AfficherCoupsRestant(float coups)
    {
        //Debug.Log(coups);
        nbCoupsRestant.SetText($"{coups}");
    }

    /// <summary>
    /// fonction qui active la transition vers le jeu pour sortir du menu pause
    /// </summary>
    public void ActiverInGameHUD()
    {
        //code pris à alex
        Time.timeScale = 1.0f;
        menuAnimator.SetTrigger("offMenuTrig");

        //code pris à alex
        menuSoundEffect.Play();
    }

    /// <summary>
    /// fonction qui active la transition vers le menu pause
    /// </summary>
    public void ActiverPauseHUD()
    {
        menuSoundEffect.Play();
        menuAnimator.SetTrigger("onMenuTrig");
    }

    /// <summary>
    /// fonction qui fait apparaître le tutoriel
    /// </summary>
    public void ActiverTuto()
    {
        tutoHUD.SetActive(true);
    }

    /// <summary>
    /// fonction qui fait disparaitre le tutoriel
    /// </summary>
    public void DesactiverTuto()
    {
        tutoHUD.SetActive(false);
    }

    /// <summary>
    /// est déclanché lors d'un animation event, pour s'assurer que l'animation est joué avant de mettre le timeScale à 0.0f;
    /// l'animation event est dans l'animation "onMenu" à la dernière frame
    /// </summary>
    public void PauseEffet()
    {
        //code pris à alex
        Time.timeScale = 0.0f;
    }
}

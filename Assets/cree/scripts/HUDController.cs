using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor.Animations;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    private GameObject inGameHUD;
    [SerializeField]
    private GameObject pauseHUD;
    [SerializeField]
    private TextMeshProUGUI nbCoupsRestant;
    [SerializeField]
    public Slider volumeSlider;
    [SerializeField]
    private AudioSource musique;

    private Animator menuAnimator;

    [SerializeField]
    private AudioMixerGroup musiqueGroup;

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
    /// marche pas comme je voulais mais �a marche
    /// </summary>
    public void ChangerVolume()
    {
        musique.volume = volumeSlider.value;
        //musiqueGroup.audioMixer.Equals(volumeSlider.value);
    }

    public void AfficherCoupsRestant(float coups)
    {
        //Debug.Log(coups);
        nbCoupsRestant.SetText($"{coups}");
    }

    public void ActiverInGameHUD()
    {
        //code pris � alex
        Time.timeScale = 1.0f;
        menuAnimator.SetTrigger("offMenuTrig");

        //code pris � alex
        menuSoundEffect.Play();
    }

    public void ActiverPauseHUD()
    {
        menuSoundEffect.Play();
        menuAnimator.SetTrigger("onMenuTrig");
    }

    /// <summary>
    /// est d�clanch� lors d'un animation event, pour s'assurer que l'animation est jou� avant de mettre le timeScale � 0.0f;
    /// </summary>
    public void PauseEffet()
    {
        //code pris � alex
        Time.timeScale = 0.0f;
    }
}

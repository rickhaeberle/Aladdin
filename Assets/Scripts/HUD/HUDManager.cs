using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    private const string HUD_HEALTH_ANIM = "Health";

    public GameObject Health;
    public GameObject Lives;
    public GameObject Jewels;
    public GameObject Apples;

    private Image _healthImage;
    private Image _applesImage;

    private Animator _healthAnimator;

    private Text _livesText;
    private Text _jewelsText;
    private Text _applesText;

    private bool _blinkHealth;
    private bool _blinkApples;

    private void Awake() {
        GameManager.Instance.RegisterHUDManager(this);

        _healthImage = Health.GetComponent<Image>();
        _applesImage = Apples.GetComponent<Image>();

        _healthAnimator = Health.GetComponent<Animator>();

        _livesText = Lives.GetComponentInChildren<Text>();
        _jewelsText = Jewels.GetComponentInChildren<Text>();
        _applesText = Apples.GetComponentInChildren<Text>();

        _blinkHealth = false;
        _blinkApples = false;
    }

    private void Update() {
        if (_blinkHealth) {
            _healthImage.enabled = !_healthImage.enabled;
        } else {
            _healthImage.enabled = true;
        }

        if (_blinkApples) {
            _applesImage.enabled = !_applesImage.enabled;
        } else {
            _applesImage.enabled = true;
        }
    }

    public void UpdateHUD(int hp, int lives, int jewels, int apples) {
        _healthAnimator.SetInteger(HUD_HEALTH_ANIM, hp);

        _blinkHealth = hp <= 3;

        _livesText.text = lives.ToString();

        if (jewels > 0) {
            Jewels.SetActive(true);
            _jewelsText.text = jewels.ToString();
        } else {
            Jewels.SetActive(false);

        }

        if (apples > 0) {
            Apples.SetActive(true);
            _applesText.text = apples.ToString();
            _blinkApples = apples <= 5;

        } else {
            Apples.SetActive(false);
            _blinkApples = false;
        }


    }
}

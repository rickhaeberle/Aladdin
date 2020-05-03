using UnityEngine;

public class GameOverDirector : MonoBehaviour {

    public AudioClip LaughtSfx;

    public float TimeToReload = 5f;

    private float _timeToReloadTimer = 0;

    // Start is called before the first frame update
    void Start() {
        AudioSource.PlayClipAtPoint(LaughtSfx, transform.position, GameManager.Instance.Volume);

    }

    // Update is called once per frame
    void Update() {
        if (_timeToReloadTimer < TimeToReload) {
            _timeToReloadTimer += Time.deltaTime;
        } else {
            GameManager.Instance.Restart();
        }

    }
}

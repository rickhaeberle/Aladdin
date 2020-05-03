using UnityEngine;

public class ItemApple : MonoBehaviour {

    public AudioClip AppleCollectedSfx;
    public GameObject ItemTakenEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other) {

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.TakeApple();

        AudioSource.PlayClipAtPoint(AppleCollectedSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(ItemTakenEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }
}

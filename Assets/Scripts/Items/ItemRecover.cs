using UnityEngine;

public class ItemRecover : MonoBehaviour {

    public GameObject ItemTakenEffectPrefab;
    public AudioClip ItemTakenSfx;

    private void OnTriggerEnter2D(Collider2D other) {

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.RecoverHealth();

        AudioSource.PlayClipAtPoint(ItemTakenSfx, transform.position, GameManager.Instance.Volume);
        Instantiate(ItemTakenEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }
}

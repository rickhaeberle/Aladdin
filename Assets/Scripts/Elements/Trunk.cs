using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class Trunk : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.OnTrunkEnter(transform.gameObject);
    }
}

using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class Rope : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.SetRope(transform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        player.SetRope(null);
    }

}

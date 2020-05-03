using UnityEngine;

public class EnemySight : MonoBehaviour
{

    private Enemy _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            _owner.SetPlayer(player);

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            _owner.SetPlayer(null);

        }

    }

}

using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject Player;

    public AudioClip RespawnSfx;

    void Start()
    {
        Vector3? checkpoint = GameManager.Instance.GetCheckpoint();
        if (checkpoint != null)
        {
            Player.GetComponent<Player>().RespawnAtCheckpoint(checkpoint.Value);
            AudioSource.PlayClipAtPoint(RespawnSfx, transform.position, GameManager.Instance.Volume);

        }
    }
}

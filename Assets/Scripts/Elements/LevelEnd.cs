using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {

        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        GameManager.Instance.LevelCompleted();
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour {

    public Player Player;
    public Vector2 MinPosition;
    public Vector2 MaxPosition;
    public float VerticalSpeed;

    public float UpOffset = 1.1f;
    public float DownOffset = 0.8f;

    private void Update() {
        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = Player.transform.position.x;

        bool isPlayerOnGround = Player.IsGrounded();

        if (Player.IsLookingUp() && isPlayerOnGround) {
            if (Player.transform.position.y + UpOffset > newCameraPosition.y) {
                newCameraPosition.y += (Time.deltaTime * VerticalSpeed);
            }

        } else if (Player.IsLookingDown() && isPlayerOnGround) {
            if (Player.transform.position.y - DownOffset < newCameraPosition.y) {
                newCameraPosition.y -= (Time.deltaTime * VerticalSpeed);
            }

        } else {
            newCameraPosition.y = Player.transform.position.y;

        }

        if (newCameraPosition.x < MinPosition.x) {
            newCameraPosition.x = MinPosition.x;
        } else if (newCameraPosition.x > MaxPosition.x) {
            newCameraPosition.x = MaxPosition.x;
        }

        if (newCameraPosition.y < MinPosition.y) {
            newCameraPosition.y = MinPosition.y;
        } else if (newCameraPosition.y > MaxPosition.y) {
            newCameraPosition.y = MaxPosition.y;
        }

        transform.position = newCameraPosition;

    }
}

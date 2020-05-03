using UnityEngine;

public class Switcher : MonoBehaviour
{

    public GameObject[] Objects;

    private void SwitchStates()
    {
        if (Objects != null)
        {
            foreach (GameObject obj in Objects)
            {
                obj.SetActive(!obj.activeSelf);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        SwitchStates();
    }
}

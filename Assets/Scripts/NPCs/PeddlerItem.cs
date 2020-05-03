using UnityEngine;

public class PeddlerItem : MonoBehaviour
{

    public int Price;
    public EShopItem Item;

    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Hide()
    {
        _collider.enabled = false;
    }

    public void Show()
    {
        _collider.enabled = true;
    }

}

using System.Collections.Generic;
using UnityEngine;

public class Peddler : MonoBehaviour {
    private const string IS_COAT_OPEN_ANIM = "IsCoatOpen";
    private const string IS_SHOP_VISIBLE_ANIM = "IsShopVisible";

    public float TimeToShowShop = 3f;
    public List<PeddlerItem> Items;

    private float _timeToShowShopTimer = 0;

    private Player _player;

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();

    }

    private void Start() {
        HideItems();

    }

    private void Update() {
        if (_player != null) {
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            if (distance > 2.5f) {
                _animator.SetBool(IS_COAT_OPEN_ANIM, false);
                _animator.SetBool(IS_SHOP_VISIBLE_ANIM, false);
                _player = null;
            } else {
                if (_timeToShowShopTimer < TimeToShowShop) {
                    _timeToShowShopTimer += Time.deltaTime;
                } else {
                    _animator.SetBool(IS_SHOP_VISIBLE_ANIM, true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        _player = player;
        _timeToShowShopTimer = 0;

        _animator.SetBool(IS_COAT_OPEN_ANIM, true);
    }

    public void ShowItems() {
        foreach (PeddlerItem item in Items) {
            item.Show();
        }
    }

    public void HideItems() {
        foreach (PeddlerItem item in Items) {
            item.Hide();
        }
    }

}

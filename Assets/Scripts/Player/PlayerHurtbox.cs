using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    private Player _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
    }

    public void TakeDamage()
    {
        _owner.TakeDamage();
    }

    public void Stolen() {
        _owner.Stolen();
    }
}

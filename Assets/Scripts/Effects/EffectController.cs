using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {
    public void OnAnimationEnded() {
        Destroy(gameObject);
    }
}

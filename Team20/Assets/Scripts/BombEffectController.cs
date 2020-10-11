using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("selfDestroy", .5f);
    }

    void selfDestroy()
    {
        Destroy(gameObject);
    }
    
}

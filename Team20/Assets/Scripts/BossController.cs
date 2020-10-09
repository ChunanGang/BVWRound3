using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // ref
    public SpriteRenderer bossSprite;

    // game logic
    public float maxHP;
    private float curHP;

    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getHP()
    {
        return curHP;
    }

    // do damage to boss
    public void doDamage(float dmg)
    {
        curHP -= dmg;
        StartCoroutine(damaged());
    }

    // change the color to red for a short period (used when dmged)
    IEnumerator damaged()
    {
        bossSprite.color = Color.red;
        yield return new WaitForSeconds(.05f);
        bossSprite.color = Color.white;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // ref
    private BossController boss;
    private PlayerController player1; 
    private PlayerController player2;
    public Sprite[] BulletSprites;

    // game logic
    private int bulletType = -1; // we will have diff typesof bullet. -1 means no type set yet; boss have type 1

    // ----------- BULLETS ----------
    public float[] bulletsDMG;
    // bullet 0: normal bullet
    public float bullet0Speed;

    private void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<BossController>();
        player1 = GameObject.Find("player1").GetComponent<PlayerController>();
        player2 = GameObject.Find("player2").GetComponent<PlayerController>();
    }
    void Update()
    {
        // move bullet when tyope is set
        if (bulletType != -1)
            move();
    }

    // type will be set in playerController's attack fucntion, whne bullet is created
    public void setType(int type)
    {
        bulletType = type;
        // hcnage the sprite according to type
        GetComponent<SpriteRenderer>().sprite = BulletSprites[type];
    }

    void move()
    {
        if (bulletType == 0)
            transform.Translate(Vector3.right * bullet0Speed * Time.deltaTime);
        else if(bulletType == 1){
            transform.Translate(-1 * Vector3.right * bullet0Speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //print(col.gameObject.tag);
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Boss"))
        {
            boss.doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("player1"))
        {
            player1.doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("player2"))
        {
            player2.doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
    }
}

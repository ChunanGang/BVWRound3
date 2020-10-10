using System.Collections;
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
    // bullet 0: normal bullet
    // bullet 1: Boss's normal bullet
    public float[] bulletsDMG;
    public float[] bulletsSpeed;

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
            transform.Translate(Vector3.right * bulletsSpeed[0] * Time.deltaTime);
        else if(bulletType == 1)
            transform.Translate(-1 * Vector3.right * bulletsSpeed[1] * Time.deltaTime);
        else if (bulletType == 2)
            transform.Translate( Vector3.right * bulletsSpeed[2] * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //print(col.gameObject.tag);
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Boss") && bulletShooter()==0)
        {
            boss.doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("player1") && bulletShooter() == 1)
        {
            player1.heal(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("player2") && bulletShooter() == 1)
        {
            player2.doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
    }

    // return the shooter of this bullet (0 for players, 1 for boss) 
    int bulletShooter()
    {
        // boss
        if(bulletType == 1)
        {
            return 1;
        }
        
        else            
            return 0;
    }
}

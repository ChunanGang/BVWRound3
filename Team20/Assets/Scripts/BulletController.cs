using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // ref
    public GameObject bombEffect;
    public Sprite[] BulletSprites;

    // game logic
    private int bulletType = -1; // we will have diff typesof bullet. -1 means no type set yet; boss have type 1
    private float shootAngle = 0f;
    // ----------- BULLETS ----------
    // bullet 0: Boss's normal bullet
    // bullet 1: player1 normal bullet
    // bullet 2: player1 strong bullet
    // bullet 3: player2 normal bullet
    // bullet 4: player2 strong bullet
    public float[] bulletsDMG;
    public float[] bulletsSpeed;

    private void Start()
    {
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
        /*
        if(!isBossBool){
            if (bulletType == 0)
                transform.Translate(Vector3.right * bulletsSpeed[0] * Time.deltaTime);
            else if(bulletType == 1)
                transform.Translate(-1 * Vector3.right * bulletsSpeed[1] * Time.deltaTime);
            else if (bulletType == 2)
                transform.Translate( Vector3.right * bulletsSpeed[2] * Time.deltaTime);
        }
        //game logic of the boss bullets
        else{
            //Debug.Log("shootAngle" + shootAngle);
            if((0 <= shootAngle) && (shootAngle <= Mathf.PI/2)){
                Vector3 moveVector = new Vector3( - Mathf.Cos(shootAngle),  Mathf.Sin(shootAngle),0);
                transform.Translate(moveVector * bulletsSpeed[1] * Time.deltaTime);
            }
            //Creates the reflection of the bullets
            else{
                Debug.Log("I am here");
                shootAngle = - shootAngle;
                Debug.Log("reverted angle = " + shootAngle);
                Vector3 moveVector = new Vector3( - Mathf.Cos(shootAngle), - Mathf.Sin(shootAngle),0);
                transform.Translate(moveVector * bulletsSpeed[1] * Time.deltaTime);
            }
        }  */

        // players' bullet move right
        if (bulletType == 1 || bulletType == 2 || bulletType == 3 || bulletType == 4)
            transform.Translate(Vector3.right * bulletsSpeed[bulletType] * Time.deltaTime);
        // boss's bullet move right
        if (bulletType == 0)
            transform.Translate(Vector3.left * bulletsSpeed[bulletType] * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //print(col.gameObject.tag);
        if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        // players' bullets hit boss
        else if (col.gameObject.CompareTag("Boss") && bulletShooter()==0)
        {
            col.gameObject.GetComponent<BossController>().doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
            // if is strong bullet
            if(bulletType == 2 || bulletType ==4)
            {
                // play bomb effect
                Instantiate(bombEffect, transform.position, transform.rotation);
            }

        }
        // players' bullets hit Minions
        else if (col.gameObject.CompareTag("Minion") && bulletShooter() == 0)
        {
            col.gameObject.GetComponent<MinionsController>().doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
            // if is strong bullet
            if (bulletType == 2 || bulletType == 4)
            {
                // play bomb effect
                Instantiate(bombEffect, transform.position, transform.rotation);
            }

        }
        // enemies's bullets hit playetrs
        else if (col.gameObject.CompareTag("player2") && bulletShooter() == 1)
        {
            col.gameObject.GetComponent<PlayerController>().heal(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("player1") && bulletShooter() == 1)
        {
            col.gameObject.GetComponent<PlayerController>().doDamage(bulletsDMG[bulletType]);
            Destroy(gameObject);
        }
    }

    // return the shooter of this bullet (0 for players, 1 for boss) 
    int bulletShooter()
    {
        // boss
        if(bulletType == 0)
        {
            return 1;
        }
        
        else            
            return 0;
    }

    public void setAngle(float curAngle){
        shootAngle = curAngle;
    }
}

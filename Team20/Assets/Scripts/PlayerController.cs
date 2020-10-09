using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ref
    public Rigidbody2D playerRB;
    public GameObject bulletPrefab;
    public SpriteRenderer playerSprite;

    // Game logic
    private float screenTopX = 4.4f, screenBotX = -4.4f; // boudary of the screnn
    public float moveSpeed;
    private int currentBulletType = 0;
    public float maxHP;
    private float curHP;

    // Start is called before the first frame update
    void Start()
    { 
    }
   
    // this function will be called in gameManager.cs
    public void move(float dir)
    {
        // only move within boudary
        if ((transform.position.y <= screenTopX || dir <0)  && (transform.position.y >= screenBotX || dir > 0))
            playerRB.velocity = Vector3.up * dir * moveSpeed;
    }

    // this function will be called in gameManager.cs
    public void stop()
    {
        playerRB.velocity = Vector3.zero;
    }

    // this function will be called in gameManager.cs
    public void attack()
    {
        // gen bullet at player's location
        Vector3 pos = transform.position + new Vector3(0.5f,0,0);
        Quaternion rotation = transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
        bullet.GetComponent<BulletController>().setType(currentBulletType);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //print(col.gameObject.tag);
        // stop the movement when hit hte wall
        if (col.gameObject.CompareTag("Wall"))
        {
            stop();
        }
    }

    // do damage to player
    public void doDamage(float dmg)
    {
        curHP -= dmg;
        StartCoroutine(damaged());
    }

    // change the color to red for a short period (used when dmged)
    IEnumerator damaged()
    {
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(.05f);
        playerSprite.color = Color.white;
    }

}

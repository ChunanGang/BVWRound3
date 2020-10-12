using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ref
    public Rigidbody2D playerRB;
    public GameObject bulletPrefab;
    public Manager manager;
    public SpriteRenderer playerSprite;

    // Game logic
    public int playerID;
    public float hpDecrSpeed; // lose this much hp each sec
    private float screenTopY = 4.2f, screenBotY = -4.2f; // boudary of the screnn
    private float screenLeftX = -8.3f, screenRightX = 2.5f; // boudary of the screnn
    public float moveSpeed;
    public float attackInterval;
    private bool canAttack = true; // only can attack after some interval
    private int currentBulletType;
    private int normalBulletType;
    public float maxHP;
    private float curHP;
    private int bulletLeft; // how many bullet lefy (-1 means infinite)
    private Vector3 initPos;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    { 
        curHP = maxHP;
        initPos = transform.position;
        if(playerID == 1)
            currentBulletType = 1;
        else
            currentBulletType = 3;
        normalBulletType = currentBulletType;
        bulletLeft = -1;
    }
    private void LateUpdate()
    {
        if(manager.gameStarted)
            curHP -= Time.deltaTime * hpDecrSpeed;
        // check if dead
        if(curHP <= 0 && manager.gameStarted && !dead)
        {
            print("disavtive player " + playerID);
            dead = true;
            manager.setPlayerDead(playerID);
            gameObject.SetActive(false);
        }

    }

    // this function will be called in gameManager.cs
    public void move(int dir)
    {
        // move up (1) / down (-1)
        if (dir == 1 || dir == -1)
        {
            // only move within boudary
            if ((transform.position.y <= screenTopY || dir < 0) && (transform.position.y >= screenBotY || dir > 0))
                playerRB.velocity = Vector3.up * dir * moveSpeed;
        }
        // move left (-2) / right (2)
        else
        {
            // only move within boudary
            if ((transform.position.x <= screenRightX || dir < 0) && (transform.position.x >= screenLeftX || dir > 0))
                playerRB.velocity = Vector3.right * dir/2 * moveSpeed;
        }
    }

    public void resetStatus()
    {
        //print("reste player " + playerID);
        gameObject.SetActive(true);
        curHP = maxHP;
        transform.position = initPos;
        currentBulletType = normalBulletType;
        playerSprite.color = Color.white;
        bulletLeft = -1;
        dead = false;
    }

    // this function will be called in gameManager.cs
    public void stop()
    {
        playerRB.velocity = Vector3.zero;
    }

    // this function will be called in gameManager.cs
    public void attack()
    {
        if (!canAttack)
            return; 
        // check how many bullets left
        if (bulletLeft == 0) {
            // stwitch back to back attack if no bullet left
            bulletLeft = -1;
            currentBulletType = normalBulletType;
        }
        if(bulletLeft > 0)
            bulletLeft--;
        // gen bullet at player's location
        Vector3 pos = transform.position + new Vector3(0.5f,0,0);
        Quaternion rotation = transform.rotation;
        //attack audio
        //Debug.Log("bullet type" + currentBulletType);
        if(playerID == 1){
            //Debug.Log("Here! inside player1");
            if(currentBulletType == 1){
                audioController.AC.PlayPlayer1("player1Att");
            }
            else if(currentBulletType == 2){
                audioController.AC.PlayPlayer1("player1Ult");
            }
        }
        else if(playerID == 2)
        {
            if(currentBulletType == 3){
                audioController.AC.PlayPlayer2("player2Collect");
            }
            else if(currentBulletType == 4){
                audioController.AC.PlayPlayer2("player2Ult");
            }
        }
        GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
        bullet.GetComponent<BulletController>().setType(currentBulletType);
        // set attack cd
        canAttack = false;
        Invoke("setCanAttack", attackInterval);
    }

    void setCanAttack()
    {
        canAttack = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //print(col.gameObject.tag);
        // stop the movement when hit hte wall
        if (col.gameObject.CompareTag("Boudary"))
        {
            stop();
        }
    }

    public void heal(float healAmount){
        float updatedHPTemp = curHP + healAmount;
        //Debug.Log(updatedHPTemp);
        if(updatedHPTemp > maxHP){
            curHP = maxHP;
        }
        else{
            curHP = updatedHPTemp;
        }
    }

    // change the curBullet type of the player (called when hit by an item) 
    public void changeBullet(int bulletAmount)
    {
        // alrady in strong bullet, add bullet amount
        if (currentBulletType == 2 || currentBulletType == 4)
            bulletLeft += bulletAmount;
        // otherwise change type
        else
        {
            currentBulletType ++ ; // 1 become 2; 3 become 4
            bulletLeft = bulletAmount;
        }
    }

    // do damage to player
    public void doDamage(float dmg)
    {
        if(curHP > 0){
            curHP -= dmg;
        }
        StartCoroutine(damaged());
    }


    // change the color to red for a short period (used when dmged)
    IEnumerator damaged()
    {
        if(gameObject.tag == "player2"){
            audioController.AC.PlayPlayer2("characterAttacked");
        }
        else if(gameObject.tag == "player1"){
            audioController.AC.PlayPlayer1("characterAttacked");
        }
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(.05f);
        playerSprite.color = Color.white;
    }

    public float getHP(){
        return curHP;
    }

}

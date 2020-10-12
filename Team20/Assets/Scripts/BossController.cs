using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // ref
    public SpriteRenderer bossSprite;
    public GameObject bulletPrefab;
    public Manager manager;
    public Transform player1;

    // game logic
    public float maxHP;
    private float curHP;
    public float stage2HP;
    private int currentBulletType = 0;
    public float moveSpeed = 0.5f;

    private bool deathAudioPlayed = false; 
    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
        StartCoroutine(fireAndMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.gameStarted){
            if(curHP <= 0){
                if(!deathAudioPlayed){
                    audioController.AC.PlayBoss("bossDie");
                }
            }
        }
    }
    public void resetStatus()
    {
        curHP = maxHP;
        bossSprite.color = Color.white;
        StartCoroutine(fireAndMove());
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

    IEnumerator fireAndMove()
    {
        //this is the indicator of the move direction, if 1 up, -1 down
        int moveDirection = 1; 
        while (curHP > 0)
        {
            if (manager.gameStarted)
            {
                // ======== movement here =========== //
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.up * moveDirection * moveSpeed;
                // ======== attack here ========= //
                int attackType = Random.Range(0,5);
                //this  is the threshold to do flower attack and aiming attack
                //boss does more flower attack when the health is low
                int threshold = 0;
                float healthRatio = curHP / maxHP;
                if((healthRatio < 0.6f) && (healthRatio > 0.3f)) {
                    //half health
                    threshold = 1;
                }
                else if(healthRatio <= 0.3f){
                    //in danger
                    threshold = 2;
                }
                else{
                    //full health
                    threshold = 0;
                }
                
                if (attackType <= threshold)
                {
                    // --- flower bullets attack --- //
                    float rotateAngle = 8;
                    // each wave (separate by time)
                    for (int i = 0; i < 15; i++)
                    {
                        // eahc line (saperate by angle)
                        for (int j = 0; j < 6; j++)
                        {
                            float curAngle = i * rotateAngle + j * 60;
                            Vector3 pos = transform.position;
                            Quaternion rotation = Quaternion.Euler(0, 0, curAngle);
                            GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
                            bullet.GetComponent<BulletController>().setType(currentBulletType);
                        }

                        yield return new WaitForSeconds(.1f);
                    }
                    // ---- flower attack done --- //
                }
                else 
                {
                    // --- animing attack here --- //
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                    // anim at player 1 (since he got dmg fro mbullet)
                    bullet.transform.right = transform.position - player1.position;
                    bullet.GetComponent<BulletController>().setType(currentBulletType);
                    // --- animing attack done --- //
                }
            }
            moveDirection = moveDirection * (-1);
            yield return new WaitForSeconds(.8f);
        }

        /*
    	//Direction constant, 1 up, -1 down
    	int curDirection = 1;
        //How many iterations in one flower
        int iterationCtr = 0;
        int maxIteration = 7;
        //number of bullets per iteration
        int numBullet = 8; 
    	while(curHP > 0){
            if (manager.gameStarted)
            {
                // if (curDirection == 1)
                // {
                //     transform.position = transform.position + new Vector3(0, 1, 0);
                // }
                // else
                // {
                //     transform.position = transform.position + new Vector3(0, -1, 0);
                // }
                //Stage one attack of the boss
                if(curHP > stage2HP){
                    //gen bullet at boss's location
                    Vector3 pos = transform.position - new Vector3(3f, 0, 0);
                    Quaternion rotation = transform.rotation;
                    float angleConstant = Mathf.PI/(4*(maxIteration + numBullet));
                    float curAngle = iterationCtr * angleConstant;
                    //8 bullets per iteration
                    for(int i = 0; i < numBullet; i++){
                        GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
                        bullet.GetComponent<BulletController>().setType(currentBulletType);
                        bullet.GetComponent<BulletController>().isBoss();
                        bullet.GetComponent<BulletController>().setAngle(curAngle);
                        //The reflection bullet over x axis
                        GameObject bulletReflection = Instantiate(bulletPrefab, pos, rotation);
                        bulletReflection.GetComponent<BulletController>().setType(currentBulletType);
                        bulletReflection.GetComponent<BulletController>().isBoss();
                        bulletReflection.GetComponent<BulletController>().setAngle(-curAngle);
                        curAngle = curAngle + angleConstant;
                    }
                    audioController.AC.PlayBoss("enemyBossBullet");
                }
                //Stage two attack of the boss; aim at the Players!
                else{
                    //gen bullet at boss's location
                    Vector3 pos = transform.position - new Vector3(3f, 0, 0);
                    Quaternion rotation = transform.rotation;
                    float curAngle = 0;
                    for(int i = 0; i < numBullet; i++){
                        GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
                        bullet.GetComponent<BulletController>().setType(currentBulletType);
                        bullet.GetComponent<BulletController>().isBoss();
                        bullet.GetComponent<BulletController>().setAngle(curAngle);
                    }
                    audioController.AC.PlayBoss("enemyBossBullet");
                }
                curDirection = curDirection * (-1);
            }
            if(iterationCtr < maxIteration){
                iterationCtr ++;
            }
            else{
                iterationCtr = 0;
            }
            yield return new WaitForSeconds(.8f);
        }*/




    }


}

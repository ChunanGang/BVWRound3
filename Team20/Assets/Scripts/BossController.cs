using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // ref
    public SpriteRenderer bossSprite;
    public GameObject bulletPrefab;
    public Manager manager;

    // game logic
    public float maxHP;
    private float curHP;
    public float stage2HP;
    private int currentBulletType = 1;

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

    IEnumerator fireAndMove(){
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
        }
    }


}

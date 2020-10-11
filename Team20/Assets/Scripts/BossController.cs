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
    	while(curHP > 0){
            if (manager.gameStarted)
            {
                if (curDirection == 1)
                {
                    transform.position = transform.position + new Vector3(0, 1, 0);
                }
                else
                {
                    transform.position = transform.position + new Vector3(0, -1, 0);
                }
                //gen bullet at boss's location
                Vector3 pos = transform.position - new Vector3(3f, 0, 0);
                Quaternion rotation = transform.rotation;
                GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
                audioController.AC.PlayBoss("enemyBossBullet");
                bullet.GetComponent<BulletController>().setType(currentBulletType);
                curDirection = curDirection * (-1);
            }
            yield return new WaitForSeconds(.8f);
        }
    }


}

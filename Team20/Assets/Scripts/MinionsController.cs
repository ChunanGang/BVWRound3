using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsController : MonoBehaviour
{
    // ref
    public SpriteRenderer bossSprite;
    public GameObject bulletPrefab;
    public CanvasController canvas; 
    public Manager manager;

    // game logic
    public bool isMinionBoss;
    public float maxHP;
    private float curHP;
    private int currentBulletType = 0;
    private Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        curHP = maxHP;
        StartCoroutine(fireAndMove());
    }
    public void resetStatus()
    {
        //print("reste player " + playerID);
        transform.position = initPos;
        gameObject.SetActive(true);
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
        // check if dead
        if(curHP <= 0)
        {
            if (isMinionBoss)
            {
                canvas.moveToStageThree();
                gameObject.SetActive(false);
            }
            else
                gameObject.SetActive(false);
        }

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
        int curDirection = 1;
        while (curHP > 0)
        {
            if (manager.gameStarted)
            {
                // minion boss does not move
                if (!isMinionBoss)
                {
                    if (curDirection == 1)
                    {
                        transform.position = transform.position + new Vector3(0, 1, 0);
                    }
                    else
                    {
                        transform.position = transform.position + new Vector3(0, -1, 0);
                    }
                }
                //gen bullet at boss's location
                Vector3 pos = transform.position ;
                Quaternion rotation = transform.rotation;
                GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
                bullet.GetComponent<BulletController>().setType(currentBulletType);
                curDirection = curDirection * (-1);
            }
            yield return new WaitForSeconds(.8f);
        }
    }
}

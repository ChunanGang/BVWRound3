using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // ref
    public SpriteRenderer bossSprite;
    public GameObject bulletPrefab;

    // game logic
    public float maxHP;
    private float curHP;
    //Will change here later
    private int currentBulletType = 1;

    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
        StartCoroutine(fireAndMove());
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

    IEnumerator fireAndMove(){
    	//Direction constant, 1 up, -1 down
    	int curDirection = 1;
    	while(curHP > 0){
        	if(curDirection == 1){
        		transform.position = transform.position + new Vector3(0,1,0);
        	}
        	else{
        		transform.position = transform.position + new Vector3(0,-1,0);
        	}
        	//gen bullet at boss's location
        	Vector3 pos = transform.position - new Vector3(3f,0,0);
        	Quaternion rotation = transform.rotation;
        	yield return new WaitForSeconds(.8f);
        	GameObject bullet = Instantiate(bulletPrefab, pos, rotation);
        	bullet.GetComponent<BulletController>().setType(currentBulletType);
        	curDirection = curDirection * (-1);
        }
    }


}

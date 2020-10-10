using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
	//ref
	private PlayerController player1; 
    private PlayerController player2;
    public Sprite[] ItemSprites;

    // game logic
    private int itemType = -1; // we will have diff typesof bullet. -1 means no type set yet; boss have type 1

    // ----------- itemHealEffect ----------
    // item 0: heal
    public float item0Heal;
    public float item0Speed;
    // item 1: differny weapon
    public float item1Speed;
    public float item1BulletAmount; // how many time can the player use (attacks)


    // Start is called before the first frame update
    void Start()
    {
    	player1 = GameObject.Find("player1").GetComponent<PlayerController>();
        player2 = GameObject.Find("player2").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // move bullet when tyope is set
        if (itemType != -1)
            move();
    }

    void move()
    {
        if (itemType == 0){
            transform.Translate(-1 * Vector3.right * item0Speed * Time.deltaTime);
        }
        if (itemType == 1)
        {
            transform.Translate(-1 * Vector3.right * item1Speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("player1"))
        {
            //Debug.Log("Player1 object detected, object type: " + itemType);
            // heal
            if(itemType == 0){
            	player1.heal(item0Heal);
            }
            else if (itemType == 1)
            {
                player1.changeBullet(2, 5);
            }
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("player2"))
        {
            //Debug.Log("Player2 item detected, object type: " + itemType);
            // heal
            if(itemType == 0){
            	player2.heal(item0Heal);
            }
            else if (itemType == 1)
            {
                player2.changeBullet(2, 5);
            }
            Destroy(gameObject);
        }
    }

    // type will be set in playerController's attack fucntion, when item is created
    public void setType(int type)
    {
        itemType = type;
        // hcnage the sprite according to type
        GetComponent<SpriteRenderer>().sprite = ItemSprites[type];
    }

}

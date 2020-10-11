using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
	// ref
    public SpriteRenderer itemGeneratorSprite;
    public GameObject itemPrefab;
    public Manager manager;

    // game logic
    public int itemAmount; //  how many types of items
    public float genItemInterval;
    private int currentItemType = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(generateItem());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator generateItem(){
    	//Direction constant, 1 up, -1 down
    	int curDirection = -1;
    	while(true){
            // only gen when game started
            if (manager.gameStarted)
            {
                if (curDirection == 1)
                {
                    transform.position = transform.position + new Vector3(0, 2.5f, 0);
                }
                else
                {
                    transform.position = transform.position + new Vector3(0, -2.5f, 0);
                }
                //gen item at the correct location
                //Vector3 pos = transform.position - (curDirection * new Vector3(3f,0,0));
                Quaternion rotation = transform.rotation;
                GameObject item = Instantiate(itemPrefab, transform.position, rotation);
                item.GetComponent<ItemController>().setType(currentItemType);
                curDirection = curDirection * (-1);

                // randomly decide the next item type
                currentItemType = Random.Range(0, itemAmount);
            }

            yield return new WaitForSeconds(genItemInterval);
        }
    }
}

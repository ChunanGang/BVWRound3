using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{

    public BossController boss;
    public Image bossHPTop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateHP();
    }

    // update everyone 's HP here (boss, players)
    void updateHP()
    {
        bossHPTop.fillAmount = boss.getHP() / boss.maxHP;
    }
}

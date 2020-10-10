using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{

    public BossController boss;
    public Manager manager;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI gamePausedText;
    public TextMeshProUGUI player1ReadyText;
    public TextMeshProUGUI player2ReadyText;
    public Image bossHPTop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateHP();
        updateReady();
    }

    // update everyone 's HP here (boss, players)
    void updateHP()
    {
        bossHPTop.fillAmount = boss.getHP() / boss.maxHP;
    }
    void updateReady()
    {
        if (manager.player1Ready)
            player1ReadyText.SetText("Ready");
        else
            player1ReadyText.SetText("Press A");

        if (manager.player2Ready)
            player2ReadyText.SetText("Ready");
        else
            player2ReadyText.SetText("Press A");

        if (!manager.gameStarted)
            gamePausedText.SetText("Game Paused");
        else
            gamePausedText.SetText("");
    }

    public void setGameInfo(string info)
    {
        infoText.SetText(info);
    } 
}

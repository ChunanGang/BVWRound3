using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{

    public BossController boss;
    public PlayerController player1; 
    public PlayerController player2;
    public Manager manager;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI gamePausedText;
    public TextMeshProUGUI player1ReadyText;
    public TextMeshProUGUI player2ReadyText;
    public Image bossHPTop;
    public Image player1HPTop; 
    public Image player2HPTop;

    // Start is called before the first frame update
    void Start()
    {
        audioController.AC.PlayBgm("bgm");
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
        player1HPTop.fillAmount = player1.getHP() / player1.maxHP;
        player2HPTop.fillAmount = player2.getHP() / player2.maxHP; 
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

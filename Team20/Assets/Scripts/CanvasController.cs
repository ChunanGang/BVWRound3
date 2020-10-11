using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject gameStageCover;
    private GameObject[] gameUI;

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
    // video
    private bool playingVideo = false;
    public VideoPlayer startAnim;
    public float videoLength;

    // fading effect
    public Image fadeImage;
    private float maxAlpha = 255;
    private float fadeImageA;
    public float fadeTime;
    private IEnumerator lastFadingRoutine;

    // audio
    private float initVolume;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("here");
        //audioController.AC.PlayPlayer1("player1Att");
        audioController.AC.PlayBgm("bgm");
        initVolume = GetComponent<AudioSource>().volume;

        // disable game ui first
        gameUI = GameObject.FindGameObjectsWithTag("GameUI");
        foreach (GameObject obj in gameUI)
        {
            obj.SetActive(false);
        }
        startAnim.url = System.IO.Path.Combine(Application.streamingAssetsPath, "anim.mp4");
        startMenu.SetActive(true);
        gameStageCover.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        updateHP();
        updateReady();
        // from menu to vedio
        if (Input.GetKeyDown(KeyCode.Space) && manager.gameStage == 0)
        {
            lastFadingRoutine = fadeToStageOne();
            StartCoroutine(lastFadingRoutine);
        }
        // skip video
        if(Input.GetKeyDown(KeyCode.S) && playingVideo)
        {
            playingVideo = false;
            StartCoroutine(fadeToStageTwo());
        }
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

    private IEnumerator fadeToStageOne()
    {
        // fade in
        fadeImage.gameObject.SetActive(true);
        fadeImageA = 0;
        while (fadeImageA < maxAlpha)
        {
            fadeImage.color = new Color32(0, 0, 0, (byte)fadeImageA);
            fadeImageA = fadeImageA + maxAlpha / 30f;
            yield return new WaitForSeconds(fadeTime / 30);
        }
        // stop the audio
        GetComponent<AudioSource>().volume = 0;
        // remove start menu and play video
        startMenu.SetActive(false);
        StartCoroutine(playAnimVideo());
        manager.gameStage = 1;

        // fade out
        while (fadeImageA > 0)
        {
            fadeImage.color = new Color32(0, 0, 0, (byte)fadeImageA);
            fadeImageA -= maxAlpha / 30f;
            yield return new WaitForSeconds(fadeTime / 30);
        }
        fadeImage.gameObject.SetActive(false);

    }

    private IEnumerator playAnimVideo()
    {
        playingVideo = true;
        startAnim.Play();
        yield return new WaitForSeconds(videoLength);
        // if the video still active, stop it and move to game stage
        if (playingVideo)
        {
            playingVideo = false;
            StartCoroutine(fadeToStageTwo());
        }
    }

    private IEnumerator fadeToStageTwo()
    {
        StopCoroutine(lastFadingRoutine);
        // fade in
        fadeImage.gameObject.SetActive(true);
        fadeImageA = 0;
        while (fadeImageA < maxAlpha)
        {
            fadeImage.color = new Color32(0, 0, 0, (byte)fadeImageA);
            fadeImageA = fadeImageA + maxAlpha / 30f;
            yield return new WaitForSeconds(fadeTime / 30);
        }
        // remove the video and enable the game UI
        startAnim.Stop();
        startAnim.gameObject.SetActive(false);
        gameStageCover.SetActive(false);
        // enable game ui 
        foreach (GameObject obj in gameUI)
        {
            obj.SetActive(true);
        }
        manager.gameStage = 2;
        // resume bgm
        GetComponent<AudioSource>().volume = initVolume;
        // fade out
        while (fadeImageA > 0)
        {
            fadeImage.color = new Color32(0, 0, 0, (byte)fadeImageA);
            fadeImageA -= maxAlpha / 30f;
            yield return new WaitForSeconds(fadeTime / 30);
        }
        fadeImage.gameObject.SetActive(false);
    }

}

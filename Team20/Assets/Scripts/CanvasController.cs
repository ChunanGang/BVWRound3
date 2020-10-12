using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
    // GAME STAGE: 0: start menu, 1: playing video; 2: tutorial game; 3: video; 4: real game

    public GameObject startMenu;
    public GameObject gameoverPage;
    public GameObject gameStageCover;
    public GameObject tutorialStageObjs;
    public GameObject gameStageObjs;


    private GameObject[] gameUI;

    public BossController boss;
    public MinionsController tutorialBoss;
    public PlayerController player1;
    public PlayerController player2;
    public Manager manager;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI player1ReadyText;
    public TextMeshProUGUI player2ReadyText;
    public Image bossHPTop;
    public Image player1HPTop;
    public Image player2HPTop;
    // video
    private bool playingVideo = false;
    public VideoPlayer startAnim;
    public VideoPlayer afterTutorialVideo;
    public float[] videoLength;

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
        startAnim.url = System.IO.Path.Combine(Application.streamingAssetsPath, "placeholder.mp4");
        afterTutorialVideo.url = System.IO.Path.Combine(Application.streamingAssetsPath, "anim.mp4");
        startMenu.SetActive(true);
        gameStageCover.SetActive(true);
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
        // boss (tutorial boss or real boss)
        if (manager.gameStage == 2)
            bossHPTop.fillAmount = tutorialBoss.getHP() / tutorialBoss.maxHP;
        else
            bossHPTop.fillAmount = boss.getHP() / boss.maxHP;

        player1HPTop.fillAmount = player1.getHP() / player1.maxHP;
        player2HPTop.fillAmount = player2.getHP() / player2.maxHP;
    }
    void updateReady()
    {
        if (manager.player1Ready)
        {
            if (manager.player1Dead)
            {
                player1ReadyText.SetText("DEAD");
            }
            else
                player1ReadyText.SetText("");
        }
        else
            player1ReadyText.SetText("Press A");

        if (manager.player2Ready)
        {
            if (manager.player2Dead)
            {
                player2ReadyText.SetText("DEAD");
            }
            else
                player2ReadyText.SetText("");
        }
        else
            player2ReadyText.SetText("Press A");
    }

    public void setGameInfo(string info)
    {
        infoText.SetText(info);
    }

    public void moveToStageOne()
    {
        StopAllCoroutines();
        // from menu/gameover_page to vedio
        lastFadingRoutine = fadeToStageOne();
        StartCoroutine(lastFadingRoutine);
    }
    public void skipVideo()
    {
        // skip video
        //print("skiping v");
        playingVideo = false;
        StartCoroutine(fadeToStageTwo());
    }

    // fade from menu to the starting video
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
        // remove start menu / gameover menu and play video
        startMenu.SetActive(false);
        gameoverPage.SetActive(false);
        startAnim.gameObject.SetActive(true);
        StartCoroutine(playAnimVideo());
        manager.gameStage = 1;
        // disenable game ui 
        foreach (GameObject obj in gameUI)
        {
            obj.SetActive(false);
        }

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
        yield return new WaitForSeconds(videoLength[0]);
        // if the video still active, stop it and move to game stage
        if (playingVideo)
        {
            playingVideo = false;
            StartCoroutine(fadeToStageTwo());
        }
    }

    // fade from the starting video to tutorial stage
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
        gameStageObjs.SetActive(false);
        tutorialStageObjs.SetActive(true);
        manager.resetMinions();
        gameStageCover.SetActive(false);
        audioController.AC.PlayBgm("gamebgm");

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

    // called when tutorial boss is dead
    public void moveToStageThree()
    {
        // from tutorial stage to after-tutor vedio
        manager.gameStarted = false;
        lastFadingRoutine = fadeToStageThree();
        StartCoroutine(lastFadingRoutine);
    }
    // fade from tutorial stage to after-tutorial video
    private IEnumerator fadeToStageThree()
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
        // remove gameUI, tutorial game objects, and play video
        foreach (GameObject obj in gameUI)
        {
            obj.SetActive(false);
        }
        tutorialStageObjs.SetActive(false);
        // player the video
        manager.gameStarted = false;
        afterTutorialVideo.gameObject.SetActive(true);
        StartCoroutine(playAfterTutorialVideo());
        manager.gameStage = 3;

        // fade out
        while (fadeImageA > 0)
        {
            fadeImage.color = new Color32(0, 0, 0, (byte)fadeImageA);
            fadeImageA -= maxAlpha / 30f;
            yield return new WaitForSeconds(fadeTime / 30);
        }
        fadeImage.gameObject.SetActive(false);

    }
    private IEnumerator playAfterTutorialVideo()
    {
        gameStageCover.SetActive(true);
        afterTutorialVideo.Play();
        yield return new WaitForSeconds(videoLength[1]);
        // if the video still active, stop it and move to game stage
        playingVideo = false;
        StartCoroutine(fadeToStageFour());
    }

    // fade from the after-tutor video to real game stage
    IEnumerator fadeToStageFour()
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
        afterTutorialVideo.Stop();
        afterTutorialVideo.gameObject.SetActive(false);
        // enable game ui 
        foreach (GameObject obj in gameUI)
        {
            obj.SetActive(true);
        }
        manager.gameStage = 4;
        manager.gameStarted = false;
        // enable game stage objs
        gameStageObjs.SetActive(true);
        manager.resetBoss();
        gameStageCover.SetActive(false);
        // reset player 's status and boss status
        manager.resetPlayers();
        manager.resetBoss();
        // resume bgm
        audioController.AC.PlayBgm("gamebgm");
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


    // set to game over page
    public void setGameOver()
    {
        gameStageCover.SetActive(true);
        StartCoroutine(fadeToGameOver());
    }
    // fade to gameover page
    IEnumerator fadeToGameOver()
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
        yield return new WaitForSeconds(1.5f);
        manager.gameStage = -1;
        gameoverPage.SetActive(true);
        gameStageObjs.SetActive(true);

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

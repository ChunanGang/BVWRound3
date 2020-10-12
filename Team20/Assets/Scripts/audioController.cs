using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
	public AudioSource bgm;
	public AudioSource player1;
	public AudioSource player2;
	public AudioSource boss;
	public static audioController AC;
	public float maxBGMvol = 0.15f;
    float soundVel = 0;
    float smoothTime = 0.3f;
    private Dictionary<string, AudioClip> sounds;

    // Start is called before the first frame update
    void Awake()
    {
        AC = this;

        bgm.loop = true;
        player1.loop = false;
        player2.loop = false;
        boss.loop = false;
        sounds = new Dictionary<string, AudioClip>();
        sounds.Add("bgm", Resources.Load("windowbgm", typeof(AudioClip)) as AudioClip);
        sounds.Add("player1Ult", Resources.Load("Character1_ULT", typeof(AudioClip)) as AudioClip);
        sounds.Add("player2Ult", Resources.Load("Character2_ULT", typeof(AudioClip)) as AudioClip);
        sounds.Add("player1Att", Resources.Load("Character1_Attact", typeof(AudioClip)) as AudioClip);
        sounds.Add("player2Collect", Resources.Load("Character2_Get", typeof(AudioClip)) as AudioClip);
        sounds.Add("itemCollect", Resources.Load("Get_ULT", typeof(AudioClip)) as AudioClip);
        sounds.Add("mouseClick", Resources.Load("Mouse_Click", typeof(AudioClip)) as AudioClip);
        sounds.Add("mouseMove", Resources.Load("Mouse_Move", typeof(AudioClip)) as AudioClip);
        sounds.Add("bossDie", Resources.Load("BossDie", typeof(AudioClip)) as AudioClip);
        sounds.Add("characterAttacked", Resources.Load("Character_Attacked", typeof(AudioClip)) as AudioClip);
        sounds.Add("enemyBossAttacked", Resources.Load("EnemyBoss_Attacked", typeof(AudioClip)) as AudioClip);
        sounds.Add("enemyBossBullet", Resources.Load("EnemyBoss_Bullet", typeof(AudioClip)) as AudioClip);
        sounds.Add("enemyDie", Resources.Load("EnemyDie", typeof(AudioClip)) as AudioClip);
        sounds.Add("tutorialbgm", Resources.Load("TutorialBGM_Loop", typeof(AudioClip)) as AudioClip);
        sounds.Add("gamebgm", Resources.Load("GameBGM_Loop", typeof(AudioClip)) as AudioClip);
        bgm.clip = sounds["bgm"];
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPlayer1(string soundID, float vol = 0.5f)
    {
        Debug.Log("playplayer1 called here");
        AudioClip clip = sounds[soundID];
        Debug.Log(clip);
        player1.PlayOneShot(clip, vol);
    }

    public void PlayPlayer2(string soundID, float vol = 0.5f){
    	AudioClip clip = sounds[soundID];
        player2.PlayOneShot(clip, vol);
    }

    public void PlayBoss(string soundID, float vol = 0.5f){
    	AudioClip clip = sounds[soundID];
        boss.PlayOneShot(clip, vol);
    }

    public void PlayBgm(string soundID, float vol = 0.5f){
        bgm.Stop();
    	AudioClip clip = sounds[soundID];
        bgm.PlayOneShot(clip, vol);
    }

    public void muteAll(){
        bgm.volume = 0; 
        player1.volume = 0;
        player2.volume = 0; 
        boss.volume =0;
    }

    public void unmuteAll(){
        bgm.volume = 0.5f; 
        player1.volume = 0.5f;
        player2.volume = 0.5f; 
        boss.volume =0.5f;
    }

}

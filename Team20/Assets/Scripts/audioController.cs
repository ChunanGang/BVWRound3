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
        sounds.Add("player1Ult", Resources.Load("Chatacter1_ULT", typeof(AudioClip)) as AudioClip);
        sounds.Add("player2Ult", Resources.Load("Chatacter2_ULT", typeof(AudioClip)) as AudioClip);
        sounds.Add("player1Att", Resources.Load("Chatacter1_Attact", typeof(AudioClip)) as AudioClip);
        sounds.Add("player2Att", Resources.Load("Chatacter2_Get", typeof(AudioClip)) as AudioClip);
        sounds.Add("itemCollect", Resources.Load("Get_ULT", typeof(AudioClip)) as AudioClip);
        sounds.Add("mouseClick", Resources.Load("Mouse_Click", typeof(AudioClip)) as AudioClip);
        sounds.Add("mouseMove", Resources.Load("Mouse_Move", typeof(AudioClip)) as AudioClip);
        bgm.clip = sounds["bgm"];
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPlayer1(string soundID, float vol = 0.5f)
    {
        AudioClip clip = sounds[soundID];
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
    	AudioClip clip = sounds[soundID];
        bgm.PlayOneShot(clip, vol);
    }

}

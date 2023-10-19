using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public bool isMute= false;
    // AudioSource để phát âm thanh
    private AudioSource audioSource;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Khởi tạo AudioSource
            audioSource = gameObject.AddComponent<AudioSource>();

            // Load tất cả các AudioClip và đặt chúng vào Dictionary
            LoadAllAudioClips();
        }
    }

    void LoadAllAudioClips()
    {
        // Lấy tất cả các file âm thanh trong thư mục
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        Debug.Log(clips.Length);
        // Đặt các âm thanh vào Dictionary với tên là key
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }
    }
    //Phát nhạc dựa vào tên audio clip
    public void PlaySound(string soundName)
    {
        if (audioClips.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(audioClips[soundName]);
        }
        else
        {
            Debug.Log("Khong co file am thanh voi ten " + soundName);
        }
    }
    //Bật tắt âm thanh
    public void OnOffSound()
    {
        isMute = !isMute;
        audioSource.mute = isMute;
    }
}

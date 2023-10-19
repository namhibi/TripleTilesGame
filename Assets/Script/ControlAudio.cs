using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAudio : MonoBehaviour
{
    public void ControlAudioOnOff()
    {
        AudioManager.Instance.OnOffSound();
    }
    private void OnEnable()
    {
        transform.GetComponent<ToggleImage>().changeImage();
    }

}

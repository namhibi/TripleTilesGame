using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleImage : MonoBehaviour
{
    public void changeImage()
    {
        if (AudioManager.Instance.isMute)
        {
            transform.GetComponent<SpriteAtlasRender>().RenderImage("Icon_Music_Off");
            transform.parent.GetComponent<Image>().color = Color.red;
        }
        else
        {
            transform.GetComponent<SpriteAtlasRender>().RenderImage("Icon_Music_On");
            transform.parent.GetComponent<Image>().color = Color.green;
        }
    }
}

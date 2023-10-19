using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    private static PopupManager instance;
    private bool isOpen = false;
    public GameObject ui, overlay, replay, close;
    public TextMeshProUGUI title;
    public static PopupManager Instance { get { return instance; } }
    private void Awake()
    {
        PopupManager.instance = this;

    }
    public void showUi()
    {
        if (!isOpen)
        {
            overlay.SetActive(true);
            ui.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            overlay.SetActive(false);
            ui.SetActive(false);
            Time.timeScale = 1f;
        }
        isOpen = !isOpen;
    }
    public void changecTitle(int i)
    {
        switch (i)
        {
            case 0:
                title.text = "Pause";
                close.SetActive(true);
                replay.GetComponentInChildren<SpriteAtlasRender>().RenderImage("Icon_Restart");
                break;
            case 1:
                title.text = "Win";
                close.SetActive(false);
                replay.GetComponentInChildren<SpriteAtlasRender>().RenderImage("Icon_Play");
                break;
            case 2:
                title.text = "Lose";
                close.SetActive(false);
                replay.GetComponentInChildren<SpriteAtlasRender>().RenderImage("Icon_Restart");
                break;
        }
        showUi();
    }
}

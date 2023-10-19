using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public class SpriteAtlasRender : MonoBehaviour
{
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private string sprite_name;
    // Start is called before the first frame update
    void Start()
    {
    }

   public void RenderImage(string name)
    {
        GetComponent<Image>().sprite=atlas.GetSprite(name);
    }
    public void RenderSprite(string name)
    {
        GetComponent<SpriteRenderer>().sprite = atlas.GetSprite(name);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile_Display : MonoBehaviour
{
    public Tile_Data tile;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadData(Tile_Data tile)
    {
        this.tile = tile;
        GetComponentInChildren<SpriteAtlasRender>().RenderSprite(tile.spriteName);
    }
}

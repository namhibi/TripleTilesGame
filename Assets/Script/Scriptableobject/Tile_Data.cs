using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTile", menuName = "Tile", order = 1)]
public class Tile_Data : ScriptableObject
{
    private bool isIdGenerated = false;
    [SerializeField]
    private string iD;
    public string name;
    public string spriteName;
    public Sprite tile_Image;

    private void OnEnable()
    {
        if (!isIdGenerated)
        {
            GenerateRandomId();
            isIdGenerated = true;
        }
        spriteName=tile_Image.name;
    }
    public string Tile_ID
    {
        get { return iD; }
    }
    private void GenerateRandomId()
    {
        iD = $"{DateTime.Now.Ticks}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level", order = 1)]

public class Level : ScriptableObject
{
    public string Name;
    public string DisplayName;
    public int Lv;
    public int PlayTime;
    public List<MyKeyValuePair> dictionaryList;

    [System.Serializable]
    public class MyKeyValuePair
    {
        public Tile_Data tile;
        public int quantity;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu(fileName = "NewUser", menuName = "UserData", order = 1)]
public class User_Data : ScriptableObject
{
    public Level currentLevel;
    public int totalScore=0;
    public Level[] lvlList;
    int index = 0;
    private void OnEnable()
    {
        lvlList = Resources.LoadAll<Level>("Level");
        currentLevel= lvlList[index];
    }
    public void nextLvl()
    {
        index++;
        if(index>=lvlList.Length)
        {
            index= 0;
        }
        currentLevel = lvlList[index];
    }
}

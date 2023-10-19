using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    private TextMeshProUGUI score_txt, lvl_txt;
    public User_Data userData;
    // Start is called before the first frame update
    private void Awake()
    {
        score_txt = GameObject.Find("Score_txt").GetComponent<TextMeshProUGUI>();
        lvl_txt = GameObject.Find("Level_txt").GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {

        DisPlay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void DisPlay()
    {
        score_txt.text = userData.totalScore.ToString();
        score_txt.color = Color.black;
        lvl_txt.text = userData.currentLevel.DisplayName.ToString();
        lvl_txt.color = Color.white;
    }
    private void OnEnable()
    {
        DisPlay();
    }
    public void LoadPlayScene()
    {
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex+1);
    }
}

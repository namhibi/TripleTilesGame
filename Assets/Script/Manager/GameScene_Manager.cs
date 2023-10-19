using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene_Manager : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    private List<GameObject> Tileobject = new List<GameObject>();
    private Dictionary<string, TileStatus> countTile = new Dictionary<string, TileStatus>();
    GameObject Tile_Bar;
    public User_Data userData;
    public GameObject comboBar;
    private Level lv;
    private TextMeshProUGUI score_txt, time_txt, lvl_txt;
    private int score = 0, time, countwin = 0;
    bool isPause = false;
    bool isCombo = false;
     int combo = 0;
    int maxList = 7;
    GameObject slide;
    private IEnumerator coroutine;
   
    private void Awake()
    {
        Tile_Bar = GameObject.Find("Tile_Bar");
        score_txt = GameObject.Find("Score_txt").GetComponent<TextMeshProUGUI>();
        time_txt = GameObject.Find("Time_txt").GetComponent<TextMeshProUGUI>();
        lvl_txt = GameObject.Find("Level_txt").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        lv = userData.currentLevel;
        LoadPositionTileList(Tile_Bar);
        LoadGameLevel();
        StartCoroutine(CountTime());
        slide = comboBar.transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isPause == false)
        {
            if(coroutine== null)
            {
                // Tạo một ray từ vị trí chuột
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Tạo RaycastHit để lưu thông tin về va chạm
                RaycastHit hit;

                // Kiểm tra xem ray có va chạm với một đối tượng không
                if (Physics.Raycast(ray, out hit))
                {
                    // Lấy GameObject mà ray va chạm
                    GameObject clickedObject = hit.collider.gameObject;
                    if (clickedObject.GetComponentInParent<Tile_Display>())
                    {
                        Debug.Log("Clicked on: " + clickedObject.GetComponentInParent<Tile_Display>().tile.name);
                        AudioManager.Instance.PlaySound("Click_2");
                        ResetTilesStt(clickedObject);
                        CheckTile(clickedObject);
                    }
                }
            }
        }
    }
    //Load vị trí của các ô chứa tile
    void LoadPositionTileList(GameObject Tile_Bar)
    {
        for (int i = 0; i < Tile_Bar.transform.childCount; i++)
        {
            positions.Add(Tile_Bar.transform.GetChild(i).position);
        }
        /*  foreach (Vector3 pos in positions)
          {
              Debug.Log("x:"+pos.x+"y:"+pos.y+"z:"+pos.z);
          }*/

    }
    //Di chuyển các tile đến vị trí tương ứng trên ô chứa
    void MoveToPosition(int index, GameObject obj)
    {
        obj.transform.DOMove(positions[index], 0.5f);
    }
    //Kiểm tra tile trước khi thêm vào danh sách
    void CheckTile(GameObject obj)
    {
        Tile_Data data = obj.GetComponentInParent<Tile_Display>().tile;
        if (countTile.ContainsKey(data.Tile_ID))
        {
            if (countTile[data.Tile_ID].count < 2)
            {
                Tileobject.Insert(countTile[data.Tile_ID].count + countTile[data.Tile_ID].firstIndex, obj);
                countTile[data.Tile_ID].count++;
                UpdatePos();
                UpdateFirstIndex();
                if (Tileobject.Count == maxList) { 
                    PopupManager.Instance.changecTitle(2);
                    AudioManager.Instance.PlaySound("Lose");
                }
            }
            else
            {
                Tileobject.Insert(countTile[data.Tile_ID].count + countTile[data.Tile_ID].firstIndex, obj);
                countTile[data.Tile_ID].count++;
                UpdatePos();
                coroutine = UpdateScore(countTile[data.Tile_ID].firstIndex);
                StartCoroutine(coroutine);
                countTile.Remove(data.Tile_ID);

                if (isCombo)
                {
                    StopCoroutine("CountCombo");
                }
                StartCoroutine("CountCombo");
            }
        }
        else
        {
            countTile[data.Tile_ID] = new TileStatus(1, Tileobject.Count);
            Tileobject.Add(obj);
            MoveToPosition(Tileobject.Count - 1, obj);
            if (Tileobject.Count == maxList)
            {
                PopupManager.Instance.changecTitle(2);
                AudioManager.Instance.PlaySound("Lose");
            }
        }
    }
    //Di chuyển toàn bộ Tile trong danh sách
    void UpdatePos()
    {
        for (int i = 0; i < Tileobject.Count; i++)
        {
            MoveToPosition(i, Tileobject[i]);
        }
    }
    //Cập nhật lại vị trí xuất hiện đầu tiên của 1 Tile
    void UpdateFirstIndex()
    {
        Tile_Data data;
        for (int i = 0; i < Tileobject.Count; i++)
        {
            data = Tileobject[i].GetComponentInParent<Tile_Display>().tile;
            if (countTile.ContainsKey(data.Tile_ID))
            {
                if (!countTile[data.Tile_ID].isUpDate)
                {
                    countTile[data.Tile_ID].isUpDate = true;
                    countTile[data.Tile_ID].firstIndex = i;
                }
            }
        }
        foreach (var pair in countTile)
        {
            pair.Value.isUpDate = false;
        }
    }
    // Load game dựa vào data của Level
    void LoadGameLevel()
    {
        time = lv.PlayTime;
        ConvertTime(time);
        score_txt.text = score.ToString();
        lvl_txt.text = lv.DisplayName.ToString();
        for (int i = 0; i < lv.dictionaryList.Count; i++)
        {
            countwin += lv.dictionaryList[i].quantity;
            int count = lv.dictionaryList[i].quantity * 3;
            transform.GetComponent<TileSpawner>().SpawnObject(lv.dictionaryList[i].tile, count);
        }
    }
    //Chỉnh lại tình trạng tile trước khi vào list: tắt rigidboy, tắt shadow, tắt box collider, reset rotation và chỉnh lại độ lớn của tile
    void ResetTilesStt(GameObject obj)
    {
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        obj.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f);
        Renderer[] childRenderers = obj.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
    //Chuyển đổi thời gina từ giây sang định dạng phút
    void ConvertTime(int second)
    {
        int minute = second / 60;
        int secondleft = second % 60;
        time_txt.text = $"{minute:D2}:{secondleft:D2}";
    }
    //Reset lại game. Nếu level của game được cập nhật sẽ chuyển đến level tiếp theo
    public void ResetGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1.0f;
    }
    //Load lại màn hình home
    public void LoadHomeScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex -1);
    }
    //thay đổi cờ xem game có đang chạy hay không
    public void PauseOrContinue()
    {
        isPause = !isPause;
    }
    //Thay đổi ui thanh combo
    void ChangeComboBarUI()
    {
        comboBar.GetComponentInChildren<TextMeshProUGUI>().text = "X" + combo;
        if (combo <= 3)
        {
            slide.GetComponent<Image>().color = Color.red;
            comboBar.GetComponentInChildren<TextMeshProUGUI>().color= Color.red;
            return;
        }
        if (combo > 3 && combo <= 9)
        {
            slide.GetComponent<Image>().color = Color.yellow;
            comboBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.yellow;
            return;
        }
        else
        {
            slide.GetComponent<Image>().color = Color.green;
            comboBar.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            return;
        }
    }
    //Tăng giới hạn danh sách
    public void IncreaseMaxList()
    {
        maxList++;
    }
    //Cập nhật điểm dồng thời xét điều kiện thắng
    IEnumerator UpdateScore(int index)
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = Tileobject[index];
            Tileobject.Remove(obj);
            Destroy(obj);
        }
        score += combo;
        score_txt.text = score.ToString();
        UpdatePos();
        UpdateFirstIndex();
        countwin--;
        AudioManager.Instance.PlaySound("Get_Score");
        if (countwin == 0)
        {
            userData.totalScore += score;
            AudioManager.Instance.PlaySound("Win");
            PopupManager.Instance.changecTitle(1);
            userData.nextLvl();
        }
        coroutine = null;
    }
    //Tính thời gian
    IEnumerator CountTime()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time -= 1;
            ConvertTime(time);
        }
        PopupManager.Instance.changecTitle(2);
        AudioManager.Instance.PlaySound("Lose");
    }
    //Tính combo
    IEnumerator CountCombo()
    {   
        float comboTime = 4;
        isCombo = true;
        combo += 1;
        ChangeComboBarUI();
        slide.transform.localScale = new Vector3(1, slide.transform.localScale.y, slide.transform.localScale.z);
        comboBar.SetActive(true);
        while (comboTime > 0)
        {
            yield return new WaitForSeconds(0.2f);
            comboTime -= 0.2f;
            slide.transform.DOScaleX(comboTime / 4, 0.2f);
        }
        isCombo = false;
        comboBar.SetActive(false);
        combo = 0;
        ChangeComboBarUI();
    }
    public class TileStatus
    {
        public int count;
        public int firstIndex;
        //Cờ kiểm tra tile đó update hay chưa
        public bool isUpDate;
        public TileStatus(int count, int firstIndex)
        {
            this.count = count;
            this.firstIndex = firstIndex;
            this.isUpDate = false;
        }
    }
}

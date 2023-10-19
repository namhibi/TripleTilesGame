using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
   public void SpawnObject(Tile_Data tile,int quantity)
    {
        // Lấy kích thước của màn hình trong đơn vị Viewport
        float viewportWidth = Camera.main.orthographicSize * 2.0f * Camera.main.aspect;
        float viewportHeight = Camera.main.orthographicSize * 2.0f;
        while (quantity > 0)
        {
            // Random vị trí x và y trong khoảng từ 0 đến 1
            float randomZ = Random.Range(-viewportHeight/2+2.5f, viewportHeight/2-2f);
            float randomX = Random.Range(-viewportWidth/2+1f, viewportWidth/2-1f);
            float randomY= Mathf.RoundToInt(Random.Range(1f, 5f));
            // Chuyển đổi giá trị x và y sang đơn vị World (thế giới game)
            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
            // Spawn game object tại vị trí đã tính toán
            GameObject obj=Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            obj.transform.GetComponent<Tile_Display>().loadData(tile);
            quantity -= 1;
        }
       
    }
}

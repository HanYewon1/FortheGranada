using UnityEngine;

public class bosscam : MonoBehaviour
{
    public float mapHeight;
    public float mapWidth;
    GameObject mapObject;
    SpriteRenderer renderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapObject = GameObject.Find("BossRoom");
        renderer = mapObject.GetComponent<SpriteRenderer>();
        mapWidth = renderer.bounds.size.x;
        mapHeight = renderer.bounds.size.y;

        // 화면 비율 계산
        float aspectRatio = (float)Screen.width / Screen.height;
        // 가로와 세로 비율에 맞는 OrthographicSize 계산
        if (aspectRatio >= 1)
        {
            // 화면이 가로로 긴 경우
            Camera.main.orthographicSize = mapHeight / 2f;
        }
        else
        {
            // 화면이 세로로 긴 경우
            Camera.main.orthographicSize = mapWidth / (2f * aspectRatio);
        }

        // 카메라의 위치 조정 (맵의 중앙으로 카메라 이동)
        Camera.main.transform.position = new Vector3(mapWidth / 2f, mapHeight / 2f, -10f); //Camera.main.transform.position.z);
        // 카메라가 맵을 벗어나지 않도록 추가적인 안전장치
        float maxSize = Mathf.Max(mapWidth / 2f, mapHeight / 2f);
        Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, maxSize);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

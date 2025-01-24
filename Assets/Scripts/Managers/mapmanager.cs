using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.U2D.Aseprite;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    // 싱글톤 패턴 적용
    public static MapManager Instance;
    
    [System.Serializable]
    public struct PrefabEntry
    {
        public string prefabName;
        public GameObject prefab;
    }
    public int stage;
    public int diff;
    public Button button;
    public GameObject player;
    public GameObject[] prefabEntries;
    public Queue<GameObject> queue_room;
    public TextMeshProUGUI text_size;
    public int[] stage_size = { 0, 5, 6, 7, 7 };
    public int[] room_count = { 0, 10, 15, 20, 25 };
    public int[] type_room_count = { 0, 5, 7, 9, 11 };
    public int[] type_room = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    bool isCreate;
    string minimap_name;

    private void Awake()
    {
        // Instance 존재 유무에 따라 게임 매니저 파괴 여부 정함
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 기존에 존재 안하면 이걸로 대체하고 파괴하지 않기
        }
        else
        {
            Destroy(gameObject); // 기존에 존재하면 자신파괴
        }
        queue_room = new Queue<GameObject>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitMap());
    }

    private IEnumerator InitMap()
    {
        yield return null;
        stage = GameManager.Instance.stage;
        diff = GameManager.Instance.diff;
        
        if (GameManager.Instance.is_ingame)
        {
            player = GameManager.Instance.player.gameObject;
            CreateMap(stage);
        }
    }

    public void OnClick()
    {
        if (isCreate == false)
        {
            CreateMap(stage);
            isCreate = true;
        }
        else
        {
            while (queue_room.Count > 0)
            {
                GameObject destroy_room = queue_room.Dequeue();
                Destroy(destroy_room);
            }
            isCreate = false;
        }
        Debug.Log("onclick");
    }

    public void OnClickBackButton()
    {
        GameManager.Instance.is_ingame = false;
        GameManager.Instance.is_boss = false;
        GameManager.Instance.is_running = false;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Update()
    {
        if (!GameManager.Instance.is_running)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (stage < 3)
                {
                    stage++;
                    text_size.text = "Size: " + stage_size[stage] + "X" + stage_size[stage];
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (stage > 0)
                {
                    stage--;
                    text_size.text = "Size: " + stage_size[stage] + "X" + stage_size[stage];
                }
            }
        }
    }
    void CreateMap(int stage_num)
    {
        isCreate = true;
        if (stage_num == 0)
        {
            return;
        }
        if (diff == 3)
        {
            stage_num++;
        }
        var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
        int size = stage_size[stage_num];
        int count = room_count[stage_num];
        var parent = new (int, int)[size, size];
        int[,] stage_room = new int[size, size];
        bool[,] visited = new bool[size, size];
        minimap_name = "Canvas/InGameUI/GRayout" + size + "X" + size;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                stage_room[x, y] = -1;
                visited[x, y] = false;
            }
        }
        int start_x = UnityEngine.Random.Range(1, size - 1);
        int start_y = UnityEngine.Random.Range(1, size - 1);
        int finish_x, finish_y;

        do
        {
            finish_x = UnityEngine.Random.Range(0, size);
            finish_y = UnityEngine.Random.Range(0, size);
        }
        while (finish_x == start_x && finish_y == start_y);

        stage_room[start_x, start_y] = 11;
        stage_room[finish_x, finish_y] = 12;

        Queue<(int, int)> queue = new Queue<(int, int)>();
        queue.Enqueue((start_x, start_y));

        visited[start_x, start_y] = true;
        List<(int, int)> path = new List<(int, int)>();
        path.Add((start_x, start_y));
        path.Add((finish_x, finish_y));

        while (true)
        {
            var current = queue.Dequeue();
            int current_x = current.Item1;
            int current_y = current.Item2;
            if (current_x == finish_x && current_y == finish_y)
            {
                while (true)
                {
                    current = parent[current.Item1, current.Item2];
                    if (current.Item1 == start_x && current.Item2 == start_y)
                    {
                        break;
                    }
                    else
                    {
                        stage_room[current.Item1, current.Item2] = UnityEngine.Random.Range(0, type_room_count[stage_num]);
                        path.Add(current);
                    }
                }
                break;
            }

            foreach (var (dx, dy) in directions)
            {
                int x = current_x + dx;
                int y = current_y + dy;

                if (x >= 0 && y >= 0 && x < size && y < size && !visited[x, y])
                {
                    queue.Enqueue((x, y));
                    visited[x, y] = true;
                    parent[x, y] = (current_x, current_y);
                }
            }
        }

        while (path.Count < count)
        {
            int num = UnityEngine.Random.Range(0, path.Count);
            var random_room = path[num];
            int random_direction = UnityEngine.Random.Range(0, 4);
            var (dx, dy) = directions[random_direction];

            int x = random_room.Item1 + dx;
            int y = random_room.Item2 + dy;
            if (x >= 0 && y >= 0 && x < size && y < size && stage_room[x, y] == -1)
            {
                stage_room[x, y] = UnityEngine.Random.Range(0, type_room_count[stage_num]);
                path.Add((x, y));
            }
        }

        for (int i = 0; i < path.Count; i++)
        {
            int x = path[i].Item1;
            int y = path[i].Item2;
            int room = stage_room[x, y];
            Vector3 position = new Vector3(x * 35, y * (-35), 0);
            quaternion rotation = Quaternion.Euler(0, 0, 0);
            GameObject instance = Instantiate(prefabEntries[room], position, rotation);
            string minimap_image_name = minimap_name + "/Image " + y + x;
            GameObject minimap_image = GameObject.Find(minimap_image_name);
            Image image = minimap_image.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.white;
            }
            if (room == 11)
            {
                Debug.Log("start_room : " + x + " " + y);
                image.color = Color.red;
                if (player != null)
                {
                    player.transform.position = position;
                    playercontroller player_controller = player.GetComponent<playercontroller>();
                    player_controller.room_x = x;
                    player_controller.room_y = y;
                    player_controller.minimap_name = minimap_name + "/Image ";
                    player_controller.is_door = false;
                    GameManager.Instance.sc.UpdateBorder();
                }
            }
            if (room == 12)
            {
                image.color = Color.blue;
            }

            if (i > 1)
            {
                instance.name = "Room " + x + "_" + y;
            }
            for (int j = 0; j < 4; j++)
            {
                var (dx, dy) = directions[j];
                int current_x = x + dx;
                int current_y = y + dy;
                string door_name = "door/";

                if (current_x >= 0 && current_y >= 0 && current_x < size && current_y < size && stage_room[current_x, current_y] != -1)
                {
                    switch (j)
                    {
                        case 0:
                            door_name += "door_left";
                            break;
                        case 1:
                            door_name += "door_right";
                            break;
                        case 2:
                            door_name += "door_up";
                            break;
                        case 3:
                            door_name += "door_down";
                            break;
                        default: break;
                    }
                    Transform door = instance.transform.Find(door_name);
                    if (door != null)
                    {
                        door.gameObject.SetActive(true);
                    }
                }


            }
            queue_room.Enqueue(instance);
        }
        return;
    }


}



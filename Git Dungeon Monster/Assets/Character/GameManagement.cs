using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemy;
    [SerializeField] private GameObject[] door;
    [SerializeField] private GameObject ButtonUI;
    GameObject[] rightdoor;
    GameObject[] leftdoor;
    PlayerController PlayerScript;
    bool[] IsOpen;
    bool clear;
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //プレイヤースクリプトを取得
        IsOpen = new bool[door.Length];
        rightdoor = new GameObject[door.Length];
        leftdoor = new GameObject[door.Length];
        for (int i = 0; i < door.Length; i++)
        {
            rightdoor[i] = door[i].transform.Find("Cube_025").gameObject;
            leftdoor[i]  = door[i].transform.Find("Cube_028").gameObject;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        for (int i = 0; i < door.Length; i++)
        {
            if (!IsOpen[i] && Enemy[i].transform.childCount == 0)   //フロアのEnemyを倒しているか
            {
              
                rightdoor[i].transform.rotation = Quaternion.Euler(0,90,0);
                leftdoor[i].transform.rotation = Quaternion.Euler(0, -90, 0);
                IsOpen[i] = true;
            }
        }

        GameClear();
    }

    void GameClear()
    {
        if (Enemy[door.Length - 1].transform.childCount == 0 && !clear)
        {
            PlayerScript.SwitchingMove();
            ButtonUI.SetActive(true);
            clear = true;

        }
    }
}

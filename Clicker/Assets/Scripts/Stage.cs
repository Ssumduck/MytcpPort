using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private void Start()
    {
        MonsterSpawn(GameData.stage);
    }

    public static void MonsterSpawn(int idx)
    {
        Transform monsterSpawnPos = GameObject.Find("MonsterSpawnPos").transform;
        GameObject go = Instantiate(Resources.Load<GameObject>($"Prefabs/Monster/0"), monsterSpawnPos);
        go.transform.localPosition = Vector3.zero;
    }
}
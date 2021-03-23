using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static GameData gameData { get { Init(); return gameData; } private set { gameData = value; } }

    static void Init()
    {
        if(gameData == null)
        {
            gameData = GameObject.FindObjectOfType<GameData>();
        }
    }
}
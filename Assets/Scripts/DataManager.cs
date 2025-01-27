using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DataManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.TextAsset jsonFile; // UnityのInspectorでJSONファイルを指定

    void Start()
    {
        // JSONデータを配列にデシリアライズ
        GameData[] gameDatum = JsonUtility.FromJson<GameDataArray>(jsonFile.text).gameDatum;

        // 各キャラクター情報をログに表示
        foreach (var gameData in gameDatum)
        {
            
        }
    }
}

[System.Serializable]
public class GameDataArray
{
    public GameData[] gameDatum;
}


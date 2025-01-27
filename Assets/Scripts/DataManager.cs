using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DataManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.TextAsset jsonFile; // UnityのInspectorでJSONファイルを指定
    public static DataManager Instance; // シングルトンインスタンス

    public GameData[] gameDatum; // 他のクラスでも参照できるようにpublicにする

    void Awake()
    {
        // シングルトンのインスタンスを設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 重複を防ぐ
        }

        // JSONデータを配列にデシリアライズ
        GameDataArray dataArray = JsonUtility.FromJson<GameDataArray>(jsonFile.text);
        Instance.gameDatum = dataArray.gameDatum;
    }

    void Start()
    {

        // 各キャラクター情報をログに表示
        /*foreach (var gameData in Instance.gameDatum)
        {
            Debug.Log(gameData.id);
        }
        */
    }
}

[System.Serializable]
public class GameDataArray 
{
    public GameData[] gameDatum;
}


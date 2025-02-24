using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PanelManager : MonoBehaviour
{
    private Vector3 centerPosition = new Vector3(0, 2.5f, 5.5f);
    private Vector3 rightPosition = new Vector3(13f, 3f, 10f);
    private Vector3 leftPosition = new Vector3(-13f, 3f, 10f);

    private Vector3 rightOff = new Vector3(30f, 2f, 10f);
    private Vector3 leftOff = new Vector3(-30f, 2f, 10f);

    private int[] idState = new int[3];

    [SerializeField] private GameObject panelPrefab; //画像を表示させるオブジェクト
    [SerializeField] private GameObject childPanelPrefab; //パネルの装飾に用いるオブジェクト

    //画像を表示させるオブジェクト変数
    private GameObject centerPanel;
    private GameObject rightPanel;
    private GameObject leftPanel;
    private GameObject newLeftPanel;
    private GameObject newRightPanel;

    //パネルの装飾に用いるオブジェクト変数
    private GameObject childCenterPanel;
    private GameObject childRightPanel;
    private GameObject childLeftPanel;
    private GameObject childNewLeftPanel;
    private GameObject childNewRightPanel;

    public float moveDuration = 0.01f;
    private float elapsedTime = 0f;
    private bool isMoving = false;

    private string[] fullPath;
    private Texture2D[] textures;

    private int dataSize;
    private NewId newId = new NewId();
    // Start is called before the first frame update
    void Start()
    {
        dataSize = DataManager.Instance.gameDatum.Length;
        fullPath = new string[dataSize];
        textures = new Texture2D[dataSize];
        idState[0] = 0;//左のオブジェクトのid初期化
        idState[1] = 1;//正面のオブジェクトのid初期化
        idState[2] = 2;//右のオブジェクトのid初期化
        InitializePanel();
        InitializeImages();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving)
        {
            StartCoroutine(MoveRight());  
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving)
        {
            StartCoroutine(MoveLeft());
        }
        if(Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            //正面のオブジェクトを選択
            Debug.Log(DataManager.Instance.gameDatum[idState[1]].Title);
        }
    }

    void InitializePanel()
    {
        //パネル本体を生成
        centerPanel = Instantiate(panelPrefab, centerPosition, Quaternion.Euler(90, 90, -90));
        rightPanel = Instantiate(panelPrefab, rightPosition, Quaternion.Euler(90, 90, -90));
        leftPanel = Instantiate(panelPrefab, leftPosition, Quaternion.Euler(90, 90, -90));

        //装飾オブジェクトの生成
        childCenterPanel = Instantiate(childPanelPrefab, centerPanel.transform.position + new Vector3(0, 0, 1.1f), Quaternion.Euler(0, 90, 0));
        childRightPanel = Instantiate(childPanelPrefab, rightPanel.transform.position + new Vector3(0, 0, 1.1f), Quaternion.Euler(0, 90, 0));
        childLeftPanel = Instantiate(childPanelPrefab, leftPanel.transform.position + new Vector3(0, 0, 1.1f), Quaternion.Euler(0, 90, 0));

        //親子関係の設定
        childCenterPanel.transform.SetParent(centerPanel.transform);
        childRightPanel.transform.SetParent(rightPanel.transform);
        childLeftPanel.transform.SetParent(leftPanel.transform);
    }

    void InitializeImages()
    {
        for(int i = 0;i < dataSize; i++)
        {
            fullPath[i] = Path.Combine(Application.streamingAssetsPath, DataManager.Instance.gameDatum[i].imagePath);
            textures[i] = LoadTexture(fullPath[i]);
        }
        leftPanel.gameObject.GetComponent<Renderer>().material.mainTexture = textures[idState[0]];
        centerPanel.gameObject.GetComponent<Renderer>().material.mainTexture = textures[idState[1]];
        rightPanel.gameObject.GetComponent<Renderer>().material.mainTexture = textures[idState[2]];
        
    }

    private System.Collections.IEnumerator MoveRight()
    {
        isMoving = true;

        float elapsedTime = 0f; // 経過時間
        newLeftPanel = Instantiate(panelPrefab, rightOff, Quaternion.Euler(90, 90, -90));
        newLeftPanel.gameObject.GetComponent<Renderer>().material.mainTexture = textures[newId.GetNewLeftId(idState, dataSize)];

        childNewLeftPanel = Instantiate(childPanelPrefab, newLeftPanel.transform.position + new Vector3(0, 0, 1.1f), Quaternion.Euler(0, 90, 0));
        childNewLeftPanel.transform.SetParent(newLeftPanel.transform);

        while (elapsedTime < moveDuration)
        {
            // 正規化された時間を計算（0 ～ 1）
            float t = elapsedTime / moveDuration;

            // 滑らかな移動補間を適用
            t = Mathf.SmoothStep(0f, 1f, t);

            // オブジェクトの位置を補間
            rightPanel.transform.position = Vector3.Lerp(rightPosition, rightOff, t);
            centerPanel.transform.position = Vector3.Lerp(centerPosition, rightPosition, t);
            leftPanel.transform.position = Vector3.Lerp(leftPosition, centerPosition, t);
            newLeftPanel.transform.position = Vector3.Lerp(leftOff, leftPosition, t);

            elapsedTime += Time.deltaTime; // 時間を更新
            yield return null; // 次のフレームまで待機
        }
        Destroy(rightPanel.gameObject);
        rightPanel = centerPanel;
        centerPanel = leftPanel;
        leftPanel = newLeftPanel;
        newLeftPanel = null;
        isMoving = false; // 移動終了
    }

    private System.Collections.IEnumerator MoveLeft()
    {
        isMoving = true;

        float elapsedTime = 0f; // 経過時間
        newRightPanel = Instantiate(panelPrefab, leftOff, Quaternion.Euler(90, 90, -90));
        newRightPanel.gameObject.GetComponent<Renderer>().material.mainTexture = textures[newId.GetNewRightId(idState, dataSize)];

        childNewRightPanel = Instantiate(childPanelPrefab, newRightPanel.transform.position + new Vector3(0, 0, 1.1f), Quaternion.Euler(0, 90, 0));
        childNewRightPanel.transform.SetParent(newRightPanel.transform);
        while (elapsedTime < moveDuration)
        {
            // 正規化された時間を計算（0 ～ 1）
            float t = elapsedTime / moveDuration;

            // 滑らかな移動補間を適用
            t = Mathf.SmoothStep(0f, 1f, t);

            // オブジェクトの位置を補間
            rightPanel.transform.position = Vector3.Lerp(rightPosition, centerPosition, t);
            centerPanel.transform.position = Vector3.Lerp(centerPosition, leftPosition, t);
            leftPanel.transform.position = Vector3.Lerp(leftPosition, leftOff, t);
            newRightPanel.transform.position = Vector3.Lerp(rightOff, rightPosition, t);

            elapsedTime += Time.deltaTime; // 時間を更新
            yield return null; // 次のフレームまで待機
        }
        Destroy(leftPanel.gameObject);
        leftPanel = centerPanel;
        centerPanel = rightPanel;
        rightPanel = newRightPanel;
        newRightPanel = null;
        isMoving = false; // 移動終了
    }

    Texture2D LoadTexture(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("ファイルが存在しません: " + path);
            return null;
        }

        // ファイルをバイト配列として読み込む
        byte[] fileData = File.ReadAllBytes(path);

        // Texture2Dを作成
        Texture2D texture = new Texture2D(2, 2); // 仮のサイズで初期化
        if (texture.LoadImage(fileData))
        {
            return texture;
        }

        Debug.LogError("画像データのロードに失敗しました: " + path);
        return null;
    }

    

}

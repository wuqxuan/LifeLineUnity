using System.IO;
using SimpleJSON;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChatManager : MonoBehaviour
{
    //==============================================================================================
    // Fields
    public Queue<string> m_leftChats = new Queue<string>();
    public JSONNode status = new JSONClass();
    public JSONNode scenes = new JSONClass();
    public JSONNode choices = new JSONClass();
    private string status_savePath;
    public float m_timer = 0.0f;
    public bool m_isGameOver = false;
    public View m_view;


    //==============================================================================================
    // Methods

    void Awake()
    {
        Application.targetFrameRate = 10;
        m_view = FindObjectOfType(typeof(View)) as View;
    }
    void Start()
    {
        m_view.Initialize();
        LoadStoryData();
        m_view.m_startGameButton.onClick.AddListener(() => PlayGame());
        m_view.m_rePlayGameButton.onClick.AddListener(() => RePlayGame());
        status_savePath = Application.persistentDataPath + "/status.json";
    }

    void Update()
    {
        if (status["atScene"] != null)
        {
            ParseText.AtScene(this, status["atScene"]);     // set: status["atScene"] = null
        }
        else
        {
            if (m_isGameOver)
            {
                m_isGameOver = false;
                StartCoroutine(GameOver(0.3f));
            }
        }
        BubbleController.PopLeftChat(this);
    }

    public void PlayGame()
    {
        LoadStatusData();
        m_view.m_startGameButton.gameObject.SetActive(false);
        m_view.m_textBoxPanle.SetActive(true);
    }
    public void RePlayGame()
    {
        m_isGameOver = false;
        m_view.Initialize();
        m_view.m_rePlayGameButton.gameObject.SetActive(false);
        m_view.m_textBoxPanle.SetActive(true);
        SaveStatusData("Start");
    }

    IEnumerator GameOver(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_view.m_rePlayGameButton.gameObject.SetActive(true);
    }

    void LoadStoryData()
    {
        // 以下方式通过 Adroid, Mac 测试, iOS不通过
        // string choices_file_path = Resources.Load("Data/choices_cn").ToString();
        // string scenes_file_path = Resources.Load("Data/waypoints_cn").ToString();
        // 以下方式通过 Adroid, Mac，iOS测试
        TextAsset choices_file = Resources.Load("Data/choices_cn") as TextAsset;
        TextAsset scenes_file = Resources.Load("Data/scenes_cn") as TextAsset;
        // Debug.Log("scenes_file = " + scenes_file);

        // 将文本解析成 JSONNode
        choices = JSONNode.Parse(choices_file.text);
        scenes = JSONNode.Parse(scenes_file.text);
    }
    void LoadStatusData()
    {
        if (File.Exists(status_savePath))
        {
            status = JSONNode.LoadFromFile(status_savePath);
        }
        else
        {
            status["atScene"] = "Start";
        }
    }

    public void SaveStatusData(string scene)
    {
        status["atScene"] = scene;
        status.SaveToFile(status_savePath);
    }
}

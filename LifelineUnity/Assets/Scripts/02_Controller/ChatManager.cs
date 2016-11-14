
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using System.IO;
using System;
// using DG.Tweening;
public class ChatManager : MonoBehaviour
{
    //==============================================================================================
    // Fields
    private View m_view;
    public Queue<string> m_leftChats = new Queue<string>();
    private float m_timer = 0.0f;
    private const float mc_timerDuration = 1.5f;
    /// <summary> 右侧对话 </summary>
    private ChatObjectRight rightChat = new ChatObjectRight();
    /// <summary> 左侧对话 </summary>
    private ChatObjectLeft leftChat = new ChatObjectLeft();

    private JSONNode status = new JSONClass();
    private JSONNode scenes = new JSONClass();
    private JSONNode choices = new JSONClass();
    private string status_savePath;
    private string currentScene;
    

    //==============================================================================================
    // Methods

    void Awake()
    {
        Application.targetFrameRate = 10;
    }
    void Start()
    {
        status_savePath = Application.persistentDataPath + "/status111.json";
        m_view = FindObjectOfType(typeof(View)) as View;
        LoadStoryData();
        // LoadStatusData();
    }

    void Update()
    {
        if (status["atScene"] != null)
        {
            currentScene = status["atScene"];
            // TODO: 弹出 gameover 界面
            if (currentScene.Equals("gameover"))   // 不能直接用 if(status["atScene"].Equals("gameover"))
            {
                AtScene(currentScene);
                StartCoroutine(WaitForHideRePlayButton(3f));
                // SaveStatusData("Start");    // status["atScene"] = "Start"
            }
            else
            {
                AtScene(currentScene);     // set: status["atScene"] = null
            }
        }
        AutoPopLeftChat();
    }

    public void StartGame()
    {
         LoadStatusData();
         m_view.m_startGameButton.gameObject.SetActive(false);
    }
    public void RePlayGame()
    {
         SaveStatusData("Start");
         m_view.m_rePlayGameButton.gameObject.SetActive(false);
         m_view.Initialize();
    }

    IEnumerator WaitForHideRePlayButton(float duration)
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

    void SaveStatusData(string scene)
    {
        status["atScene"] = scene;
        status.SaveToFile(status_savePath);
    }

    void AtScene(string scene)
    {
        // Debug.Log("AtScene: scenes[scene] = " + scenes[scene].ToString());
        status["atScene"] = null;
        bool if_else = false;
        bool skip_line = false;
        JSONArray eachScene = scenes[scene].AsArray;
        for (int i = 0; i < eachScene.Count; i++)
        {
            // 转换成 string, 去掉首尾的 ‘“’ 
            string line = eachScene[i].ToString().Substring(1, eachScene[i].ToString().Length - 2);
            // Debug.Log("line = " + line);
            if (if_else)
            {
                if (line.StartsWith("<<else"))
                {
                    skip_line = !skip_line;
                    continue;
                }
                else if (line.Equals("<<endif>>"))
                {
                    if_else = false;
                    continue;
                }
                if (skip_line) continue;
            }
            if (line.StartsWith("<<if") || line.StartsWith("<<elseif"))
            {
                string LineWithNoTag = line.Substring(2, line.Length - 4);
                if_else = true;
                if (LineWithNoTag.Contains(" and "))
                {
                    // "gte" 和 “and” 同时出现的行
                    if (LineWithNoTag.Contains(" gte "))
                    {
                        string[] gteline = LineWithNoTag.Split(' ');
                        int valueOne = status[gteline[1].Substring(1)].AsInt;
                        string valueTwo = status[gteline[5].Substring(1)];
                        bool isgte = valueOne >= int.Parse(gteline[3]);
                        bool isTrueOrFalse = valueTwo.Equals(gteline[7]);

                        // Debug.Log("valueOne = " + valueOne + " valueTwo = " + valueTwo);
                        // Debug.Log("gteline[3]= " + gteline[3] + " gteline[7] = " + gteline[7]);
                        skip_line = !(isgte && isTrueOrFalse);
                        // Debug.Log("and gte skip_line  = " + skip_line);
                    }
                    else
                    {
                        string[] newline = LineWithNoTag.Split(' ');
                        string valueA = status[newline[1].Substring(1)];
                        string valueB = status[newline[5].Substring(1)];
                        // Debug.Log("valueA = " + valueA + " valueB = " + valueB);
                        // Debug.Log("newline[3] = " + newline[3] + " newline[7] = " + newline[7]);
                        skip_line = !(valueA.Equals(newline[3]) && valueB.Equals(newline[7]));
                        // Debug.Log("and skip_line = " + skip_line);
                    }
                }
                else if (LineWithNoTag.Contains(" or "))
                {
                    string[] newline = LineWithNoTag.Split(' ');
                    string valueA = status[newline[1].Substring(1)];
                    string valueB = status[newline[5].Substring(1)];

                    // Debug.Log("valueA = " + valueA + " valueB = " + valueB);
                    // Debug.Log("newline[3] = " + newline[3] + " newline[7] = " + newline[7]);
                    skip_line = !(valueA.Equals(newline[3]) || valueB.Equals(newline[7]));
                    // Debug.Log("and skip_line = " + skip_line);
                }
                else
                {
                    if (LineWithNoTag.Contains(" gte "))
                    {
                        string[] newline = LineWithNoTag.Split(' ');
                        int valueInt = status[newline[1].Substring(1)].AsInt;
                        skip_line = !(valueInt >= int.Parse(newline[3]));
                        // Debug.Log("skip_line = " + skip_line);
                    }
                    else
                    {
                        string[] newline = LineWithNoTag.Split(' ');
                        // Debug.Log("variable = " + status[newline[1].Substring(1)] + " + " + newline[3]);
                        // 下面两句不能写成skip_line = !(status[newline[1].Substring(1)].Equals(newline[3]))，会得到错误结果 </summary>
                        string value = status[newline[1].Substring(1)];
                        skip_line = !value.Equals(newline[3]);
                        // Debug.Log("skip_line = " + skip_line);
                    }
                }
            }
            else if (line.StartsWith("<<set")) HandleSet(line);
            else if (line.StartsWith("[[")) HandleJump(line);
            else if (line.StartsWith("<<category")) HandleChoice(line, scene);
            else HandleLeftChat(line);
        }
    }

    void HandleSet(string line)
    {
        string[] lines = line.Substring(7, line.Length - 9).Replace(" ", "").Split('=');
        if (lines[1].Contains("-1"))
        {
            int value = status[lines[0]].AsInt - 1;
            status[lines[0]] = value.ToString();
        }
        else status[lines[0]] = lines[1];
    }
    void HandleJump(string line)
    {
        string newLine = line.Substring(2, line.Length - 4);
        if (newLine.StartsWith("delay"))
        {
            string[] newLines = newLine.Split('|');
            status["atScene"] = newLines[1];
        }
        else status["atScene"] = newLine;
    }

    void HandleChoice(string line, string scene)
    {
        JSONArray choice = choices[int.Parse(line.Substring(19, line.Length - 21))]["actions"].AsArray;
        rightChat.Choose(m_view, new Dictionary<string, Action<string>> {
            // choiceButtonOne
            {choice[0]["choice"], message => {
                ActionFunction(choice, message, 0);
            }},
            // choiceButtonTwo
            {choice[1]["choice"], message => {
                ActionFunction(choice, message, 1);
            }}
         });
    }

    void ActionFunction(JSONArray choice, string message, int index)
    {
        string newScence = choice[index]["identifier"];
        rightChat.Say(m_view, message);
        status["atScene"] = newScence;
        SaveStatusData(newScence);
        // SaveStatusData(scene);
    }

    void HandleLeftChat(string line)
    {
        if (line.Contains("$pills") || line.Contains("$glowrods") || line.Contains("$power"))
        {
            // 替换$pills、$glowrods 和 $power
            string newLine = line.Replace("$pills", status["pills"]).Replace("$glowrods", status["glowrods"]).Replace("$power", status["power"]);
            leftChat.Say(this, newLine);
        }
        else leftChat.Say(this, line);
    }

    /// <summary> 弹出左侧对话 </summary>
    private void AutoPopLeftChat()
    {
        // 左侧有对话
        if (m_leftChats.Count > 0)
        {
            // 隐藏选择面板
            m_view.HideChoicePanel();
            m_timer += Time.deltaTime;
            // 若左侧对话不为空，且计时器时间到，继续下一句.
            while (m_leftChats.Count > 0 && m_timer >= mc_timerDuration)
            {
                // 左侧发送第一句话
                string leftChat = m_leftChats.Peek();
                m_view.PopBubble(m_view.m_BubblePrefab, leftChat, m_view.m_soundManager.m_leftAudio);
                // 删除第一句话
                m_leftChats.Dequeue();
                m_timer = 0f;
            }
        }
        // 左侧没有对话
        else
        {
            // 右侧没有对话
            if (m_view.m_isRightChatIsNull)
            {
                m_view.HideChoicePanel();
            }
            // 右侧有对话
            else
            {
                m_view.ShowChoicePanel(true);
            }
        }
    }
}

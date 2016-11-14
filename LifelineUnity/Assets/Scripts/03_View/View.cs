using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;

public class View : MonoBehaviour
{
    //==============================================================================================
    // Fields
    private ChatManager m_chatManager;
    public SoundManager m_soundManager;
    //======================================
    public Button m_startGameButton;
    public Button m_rePlayGameButton;
    // public Button m_BubblePrefab;
    public RectTransform m_chatContainer;

    private Text m_bubbleText;
    /// <summary> 对话气泡 </summary>
    private Button m_bubble;
    public List<Button> m_templateButton = new List<Button>();
    // public List<Button> m_popedButton = new List<Button>();
    /// <summary> TemplateText的索引 </summary>
    private int m_indexOfTemplateButton = 0;
    /// <summary> 已弹出的对话气泡 </summary>
    private List<Button> m_popedChatBubbles = new List<Button>();
    /// <summary> choices button </summary>
    public List<Button> m_choiceButtons = new List<Button>();
    /// <summary> 单行对话气泡高度（Button） </summary>
    private float m_chatBubbleHeight = 180f + m_gapBetweenBubble;
    /// <summary> 气泡间的间隙 </summary>
    private static float m_gapBetweenBubble = 10f;
    /// <summary> 已弹出对话的总高度 </summary>
    private float m_popedBubblesHeights = 0.0f;
    /// <summary> 新弹出对话气泡的posY </summary>
    private float m_newBubblePosY;
    /// <summary> 对话框底边的posY </summary>
    private float m_chatPanelBottomposY;
    /// <summary> 由下落转换到向上滚屏状态 </summary>
    private bool m_isFromFallToScrollUp = false;
    private string m_seletedButtonName = null;
    /// <summary> 右侧对话是否为空 </summary>
    public bool m_isRightChatIsNull = true;
    private const float mc_sizeDeltaY = 1100f;
    private const float mc_heights = 0f;
    private const float mc_firstBubblePosY = 350f;
    private const float mc_chatPanelBottomposY = -410f;
    /// <summary> 每行最大显示字符个数 </summary>
    private const int mc_charCountInPerLine = 18;

    //==============================================================================================
    // methods

    void Awake()
    {
        m_soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
        m_chatManager = FindObjectOfType(typeof(ChatManager)) as ChatManager;
    }
    void Start()
    {
        Initialize();
        m_startGameButton.onClick.AddListener(() => m_chatManager.StartGame());
        m_rePlayGameButton.onClick.AddListener(() => m_chatManager.RePlayGame());
        m_startGameButton.gameObject.GetComponentInChildren<Text>().text = "开始游戏";
        m_rePlayGameButton.gameObject.GetComponentInChildren<Text>().text = "重新开始游戏";
        m_rePlayGameButton.gameObject.SetActive(false);
    }


    // 删除聊天数据
    public void Initialize()
    {
        foreach (var popedBubble in m_popedChatBubbles)
        {
            Destroy(popedBubble.gameObject);
        }
        m_popedChatBubbles.Clear();
        m_newBubblePosY = mc_firstBubblePosY;
        m_popedBubblesHeights = mc_heights;
        m_chatContainer.sizeDelta = new Vector2(m_chatContainer.sizeDelta.x, mc_sizeDeltaY);
        m_chatPanelBottomposY = mc_chatPanelBottomposY;
    }

    /// <summary> 弹出对话 </summary>
    public void PopBubble(string message, AudioClip audioType)
    {
        // 获得用户选中的选择按钮的名字
        string currentButtonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        m_bubble = m_templateButton[m_indexOfTemplateButton];
        m_bubble.gameObject.SetActive(true);
        m_bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, GetNewBubblePosY(m_chatBubbleHeight));
        m_popedChatBubbles.Add(m_templateButton[m_indexOfTemplateButton]);
        m_soundManager.PlayMusic(audioType);
        m_bubbleText = m_bubble.GetComponentInChildren<Text>();

        HandleMessage(currentButtonName, message);
        m_indexOfTemplateButton += 1;
        if (m_templateButton.Count <= m_indexOfTemplateButton)
        {
            m_indexOfTemplateButton = 0;
        }
    }
    void HandleMessage(string buttonName, string message)
    {
        if (buttonName != m_seletedButtonName)
        {
            switch (buttonName)
            {
                case "ButtonOne":
                    m_bubbleText.color = new Color32(109, 153, 255, 255);
                    m_seletedButtonName = buttonName;
                    break;
                case "ButtonTwo":
                    m_bubbleText.color = new Color32(204, 214, 41, 255);
                    m_seletedButtonName = buttonName;
                    break;
                default:
                    m_bubbleText.color = new Color32(210, 210, 210, 255);
                    break;
            }
        }
        else
        {
            m_bubbleText.color = new Color32(210, 210, 210, 255);
        }
        // 聊天内容换行
        m_bubbleText.text = InsertWrap(message);
    }

    /// <summary> 隐藏消息选择面板 </summary>
    public void HideChoicePanel()
    {
        ShowChoicePanel(false);
    }

    /// <summary> 显示消息选择面板 </summary>
    public void ShowChoicePanel(bool isActive)
    {
        for (int i = 0; i < m_choiceButtons.Count; i++)
        {
            m_choiceButtons[i].gameObject.SetActive(isActive);
        }
    }
    /// <summary> 设置选择面板 </summary>
    public void SetChoice(Dictionary<string, Action<string>> choices)
    {
        List<string> keys = new List<string>(choices.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            int buttonIndex = i;
            if (choices.ContainsKey(keys[i]))
            {
                m_choiceButtons[i].GetComponentInChildren<Text>().text = keys[i];
                // 清空已绑定的方法.
                m_choiceButtons[i].onClick.RemoveAllListeners();
                // 给选择按钮绑定方法
                m_choiceButtons[i].onClick.AddListener(() => choices[keys[buttonIndex]](keys[buttonIndex]));
            }
        }

    }
    /// <summary> 聊天内容换行 </summary>
    private string InsertWrap(string message)
    {
        StringBuilder wrapMessage = new StringBuilder(message);
        for (int i = mc_charCountInPerLine; i < wrapMessage.Length; i += (mc_charCountInPerLine + 1))
        {
            wrapMessage.Insert(i, "\n");
        }
        return wrapMessage.ToString();
    }

    /// <summary> 计算新插入对话的Bubble的posY </summary>
    private float GetNewBubblePosY(float newBubbleHeight)
    {
        if (m_popedChatBubbles.Count < m_templateButton.Count)
        {
            m_newBubblePosY = mc_firstBubblePosY - m_popedBubblesHeights;
            m_popedBubblesHeights += newBubbleHeight;
        }
        // ------------------------------------------------------------ //
        // 下落结束，向上滚屏, 新Bubble插在对话框底端.
        else
        {
            m_isFromFallToScrollUp = true;
            CheckAndScrollBubbles(newBubbleHeight);
            m_popedChatBubbles.RemoveAt(0);
            // m_popedBubblesHeights -= (newBubbleHeight);
            m_newBubblePosY = m_chatPanelBottomposY;
        }
        return m_newBubblePosY;
    }
    /// <summary> 检查是否滚屏，并更新对话框的底边posY </summary>
    private void CheckAndScrollBubbles(float newBubbleHeight)
    {
        if (m_isFromFallToScrollUp)
        {
            // HeightenChatPanel(newBubbleHeight);
            ScrollUpBubbles(newBubbleHeight);
            // m_chatPanelBottomposY -= newBubbleHeight / 2;
        }
    }

    /// <summary> 向上移动各个Bubble，移动距离是新增Bubble（包括间距）的高度 </summary>
    private void ScrollUpBubbles(float step)
    {
        for (int i = 0; i < m_popedChatBubbles.Count; i++)
        {
            float posX = m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.x;
            float posY = m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.y;
            m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY + step);
        }
    }
}

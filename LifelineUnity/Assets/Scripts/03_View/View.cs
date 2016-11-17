using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text;

public class View : MonoBehaviour
{
    //==============================================================================================
    // Fields
    private SoundManager m_soundManager;
    public SoundManager m_SoundManager
    {
        get { return m_soundManager; }
    }

    //======================================

    public GameObject m_textBoxPanle;
    public GameObject m_choosePanle;
    public Button m_startGameButton;
    public Button m_rePlayGameButton;
    public List<Button> m_templateButton = new List<Button>();
    /// <summary> choices button </summary>
    public List<Button> m_choiceButtons = new List<Button>();

    //======================================

    private Text m_bubbleText;
    /// <summary> 对话气泡 </summary>
    private Button m_bubble;
    /// <summary> 已弹出的对话气泡 </summary>
    private List<Button> m_popedChatBubbles = new List<Button>();
    /// <summary> 新弹出对话气泡的posY </summary>
    private float m_newBubblePosY;
    /// <summary> 由下落转换到向上滚屏状态 </summary>
    private bool m_isFromFallToScrollUp = false;
    private bool m_isWaitingClick = false;
    /// <summary> 右侧是否有对话 </summary>
    private bool m_hasRightChat = true;
    public bool m_HasRightChat
    {
        get { return m_hasRightChat; }
        set { m_hasRightChat = value; }
    }
    private int m_indexOfTemplateButton;
    /// <summary> 已弹出对话的总高度 </summary>
    private float m_popedBubblesHeights;
    /// <summary> 对话框底边的posY </summary>
    private float m_chatPanelBottomposY;
    private const float POPED_BUBBLES_HEIGHT = 0f;
    private const float FIRST_BUBBLE_POSY = 350f;
    private const float CHAT_PANEL_BOTTOM_POSY = -410f;
    /// <summary> 气泡高度（Button） </summary>
    private const float BUBBLE_HEIGHT = 180f;
    /// <summary> 气泡间的间隙 </summary>
    private const float GAP_BETEEMN_BUBBLE = 10f;
    /// <summary> 每行最大显示字符个数 </summary>
    private const int MAX_CHAR_NUMBER_PER_LINE = 18;

    //==============================================================================================
    // methods

    void Awake()
    {
        m_soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
    }
    void Start()
    {
        Initialize();
        m_textBoxPanle.SetActive(false);
        m_startGameButton.gameObject.GetComponentInChildren<Text>().text = "开始游戏";
        m_rePlayGameButton.gameObject.GetComponentInChildren<Text>().text = "重新开始游戏";
    }
    // 初始化
    public void Initialize()
    {
        if (m_popedChatBubbles.Count != 0)
        {
            foreach (var bullble in m_popedChatBubbles)
            {
                bullble.gameObject.SetActive(false);
            }
        }
        m_rePlayGameButton.gameObject.SetActive(false);
        m_popedChatBubbles.Clear();
        m_newBubblePosY = FIRST_BUBBLE_POSY;
        m_popedBubblesHeights = POPED_BUBBLES_HEIGHT;
        m_chatPanelBottomposY = CHAT_PANEL_BOTTOM_POSY;
        m_indexOfTemplateButton = 0;
    }

    /// <summary> 弹出对话 </summary>
    public void PopBubble(string message, AudioClip audioType)
    {
        string currentButtonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; // 获得点击的按钮的名字
        m_bubble = m_templateButton[m_indexOfTemplateButton];
        m_bubble.gameObject.SetActive(true);
        m_soundManager.PlayMusic(audioType);
        HandleMessage(currentButtonName, message);
        m_bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, GetNewBubblePosY(BUBBLE_HEIGHT + GAP_BETEEMN_BUBBLE));
        UpdateBubble();
    }
    void UpdateBubble()
    {
        m_popedChatBubbles.Add(m_templateButton[m_indexOfTemplateButton]);
        m_indexOfTemplateButton += 1;
        if (m_indexOfTemplateButton >= m_templateButton.Count)
        {
            m_indexOfTemplateButton = 0;
        }
    }
    void HandleMessage(string buttonName, string message)
    {
        m_bubbleText = m_bubble.GetComponentInChildren<Text>();
        if (m_isWaitingClick)
        {
            switch (buttonName)
            {
                case "ButtonOne":
                    m_bubbleText.color = new Color32(109, 153, 255, 255);   // 蓝色
                    break;
                case "ButtonTwo":
                    m_bubbleText.color = new Color32(204, 214, 41, 255);    // 黄色
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

    /// <summary> 隐藏选择面板 </summary>
    public void HideChoicePanel()
    {
        m_choosePanle.SetActive(false);
        m_isWaitingClick = false;
    }

    /// <summary> 显示选择面板 </summary>
    public void ShowChoicePanel()
    {
        m_choosePanle.SetActive(true);
        m_isWaitingClick = true;
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
        for (int i = MAX_CHAR_NUMBER_PER_LINE; i < wrapMessage.Length; i += (MAX_CHAR_NUMBER_PER_LINE + 1))
        {
            wrapMessage.Insert(i, "\n");
        }
        return wrapMessage.ToString();
    }

    /// <summary> 计算新插入的Bubble的posY </summary>
    private float GetNewBubblePosY(float newBubbleHeight)
    {
        if (m_popedChatBubbles.Count < m_templateButton.Count)
        {
            m_newBubblePosY = FIRST_BUBBLE_POSY - m_popedBubblesHeights;
            m_popedBubblesHeights += newBubbleHeight;
        }
        // ------------------------------------------------------------ //
        // 下落结束，向上滚屏, 新Bubble插在对话框底端.
        else
        {
            m_isFromFallToScrollUp = true;
            ScrollUpBubbles(newBubbleHeight);
            m_popedChatBubbles.RemoveAt(0);
            m_newBubblePosY = m_chatPanelBottomposY;
        }
        return m_newBubblePosY;
    }
    /// <summary> 向上移动各个Bubble，移动距离是新增Bubble（包括间距）的高度 </summary>
    private void ScrollUpBubbles(float height)
    {
        if (m_isFromFallToScrollUp)
        {
            for (int i = 0; i < m_popedChatBubbles.Count; i++)
            {
                float posX = m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.x;
                float posY = m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.y;
                m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY + height);
            }
        }
    }
}

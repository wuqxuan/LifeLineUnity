using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    public Text m_bubbleText;
    /// <summary> 对话气泡 </summary>
    public Button m_bubble;
    /// <summary> 已弹出的对话气泡 </summary>
    public List<Button> m_popedChatBubbles = new List<Button>();
    /// <summary> 新弹出对话气泡的posY </summary>
    public float m_newBubblePosY;
    /// <summary> 由下落转换到向上滚屏状态 </summary>
    public bool m_isFromFallToScrollUp = false;
    public bool m_isWaitingClick = false;
    /// <summary> 右侧是否有对话 </summary>
    private bool m_hasRightChat = true;
    public bool m_HasRightChat
    {
        get { return m_hasRightChat; }
        set { m_hasRightChat = value; }
    }
    public int m_indexOfTemplateButton;
    /// <summary> 已弹出对话的总高度 </summary>
    public float m_popedBubblesHeights;
    /// <summary> 对话框底边的posY </summary>
    public float m_chatPanelBottomposY;
    private const float POPED_BUBBLES_HEIGHT = 0f;
    private const float FIRST_BUBBLE_POSY = 350f;
    private const float CHAT_PANEL_BOTTOM_POSY = -410f;

    //==============================================================================================
    // methods
    public void Initialize()
    {
        if (m_popedChatBubbles.Count != 0)
        {
            foreach (var bullble in m_popedChatBubbles)
            {
                bullble.gameObject.SetActive(false);
            }
        }
        m_textBoxPanle.SetActive(false);
        m_rePlayGameButton.gameObject.SetActive(false);
        m_popedChatBubbles.Clear();
        m_newBubblePosY = FIRST_BUBBLE_POSY;
        m_popedBubblesHeights = POPED_BUBBLES_HEIGHT;
        m_chatPanelBottomposY = CHAT_PANEL_BOTTOM_POSY;
        m_indexOfTemplateButton = 0;
        m_startGameButton.gameObject.GetComponentInChildren<Text>().text = "开始游戏";
        m_rePlayGameButton.gameObject.GetComponentInChildren<Text>().text = "重新开始游戏";
        m_soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
    }
}

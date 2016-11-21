using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary> 对话控制  </summary>
public static class BubbleController
{
    /// <summary> 弹出对话 </summary>
    public static void PopBubble(View view, string message, AudioClip audioType)
    {
        float BUBBLE_HEIGHT = 180f;	    // 气泡高度
        float GAP_BETEEMN_BUBBLE = 10f;	// 气泡间的间隙
        string currentButtonName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name; // 获得点击的按钮的名字
        view.m_bubble = view.m_templateButton[view.m_indexOfTemplateButton];
        view.m_bubble.gameObject.SetActive(true);
        view.m_SoundManager.PlayMusic(audioType);
        HandleMessage(view, currentButtonName, message);
        view.m_bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, GetNewBubblePosY(view, BUBBLE_HEIGHT + GAP_BETEEMN_BUBBLE));
        UpdateBubble(view);
    }
    private static void UpdateBubble(View view)
    {
        view.m_popedChatBubbles.Add(view.m_templateButton[view.m_indexOfTemplateButton]);
        view.m_indexOfTemplateButton += 1;
        if (view.m_indexOfTemplateButton >= view.m_templateButton.Count)
        {
            view.m_indexOfTemplateButton = 0;
        }
    }
    private static void HandleMessage(View view, string buttonName, string message)
    {
        view.m_bubbleText = view.m_bubble.GetComponentInChildren<Text>();
        if (view.m_isWaitingClick)
        {
            switch (buttonName)
            {
                case "ButtonOne":
                    view.m_bubbleText.color = new Color32(109, 153, 255, 255);   // 蓝色
                    break;
                case "ButtonTwo":
                    view.m_bubbleText.color = new Color32(204, 214, 41, 255);    // 黄色
                    break;
                default:
                    view.m_bubbleText.color = new Color32(210, 210, 210, 255);
                    break;
            }
        }
        else
        {
            view.m_bubbleText.color = new Color32(210, 210, 210, 255);
        }
        // 聊天内容换行
        view.m_bubbleText.text = InsertWrap(message);
    }

    /// <summary> 聊天内容换行 </summary>
    private static string InsertWrap(string message)
    {
        int MAX_CHAR_NUMBER_PER_LINE = 18;
        StringBuilder wrapMessage = new StringBuilder(message);
        for (int i = MAX_CHAR_NUMBER_PER_LINE; i < wrapMessage.Length; i += (MAX_CHAR_NUMBER_PER_LINE + 1))
        {
            wrapMessage.Insert(i, "\n");
        }
        return wrapMessage.ToString();
    }

    /// <summary> 计算新插入的Bubble的posY </summary>
    private static float GetNewBubblePosY(View view, float newBubbleHeight)
    {
        float FIRST_BUBBLE_POSY = 350f;
        if (view.m_popedChatBubbles.Count < view.m_templateButton.Count)
        {
            view.m_newBubblePosY = FIRST_BUBBLE_POSY - view.m_popedBubblesHeights;
            view.m_popedBubblesHeights += newBubbleHeight;
        }
        // ------------------------------------------------------------ //
        // 下落结束，向上滚屏, 新Bubble插在对话框底端.
        else
        {
            view.m_isFromFallToScrollUp = true;
            ScrollUpBubbles(view, newBubbleHeight);
            view.m_popedChatBubbles.RemoveAt(0);
            view.m_newBubblePosY = view.m_chatPanelBottomposY;
        }
        return view.m_newBubblePosY;
    }
    /// <summary> 向上移动各个Bubble，移动距离是新增Bubble（包括间距）的高度 </summary>
    private static void ScrollUpBubbles(View view, float height)
    {
        if (view.m_isFromFallToScrollUp)
        {
            for (int i = 0; i < view.m_popedChatBubbles.Count; i++)
            {
                float posX = view.m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.x;
                float posY = view.m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition.y;
                view.m_popedChatBubbles[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY + height);
            }
        }
    }

    /// <summary> 弹出左侧对话 </summary>
    public static void PopLeftChat(ChatManager cm)
    {
        // 左侧有对话
        if (cm.m_leftChats.Count > 0)
        {
            // 隐藏选择面板
            ChoicePanle.HideChoicePanel(cm.m_view);
            cm.m_timer += Time.deltaTime;
            // 若左侧对话不为空，且计时器时间到，继续下一句.
            while (cm.m_leftChats.Count > 0 && cm.m_timer >= 1.5f)
            {
                string leftChat = cm.m_leftChats.Peek();
                if (leftChat.Equals("游戏结束")) cm.m_isGameOver = true;
                BubbleController.PopBubble(cm.m_view, leftChat, cm.m_view.m_SoundManager.m_leftAudio);
                cm.m_leftChats.Dequeue();
                cm.m_timer = 0f;
            }
        }
        // 左侧没有对话
        else
        {
            if (cm.m_view.m_HasRightChat)
                ChoicePanle.HideChoicePanel(cm.m_view);     // 右侧没有对话
            else
                ChoicePanle.ShowChoicePanel(cm.m_view);     // 右侧有对话
        }
    }
}

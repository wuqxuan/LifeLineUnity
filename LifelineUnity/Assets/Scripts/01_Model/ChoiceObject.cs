using System;
using UnityEngine;
using System.Collections.Generic;

public static class ChoiceObject
{
    /// <summary> 右侧对话，没有等待时间 </summary>
    public static void ReplayMessage(View view, string message)
    {
        // 弹出新对话
        view.PopBubble(message, view.m_SoundManager.m_rightAudio);
        // 隐藏选择面板
        view.HideChoicePanel();
        view.m_HasRightChat = true;
    }
    public static void SetChoiceButton(View view, Dictionary<string, Action<string>> m_choices)
    {
        if (m_choices.Keys.Count == 2)
        {
            view.m_HasRightChat = false;
            view.SetChoice(m_choices);
        }
        else
        {
            Debug.LogError("选择项数量不为2");
        }
    }
}



using System;
using UnityEngine.UI;
using System.Collections.Generic;

public static class ChoicePanle
{

    /// <summary> 隐藏选择面板 </summary>
    public static void HideChoicePanel(View view)
    {
        view.m_choosePanle.SetActive(false);
        view.m_isWaitingClick = false;
    }

    /// <summary> 显示选择面板 </summary>
    public static void ShowChoicePanel(View view)
    {
        view.m_choosePanle.SetActive(true);
        view.m_isWaitingClick = true;
    }
    /// <summary> 设置选择面板 </summary>
    public static void SetChoice(View view, Dictionary<string, Action<string>> choices)
    {
        List<string> keys = new List<string>(choices.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            int buttonIndex = i;
            if (choices.ContainsKey(keys[i]))
            {
                view.m_choiceButtons[i].GetComponentInChildren<Text>().text = keys[i];
                // 清空已绑定的方法.
                view.m_choiceButtons[i].onClick.RemoveAllListeners();
                // 给选择按钮绑定方法
                view.m_choiceButtons[i].onClick.AddListener(() => choices[keys[buttonIndex]](keys[buttonIndex]));
            }
        }

    }
}

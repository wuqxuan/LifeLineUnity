using SimpleJSON;
using System;
using System.Collections.Generic;
public static class ParseText
{
    public static void AtScene(ChatManager cm, string scene)
    {
        bool if_else = false;
        bool skip_line = false;
        // Debug.Log("AtScene: scenes[scene] = " + scenes[scene].ToString());
        cm.status["atScene"] = null;
        JSONArray sceneItem = cm.scenes[scene].AsArray;
        for (int i = 0; i < sceneItem.Count; i++)
        {
            // 转换成 string, 去掉首尾的 “
            string line = sceneItem[i].ToString().Substring(1, sceneItem[i].ToString().Length - 2);
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
                if_else = true;
                string LineWithNoTag = line.Substring(2, line.Length - 4);
                if (LineWithNoTag.Contains(" and "))
                {
                    skip_line = AndOrInLine.and_InLine(cm, LineWithNoTag);
                }
                else if (LineWithNoTag.Contains(" or "))
                {
                    skip_line = AndOrInLine.or_InLine(cm, LineWithNoTag);
                }
                else
                {
                    skip_line = AndOrInLine.and_or_NotInLine(cm, LineWithNoTag);
                }
            }
            else if (line.StartsWith("<<set")) ParseText.HandleSet(cm, line);
            else if (line.StartsWith("[[")) ParseText.ToNewScene(cm, line);
            else if (line.StartsWith("<<category")) ParseText.HandleChoice(cm, line, scene);
            else ParseText.AddLeftChats(cm, line);
        }
    }
    static void HandleSet(ChatManager cm, string line)
    {
        string[] lines = line.Substring(7, line.Length - 9).Replace(" ", "").Split('=');
        if (lines[1].Contains("-1"))
        {
            int value = cm.status[lines[0]].AsInt - 1;
            cm.status[lines[0]] = value.ToString();
        }
        else cm.status[lines[0]] = lines[1];
    }
    static void ToNewScene(ChatManager cm, string line)
    {
        string newLine = line.Substring(2, line.Length - 4);
        if (newLine.StartsWith("delay"))
        {
            string[] newLines = newLine.Split('|');
            cm.status["atScene"] = newLines[1];
        }
        else cm.status["atScene"] = newLine;
    }

    static void HandleChoice(ChatManager cm, string line, string scene)
    {
        JSONArray choice = cm.choices[int.Parse(line.Substring(19, line.Length - 21))]["actions"].AsArray;
        ChoiceObject.SetChoiceButton(cm.m_view, new Dictionary<string, Action<string>> {
            // choiceButtonOne
            {choice[0]["choice"], message => {
                ActionFunction(cm, choice, message, 0);
            }},
            // choiceButtonTwo
            {choice[1]["choice"], message => {
                ActionFunction(cm, choice, message, 1);
            }}
         });
    }

    static void ActionFunction(ChatManager cm, JSONArray choice, string message, int index)
    {
        string newScence = choice[index]["identifier"];
        ChoiceObject.ReplayMessage(cm.m_view, message);
        cm.status["atScene"] = newScence;
        cm.SaveStatusData(newScence);
        // SaveStatusData(scene);
    }

    static void AddLeftChats(ChatManager cm, string line)
    {
        if (line.Contains("$pills") || line.Contains("$glowrods") || line.Contains("$power"))
        {
            // 替换$pills、$glowrods 和 $power
            string newLine = line.Replace("$pills", cm.status["pills"]).Replace("$glowrods", cm.status["glowrods"]).Replace("$power", cm.status["power"]);
            cm.m_leftChats.Enqueue(newLine);
        }
        else cm.m_leftChats.Enqueue(line);
    }
}


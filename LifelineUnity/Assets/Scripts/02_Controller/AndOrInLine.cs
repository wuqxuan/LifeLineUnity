/// <summary> "and" "or" in Line </summary>
using UnityEngine;
public static class AndOrInLine
{
    public static bool and_InLine(ChatManager cm, string line)
    {
        string[] newline = line.Split(' ');
        // "gte" 和 “and” 同时出现的行
        if (line.Contains(" gte "))
        {
            int valueA = cm.status[newline[1].Substring(1)].AsInt;
            string valueB = cm.status[newline[5].Substring(1)];
            bool isgte = valueA >= int.Parse(newline[3]);
            bool isTrueOrFalse = valueB.Equals(newline[7]);

            // Debug.Log("valueA = " + valueA + " valueB = " + valueB);
            // Debug.Log("gteline[3]= " + gteline[3] + " gteline[7] = " + gteline[7]);
            return !(isgte && isTrueOrFalse);
            // Debug.Log("and gte skip_line  = " + skip_line);
        }
        else
        {
            string valueA = cm.status[newline[1].Substring(1)];
            string valueB = cm.status[newline[5].Substring(1)];
            // Debug.Log("valueA = " + valueA + " valueB = " + valueB);
            // Debug.Log("newline[3] = " + newline[3] + " newline[7] = " + newline[7]);
            return !(valueA.Equals(newline[3]) && valueB.Equals(newline[7]));
            // Debug.Log("and skip_line = " + skip_line);
        }
    }

    public static bool or_InLine(ChatManager cm, string line)
    {
        string[] newline = line.Split(' ');
        string valueA = cm.status[newline[1].Substring(1)];
        string valueB = cm.status[newline[5].Substring(1)];

        // Debug.Log("valueA = " + valueA + " valueB = " + valueB);
        // Debug.Log("newline[3] = " + newline[3] + " newline[7] = " + newline[7]);
        return !(valueA.Equals(newline[3]) || valueB.Equals(newline[7]));
        // Debug.Log("and skip_line = " + skip_line);
    }
    public static bool and_or_NotInLine(ChatManager cm, string line)
    {
        string[] newline = line.Split(' ');
        if (line.Contains(" gte "))
        {
            int valueA = cm.status[newline[1].Substring(1)].AsInt;
            return !(valueA >= int.Parse(newline[3]));
            // Debug.Log("skip_line = " + skip_line);
        }
        else
        {
            // Debug.Log("variable = " + status[newline[1].Substring(1)] + " + " + newline[3]);
            // 下面两句不能写成skip_line = !(status[newline[1].Substring(1)].Equals(newline[3]))，会得到错误结果 </summary>
            string value = cm.status[newline[1].Substring(1)];
            return !value.Equals(newline[3]);
            // Debug.Log("skip_line = " + skip_line);
        }
    }
}


using UnityEngine;

namespace Jphoooo.Tools.Runtimes
{
    public static class ToolKit
    {
        public static string WithColor(this string str, Color col)
        {
            string hex = ColorUtility.ToHtmlStringRGB(col);
            return $"<color=#{hex}>{str}</color>";
        }
    }

 
}


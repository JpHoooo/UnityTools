using UnityEngine;
namespace Jphoooo.Tools{
    public static class RichText
    {
        public static string WithColor(this string str, Color col)
        {
            string hex = ColorUtility.ToHtmlStringRGB(col);
            return $"<color=#{hex}>{str}</color>";
        }

        public static string WithBold(this string str){
            return $"<b>{str}</b>";
        }
    }
}



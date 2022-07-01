using UnityEngine;
namespace Jphoooo.JpTools
{
   public static class StringExtensions
    {
        public static string WithColor(this string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
        }
    }
}


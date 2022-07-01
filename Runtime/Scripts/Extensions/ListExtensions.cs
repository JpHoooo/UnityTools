using UnityEngine;
using System.Collections.Generic;

namespace Jphoooo.JpTools
{
    public static class ListExtensions
    {
        public static T First<T>(this List<T> list) => list[0];
        public static T Last<T>(this List<T> list) => list[list.Count - 1];
    }
}


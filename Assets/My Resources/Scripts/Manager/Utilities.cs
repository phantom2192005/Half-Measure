using System;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static string EnumToString(Enum @enum)
    {
        return System.Text.RegularExpressions.Regex.Replace(@enum.ToString(), "([a-z])([A-Z])", "$1 $2");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAStringUtility
{
    public static string StripNonAlphabeticSuffix(string s)
    {
        int lastLetter = s.Length - 1;
        for (; lastLetter >= 0; lastLetter--)
        {
            if (char.IsLetter(s[lastLetter]))
            {
                break;
            }
        }

        return s.Substring(0, lastLetter + 1);
    }
}

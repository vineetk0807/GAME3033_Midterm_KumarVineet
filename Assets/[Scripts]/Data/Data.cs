using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// A static data script which is for managing data during gameplay
/// </summary>
public class Data : MonoBehaviour
{
    // All data
    public static int Score = 0;
    public static bool isVictory = false;

    public static void Reset()
    {
        Score = 0;
        isVictory = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{

    [SerializeField] 
    private TextMeshProUGUI GameResult;
    [SerializeField] 
    private TextMeshProUGUI TotalScore;
    // Start is called before the first frame update
    void Start()
    {
        if (Data.isVictory)
        {
            GameResult.text = "You have Won!";
        }
        else
        {
            if (Data.isTimeOut)
            {
                GameResult.text = "Time Up! :(";
            }
            else
            {
                GameResult.text = "You Lost :(";
            }
        }

        TotalScore.text = Data.Score.ToString();
    }
}

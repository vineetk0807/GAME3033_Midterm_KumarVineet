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

    public Animator TyAnimator;

    public readonly int isWon = Animator.StringToHash("IsWon");
    public readonly int isLost = Animator.StringToHash("IsLost");

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.GetInstance().PlaySceneTrack(AudioManager.MusicTrack.BGM_EndScene,0.1f,0.2f);

        Cursor.visible = true;

        if (Data.isVictory)
        {
            TyAnimator.SetBool(isWon,true);
            TyAnimator.SetBool(isLost,false);
            GameResult.text = "You have Won!";
        }
        else
        {
            TyAnimator.gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0f,90f,0f));
            TyAnimator.SetBool(isLost, true);
            TyAnimator.SetBool(isWon, false);
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

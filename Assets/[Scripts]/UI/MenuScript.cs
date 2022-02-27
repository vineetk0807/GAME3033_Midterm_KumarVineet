using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Image star;
    public Image redTile;
    public Image greenTile;
    public Image blueTile;

    public Animator TyAnimator;

    public readonly int isInMainMenuHash = Animator.StringToHash("IsInMainMenu");

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        TyAnimator.SetBool(isInMainMenuHash, true);

    }

    // Update is called once per frame
    void Update()
    {
        if (star.gameObject.activeSelf)
        {
            star.rectTransform.Rotate(new Vector3(0f, 0f, 180 * Time.deltaTime), Space.Self);
        }

        if(redTile.gameObject.activeSelf)
        {
            redTile.rectTransform.Rotate(new Vector3(0f, 180 * Time.deltaTime, 0f), Space.Self);
        }

        if (greenTile.gameObject.activeSelf)
        {
            greenTile.rectTransform.Rotate(new Vector3(0f, 180 * Time.deltaTime, 0f), Space.Self);
        }

        if (blueTile.gameObject.activeSelf)
        {
            blueTile.rectTransform.Rotate(new Vector3(0f, 180 * Time.deltaTime, 0f), Space.Self);
        }
    }
}

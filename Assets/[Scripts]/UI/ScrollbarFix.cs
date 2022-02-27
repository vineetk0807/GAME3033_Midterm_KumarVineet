using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarFix : MonoBehaviour
{
    public Scrollbar scrollbarValueFix;


    // Start is called before the first frame update
    void Start()
    {
        scrollbarValueFix.value = 1;
    }

    private void OnEnable()
    {
        scrollbarValueFix.value = 1;
    }

    private void Awake()
    {
        scrollbarValueFix.value = 1;
    }
}

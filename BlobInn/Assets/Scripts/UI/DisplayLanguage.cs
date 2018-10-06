﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLanguage : DisplayGroup {

    public static DisplayLanguage Instance;

    public Transform[] buttons;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
    }

    public void SetLanguage(int i)
    {
        Tween.Bounce(buttons[i]);

        Inventory.currentLanguageType = (Inventory.LanguageType)i;

        Inventory.Instance.Save();

        Close();
    }

}
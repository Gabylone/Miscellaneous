﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;
using System;
using System.IO;

public class Transition : MonoBehaviour {

    public static Transition Instance;

    public GameObject group;

    public Image image;

    public bool clearOnStart = true;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        if ( clearOnStart)
        {
            Show();
            Invoke("StartDelay" , 0.5f);
        }
        else
        {
            Hide();

        }
    }

    void StartDelay()
    {
        Clear(1f);
    }

    public void Fade(float duration)
    {
        Show();

        image.color = Color.clear;
        HOTween.To( image , duration , "color" , Color.black);
    }
    public void Clear(float duration)
    {
        Show();
        image.color = Color.black;
        HOTween.To(image, duration, "color", Color.clear);

        Invoke("Hide", duration);
    }

    void Show()
    {
        group.SetActive(true);
    }

    void Hide()
    {
        group.SetActive(false);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnterGame : MonoBehaviour
{
    public static UIEnterGame Current;

    [SerializeField]
    private Button _btnEnter;

    [SerializeField]
    private GameObject _objEnter;

    [SerializeField]
    private bool _isLoadedOnce;

    private void Awake()
    {
        if (!Current)
        {
            Current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            _objEnter.SetActive(false);
        }
    }
    private void Start()
    {
        _btnEnter.onClick.AddListener(BtnEnter_OnClick);
    }

    private void BtnEnter_OnClick()
    {
        _isLoadedOnce = true;
        GameMusicPlayer.Current.PlayMusic("event:/Music/PlayMenu");
        _objEnter.SetActive(false);
    }
}

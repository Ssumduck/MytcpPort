using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour, IPointerClickHandler
{
    public enum ClickType
    {
        NONE,
        SetActive
    }

    [SerializeField]
    ClickType type;
    [SerializeField]
    SoundManager.AudioType audioType;
    [SerializeField]
    string audioName;
    [SerializeField]
    GameObject go;

    void ClickSound()
    {
        Managers.Sound.SoundPlayer(audioType, audioName);
    }

    public void ClickSetActive()
    {
        go.SetActive(!go.activeSelf);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClickSound();

        switch (type)
        {
            case ClickType.NONE:
                break;
            case ClickType.SetActive:
                ClickSetActive();
                break;
        }
    }
}

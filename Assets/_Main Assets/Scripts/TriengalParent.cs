using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPHive.Core;
using MoreMountains.NiceVibrations;
public class TriengalParent : MonoBehaviour
{

    [HideInInspector] public bool makeJustOne = false;
    void Update()
    {
        if (transform.childCount == 0 && makeJustOne)
        {
            makeJustOne = false;
            transform.DOKill();
            MMVibrationManager.Haptic(HapticTypes.Success);
            GameManager.Instance.WinLevel();
        }

    }
}

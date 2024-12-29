using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPHive.Core;

public class WinConitionScript : MonoBehaviour
{
    [HideInInspector] public int gaintCount, deathGaint;
    bool firstTime = false;
    private void Update()
    {
        if (!firstTime && gaintCount <= deathGaint && gaintCount != 0)
        {
            firstTime = true;
            GameManager.Instance.WinLevel();
        }

    }
}

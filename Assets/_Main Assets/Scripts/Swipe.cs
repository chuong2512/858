using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPHive.Core;
using UnityEngine.EventSystems;

public class Swipe : Singleton<Swipe>
{
    private bool tap,swipeDown,swipeUp,swipeRight,swipeLeft;
    private bool surukle=false;
    private Vector2 startTouch,swipeStart;

    public float deadZone;

    public void SwipeMeth()
    {
        tap=true;
        surukle=true;

        if(Input.GetMouseButton(0)) startTouch=(Vector2)Input.mousePosition;
    }
    public bool canSwipe=false;
    void Update()
    {
        if(canSwipe && !EventSystem.current.IsPointerOverGameObject())
        {
            if(Input.GetMouseButtonDown(0))
            {
                tap=true;
                surukle=true;

                startTouch=(Vector2)Input.mousePosition;
            }

            if(Input.GetMouseButtonUp(0))
            {
                surukle=false;
                Reset();
            }

            //Mesafe Hesabı
            swipeStart=Vector2.zero;
            if(surukle)
            {
                if(Input.GetMouseButton(0)) swipeStart=(Vector2)Input.mousePosition-startTouch;
            }
            else
            {
                swipeUp=false;
                swipeDown=false;
                swipeLeft=false;
                swipeRight=false;
            }

            // Threshold
            if(swipeStart.magnitude>deadZone) //değeri sen belirle   , fazlalık
            {
                float x=swipeStart.x;
                float y=swipeStart.y;
                if(Mathf.Abs(x)>Mathf.Abs(y))
                {
                    if(x<0) swipeLeft=true;
                    else swipeRight=true;
                }
                else
                {
                    if(y<0) swipeDown=true;
                    else swipeUp=true;
                }

                Reset();
            }
        }

    }

    public void Reset()
    {
        surukle=false;
        swipeStart=swipeStart=Vector2.zero; //Sıfırla
    }

    public bool SwipeDown
    {
        get { return swipeDown; }
    }
    public bool SwipeUp
    {
        get { return swipeUp; }
    }
    public bool SwipeRight
    {
        get { return swipeRight; }
    }
    public bool SwipeLeft
    {
        get { return swipeLeft; }
    }
    public Vector2 SwipeStart
    {
        get { return swipeStart; }
    }


}

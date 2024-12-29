using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPHive.Core;

public class StageOppener : MonoBehaviour
{

    public List<Line> Line = new List<Line>();

    private void OnEnable()
    {
        StartCoroutine(OpenAll());
    }


    IEnumerator OpenAll()
    {
        Swipe.Instance.canSwipe = false;
        foreach (var item in Line)
        {

            foreach (var item2 in item.Colmn)
            {
                item2.SetActive(false);
            }
        }

        foreach (var item in Line)
        {
            foreach (var item2 in item.Colmn)
            {
                item2.SetActive(true);
                item2.transform.DOPunchScale(item2.transform.localScale / 2, .023f);

            }
            yield return new WaitForSeconds(.024f);
        }

        Swipe.Instance.canSwipe = true;
    }

}

[System.Serializable]
public class Line
{
    public List<GameObject> Colmn = new List<GameObject>();
}

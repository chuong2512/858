using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using GPHive.Core;
using GPHive.Game;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject stickmanPrefeb;
    [SerializeField] bool hasStickman;
    public bool HasStickman { get { return hasStickman; } }

    CollectiableChar stickmanOnBox;
    public CollectiableChar StickmanOnBox { get { return stickmanOnBox; } }

    [ShowIf("hasStickman")]
    [SerializeField] LevelSettings.ColorEnum stickmanColor;




    private void Start()
    {
        if (hasStickman)
        {
            GameObject _stickman = Instantiate(stickmanPrefeb);
            _stickman.transform.position = transform.position + Vector3.up * 1.5f;
            _stickman.SetActive(true);

            stickmanOnBox = _stickman.GetComponent<CollectiableChar>();
            stickmanOnBox.myColor = stickmanColor;
            _stickman.transform.SetParent(LevelManager.Instance.ActiveLevel.transform);
        }
    }

    private void OnDrawGizmos()
    {
        if (hasStickman)
        {
            Color _gizmosColor = LevelSettings.Instance.colors[(int)stickmanColor];
            Gizmos.color = _gizmosColor;
            Gizmos.DrawCube(transform.position + Vector3.up * 1.5f, Vector3.one);

        }
    }

    public void CollectStickman()
    {
        hasStickman = false;
    }

}

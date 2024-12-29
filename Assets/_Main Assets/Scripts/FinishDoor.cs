using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPHive.Core;

public class FinishDoor : MonoBehaviour
{
    public LevelSettings.ColorEnum myColor;
    [SerializeField] MeshRenderer meshRenderer;

    public Transform transformMovingPos;
    public Animator animator;

    private void Start()
    {
        meshRenderer.materials[1].color = LevelSettings.Instance.colors[(int)myColor];
        meshRenderer.materials[0].SetColor("_Color", LevelSettings.Instance.colors[(int)myColor]);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPHive.Core;
public class Door : MonoBehaviour
{
    public LevelSettings.ColorEnum myColor;
    public ParticleSystem enterDoorParticle;
    Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetColor("_Color", LevelSettings.Instance.colors[(int)myColor]);
        enterDoorParticle.startColor = (LevelSettings.Instance.colors[(int)myColor]);
    }
}

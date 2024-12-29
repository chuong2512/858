using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPHive.Core;
using GPHive.Game;
public class CollectiableChar : MonoBehaviour
{
    public LevelSettings.ColorEnum myColor;
    [SerializeField] LayerMask platformLayer;
    public float movmentSpeed;
    public ParticleSystem trailParticle;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Animator animator;
    // public List<Transform> touchingPoses = new List<Transform>();
    [HideInInspector] public List<Vector3> movingPoses = new List<Vector3>();
    public Transform rotation;

    [HideInInspector] public bool takePose = true, didPlayerPunch = true, levelEnd = false, IdleKiller = false;
    private void Start()
    {
        skinnedMeshRenderer.material.SetColor("_BaseColor", LevelSettings.Instance.colors[(int)myColor]);
    }


    private bool moving;
    public void Move()
    {
        if (!levelEnd && !moving)
        {
            if (movingPoses.Count > 0)
            {
                if (didPlayerPunch)
                {
                    animator.SetTrigger("Run");
                    didPlayerPunch = false;
                }
                moving = true;

                StartCoroutine(CO_MoveCharacter(movingPoses[0]));

            }
            else if (!IdleKiller)
                animator.SetTrigger("Idle");

        }
    }


    IEnumerator CO_MoveCharacter(Vector3 movingPos)
    {
        animator.SetTrigger("Run");

        if (rotation != null)
            transform.LookAt(rotation.position, Vector3.up);

        var _time = 0f;
        var _travelTime = Vector3.Distance(transform.position, movingPos) / movmentSpeed;
        var _startPos = transform.position;
        while (_time < _travelTime)
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, movingPos, _time / _travelTime);
            yield return new WaitForFixedUpdate();
        }

        if (rotation != null)
            transform.LookAt(rotation.position, Vector3.up);

        transform.position = movingPos;
        moving = false;
        rotation = null;

        movingPoses.RemoveAt(0);



        if (movingPoses.Count <= 0)
        {

            if (!IdleKiller)
            {
                animator.ResetAll();
                animator.SetTrigger("Idle");
            }

            yield break;
        }

        Move();

    }

    private void OnTriggerEnter(Collider other)
    {
        /* if (other.CompareTag("Platform"))
         {
             if (!touchingPoses.Contains(other.transform))
             {
                 touchingPoses.Add(other.transform);
             }
         }*/
        if (other.CompareTag("Door"))
        {
            Door door = other.GetComponent<Door>();
            myColor = door.myColor;
            door.enterDoorParticle.Play();
            StartCoroutine(MateialLerp());
            //skinnedMeshRenderer.material.SetColor("_BaseColor", LevelSettings.Instance.colors[(int)myColor]);
        }
        if (other.CompareTag("FinalWall"))
        {
            transform.SetParent(LevelManager.Instance.ActiveLevel.transform);
            animator.SetTrigger("Idle");
        }
    }

    float time = 0;
    IEnumerator MateialLerp()
    {

        while (time <= 2)
        {
            time += Time.deltaTime;
            Color color = Color.Lerp(skinnedMeshRenderer.material.GetColor("_BaseColor"), LevelSettings.Instance.colors[(int)myColor], time / 2);
            skinnedMeshRenderer.material.SetColor("_BaseColor", color);
            yield return null;
        }
        skinnedMeshRenderer.material.SetColor("_BaseColor", LevelSettings.Instance.colors[(int)myColor]);

    }


    public void GaintHit()
    {
        GameObject go = ObjectPooling.Instance.GetFromPool("HitParticle");
        go.SetActive(true);
        go.transform.position = transform.position;
        go.GetComponent<ParticleSystem>().startColor = LevelSettings.Instance.colors[(int)myColor];
        go.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);

    }


}



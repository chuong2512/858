using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using GPHive.Core;
using GPHive.Game;

public class MoveAndJump : MonoBehaviour
{
    [SerializeField] bool player;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    public void MoveAndJumpMeth(Vector3 playerPos, float movmentSpeed, Transform cannonPos, float jumpPower, float jumpTime)
    {
        movmentSpeed *= 2;
        jumpTime /= 2;
        if (!player)
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Run");
            transform.DOMove(playerPos, Vector3.Distance(transform.position, playerPos) / movmentSpeed).OnComplete(() =>
            {
                transform.GetChild(0).GetComponent<Animator>().SetTrigger("Fly");
                transform.DOJump(cannonPos.position, jumpPower, 1, jumpTime).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });

            });
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Fly");
            transform.DOJump(cannonPos.position, jumpPower, 1, jumpTime).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannon"))
        {
            GameObject go = ObjectPooling.Instance.GetFromPool("HitParticle");
            go.SetActive(true);
            go.transform.position = transform.position;
            go.GetComponent<ParticleSystem>().startColor = skinnedMeshRenderer.material.GetColor("_BaseColor");
            go.GetComponent<ParticleSystem>().Play();

        }
    }
}

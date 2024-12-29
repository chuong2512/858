using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPHive.Core;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
public class Gaint : MonoBehaviour
{
    [SerializeField] float maxHealt;
    private float healt;
    [SerializeField] float damgeAmount, moveTime;
    [HideInInspector] public Animator animator;
    [SerializeField] Transform[] movingPoses;
    [SerializeField] Image healtFiller, healtFillerWhite;
    [SerializeField] WinConitionScript winConitionScript;
    [SerializeField] List<GameObject> Characters = new List<GameObject>();
    int movedPosInt = 0;
    void Start()
    {
        winConitionScript.gaintCount++;
        healtFiller.fillAmount = 1;
        healtFillerWhite.fillAmount = 1;
        healt = maxHealt;
        animator = GetComponent<Animator>();
    }

    public void TakeDamge()
    {
        healt -= damgeAmount;
        healtFillerWhite.DOFillAmount(healtFiller.fillAmount - ((1 / maxHealt) * damgeAmount), .5f);

        healtFiller.fillAmount -= (1 / maxHealt) * damgeAmount;
        if (healt <= 0)
        {
            animator.SetTrigger("Death");
            MMVibrationManager.Haptic(HapticTypes.Success);
            winConitionScript.deathGaint++;
        }
        else
        {
            animator.SetFloat("Blend", Random.Range(0, 3));
            animator.SetTrigger("Damge");
        }

    }

    public void StartMove()
    {
        Move();
    }

    public void Move()
    {
        if (movedPosInt < movingPoses.Length)
        {

            animator.SetTrigger("Walk");
            transform.DOMove(movingPoses[movedPosInt].position, moveTime).OnComplete(() =>
            {
                if (GameManager.Instance.GameState != GameState.End)
                {
                    animator.SetTrigger("Roaring");
                    movedPosInt++;
                }

            });

        }
        else
        {

            animator.SetTrigger("Roaring");
            animator.SetTrigger("FinalRoaring");
            FailLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectiableChar") && !Characters.Contains(other.gameObject))
        {
            Characters.Add(other.gameObject);
            other.GetComponent<CollectiableChar>().GaintHit();
            TakeDamge();
        }
    }

    public void FailLevel()
    {
        GameManager.Instance.LoseLevel();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPHive.Core;
using UnityEngine.UI;
using TMPro;
using MoreMountains.NiceVibrations;

public class CannonScript : MonoBehaviour
{
    [SerializeField] Transform shootingPos, cannonballHadd;
    [SerializeField] GameObject endCam, gaint, tutorial;
    [SerializeField] RectTransform cross;
    [SerializeField] TextMeshProUGUI bulletText;
    [SerializeField] ParticleSystem ShootParticle;
    Color playerColor;
    [SerializeField] float crossMoveSens, charactersMoveToCannonSpeed;
    [SerializeField] float jumpTime, jumpPower, shootingTime, shootingSpeedTime;
    List<GameObject> _CollectiableCharacters = new List<GameObject>();
    List<ParticleSystem> Particles = new List<ParticleSystem>();
    bool canShoot = false, timeUp, oneTime = false, oneTime2 = false;



    Vector3 targetForce;


    private void Start()
    {
        bulletText.text = "0";
    }
    void closeTutorial()
    {
        tutorial.SetActive(false);
    }
    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.End && bulletText.transform.parent.gameObject.activeSelf)
        {
            bulletText.transform.parent.gameObject.SetActive(false);
        }
        if (canShoot)
        {
            timer();
            Targetting();
            if (timeUp)
            {
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                Shoot();
            }
        }

        if (oneTime && _CollectiableCharacters.Count <= 0)
        {
            oneTime = false;
            Invoke("LoseTester", shootingSpeedTime + .5f);
        }
    }
    Vector3 playerPos;
    float movmentSpeed;
    public IEnumerator MoveToCannon(List<GameObject> CollectiableCharacters, CharacterController characterController, GameObject player, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        foreach (var item in characterController.CollectiableCharaters)
        {
            item.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Idle");
        }
        characterController.animator.SetTrigger("Idle");

        characterController.gameFinish = true;
        _CollectiableCharacters.Add(player);
        Particles.Add(player.GetComponent<CharacterController>().trailParticle);
        playerPos = player.transform.position;
        playerColor = (LevelSettings.Instance.colors[(int)characterController.myColor]);
        if (CollectiableCharacters.Count > 0)
            movmentSpeed = CollectiableCharacters[0].GetComponent<CollectiableChar>().movmentSpeed;
        foreach (var item in CollectiableCharacters)
        {
            _CollectiableCharacters.Add(item);
            item.transform.DOKill();
        }
        characterController.tac.SetActive(false);
        characterController.enabled = false;

        foreach (var item in CollectiableCharacters)
        {
            item.GetComponent<CollectiableChar>().enabled = false;
            Particles.Add(item.GetComponent<CollectiableChar>().trailParticle);
        }

        StartCoroutine(MoveAndJump());
    }
    IEnumerator MoveAndJump()
    {
        float WaitTime;
        if (_CollectiableCharacters.Count > 1)
            WaitTime = jumpTime + Vector3.Distance(_CollectiableCharacters[_CollectiableCharacters.Count - 1].transform.position, playerPos) / movmentSpeed;
        else
            WaitTime = jumpTime;

        for (int i = 0; i < _CollectiableCharacters.Count; i++)
        {
            yield return new WaitForSeconds(.1f);
            _CollectiableCharacters[i].GetComponent<MoveAndJump>().MoveAndJumpMeth(playerPos, movmentSpeed, transform, jumpPower, jumpTime);
        }
        yield return new WaitForSeconds(WaitTime);

        bulletText.text = (_CollectiableCharacters.Count).ToString();
        Invoke("closeTutorial", 3.5f);
        endCam.SetActive(true);
        canShoot = true;
        oneTime = true;
    }

    float time = 0;
    void timer()
    {

        if (Input.GetMouseButtonDown(0))
        {
            time = 0;
        }
        if (Input.GetMouseButton(0))
        {
            time += Time.deltaTime;

            if (time >= shootingTime)
            {
                timeUp = true;
                time = 0;
            }
            else
            {
                timeUp = false;
            }
        }
    }
    public float posZ;
    void Targetting()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 _movePos = new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0) * crossMoveSens;
                Vector2 limit = cross.anchoredPosition + _movePos;
                if (limit.x > -500 && limit.x < 500 && limit.y > 0 && limit.y < 1000)
                {
                    cross.anchoredPosition += _movePos;

                    float xMultipler = 20;
                    float zMultipler = 12.5f;
                    cannonballHadd.transform.localRotation = Quaternion.Euler((limit.y / xMultipler), 0, -(limit.x / zMultipler));
                    //cannonballHadd.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(cross.position.x, cross.position.y, posZ)), Vector3.up);
                }
                targetForce = Camera.main.ScreenToWorldPoint(new Vector3(cross.position.x, cross.position.y, posZ));

            }

        }

    }
    void Shoot()
    {
        if (GameManager.Instance.GameState != GameState.End)
        {
            if (_CollectiableCharacters.Count > 0)
            {
                ShootParticle.startColor = playerColor;
                ShootParticle.Play();
                _CollectiableCharacters[0].transform.position = shootingPos.position;
                _CollectiableCharacters[0].SetActive(true);
                Rigidbody rb = _CollectiableCharacters[0].GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                if (targetForce != null)
                {
                    cannonballHadd.GetComponent<Animator>().SetTrigger("Blend");

                    _CollectiableCharacters[0].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Fly");
                    Particles[0].Play();
                    _CollectiableCharacters[0].transform.DOJump(targetForce, jumpPower, 1, shootingSpeedTime).SetEase(Ease.Linear);
                    _CollectiableCharacters[0].transform.DOScale(_CollectiableCharacters[0].transform.localScale * 2.8f, shootingSpeedTime).SetEase(Ease.Linear);
                }
                Particles.Remove(Particles[0]);
                _CollectiableCharacters.Remove(_CollectiableCharacters[0]);
                bulletText.text = (_CollectiableCharacters.Count).ToString();


            }
        }
    }
    public void LoseTester()
    {
        if (GameManager.Instance.GameState != GameState.End)
        {
            GameManager.Instance.LoseLevel();
            MMVibrationManager.Haptic(HapticTypes.Success);
            gaint.GetComponent<Gaint>().animator.SetTrigger("Win");
        }
    }


}

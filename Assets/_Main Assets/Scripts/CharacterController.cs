using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using GPHive.Core;
using MoreMountains.NiceVibrations;
using TMPro;

public class CharacterController : MonoBehaviour
{
    public Swipe swipeSc;
    [HideInInspector] public DefaultFinal defaultFinal;
    public TextMeshProUGUI CollectiableCounterTxt;
    int CollectiableCounterTxtCount;
    [SerializeField] float punchSeccond;
    public float movmentSpeed;
    [HideInInspector] public bool amIMove = false;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject tac;
    public ParticleSystem trailParticle;
    public Animator animator;
    public LevelSettings.ColorEnum myColor;
    [SerializeField] LayerMask wallLayer, platformLayer, collectiableCharLayer, hitLayer, doorLayer;
    [HideInInspector] public List<GameObject> CollectiableCharaters = new List<GameObject>();
    [SerializeField] List<GameObject> tempCollectiableCharaters = new List<GameObject>();
    [SerializeField] List<Transform> touchingPoses = new List<Transform>();
    Coroutine tempAddingCoroutine;
    bool continouSearching = false;
    [HideInInspector] public bool gameFinish = false, inputClose = false;
    Direction lastDirection;

    bool inCharacter;

    enum Direction
    {
        Right, Left, Up, Down
    }

    public Sequence sequence;

    private void Start()
    {
        skinnedMeshRenderer.material.SetColor("_BaseColor", LevelSettings.Instance.colors[(int)myColor]);
        tac.GetComponent<MeshRenderer>().materials[1].color = LevelSettings.Instance.colors[(int)myColor];
        swipeSc = GetComponent<Swipe>();
        sequence = DOTween.Sequence();
        defaultFinal = GetComponent<DefaultFinal>();
        CollectiableCounterTxt.text = (CollectiableCounterTxtCount + 1).ToString();
    }

    private void CheckPlatforms()
    {
        if (Physics.SphereCast(transform.position, .2f, Vector3.down, out RaycastHit _hit, 1f, platformLayer))
        {
            if (!touchingPoses.Contains(_hit.transform))
            {
                touchingPoses.Add(_hit.transform);


                if (_hit.transform.TryGetComponent(out Box box))
                {
                    if (box.HasStickman)
                    {
                        CollectStickman(box.StickmanOnBox);
                        return;
                    }

                    if (inCharacter)
                    {
                        CollectiableAddToRealList();
                        inCharacter = false;
                        tempBool = false;
                    }
                };
            }
        }
    }


    void CheckColoringDoor(Vector3 platform, Vector3 lastPlatform)
    {
        if (Physics.Raycast(new Vector3(platform.x, .5f, platform.z), new Vector3(lastPlatform.x, .5f, lastPlatform.z), out RaycastHit hit2, doorLayer))
        {

            Debug.DrawRay(new Vector3(platform.x, .5f, platform.z), new Vector3(lastPlatform.x, .5f, lastPlatform.z), Color.red, 300);
            if (hit2.transform.CompareTag("Door"))
            {

                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                Door door = hit2.transform.GetComponent<Door>();
                myColor = door.myColor;
                door.enterDoorParticle.Play();
                StartCoroutine(MateialLerp());
                Movement(lastDirection);
            }
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            if (!gameFinish)
            {
                if (!amIMove)
                {
                    MovmentDirectionDetergent();
                }
            }
        }
    }

    bool queueBool;
    private void CollectStickman(CollectiableChar stickman)
    {
        if (stickman.myColor != myColor)
        {
            Punch();
            return;
        }


        if (!CollectiableCharaters.Contains(stickman.gameObject))
        {
            stickman.gameObject.transform.rotation = transform.rotation;
            CollectiableCounterTxtCount++;
            CollectiableCounterTxt.text = (CollectiableCounterTxtCount + 1).ToString();

            MMVibrationManager.Haptic(HapticTypes.MediumImpact);

            if (!inCharacter)
            {
                inCharacter = true;
            }

            tempCollectiableCharaters.Add(stickman.gameObject);
            if (CollectiableCharaters.Count > 0 && touchingPoses.Count > 0)
            {

                CollectiableCharaters[0].GetComponent<CollectiableChar>().takePose = false;

                if (!tempBool)
                {
                    var _tempPos = touchingPoses[Mathf.Clamp((touchingPoses.Count - 2), 0, touchingPoses.Count - 1)].position;
                    CollectiableCharaters[0].GetComponent<CollectiableChar>().movingPoses.Add(new Vector3(_tempPos.x, CollectiableCharaters[0].transform.position.y, _tempPos.z));
                    tempBool = true;
                }
            }
        }
    }

    void ColectiableSearcher()
    {
        if (tempCollectiableCharaters.Count > 0 && continouSearching)
        {
            bool testing = false;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, .5f);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].transform.CompareTag("CollectiableChar"))
                {
                    testing = true;
                }
            }

            if (!testing)
            {
                CollectiableAddToRealList();
            }
        }
    }

    void CollectiableAddToRealList()
    {
        if (touchingPoses.Count > 1)
        {
            if (CollectiableCharaters.Count > 0)
            {
                CollectiableCharaters[0].GetComponent<CollectiableChar>().takePose = true;

                List<GameObject> temp = new List<GameObject>();
                foreach (var item in CollectiableCharaters)
                {
                    temp.Add(item);
                }
                CollectiableCharaters.Clear();
                for (int i = tempCollectiableCharaters.Count - 1; i >= 0; i--)
                {
                    CollectiableCharaters.Add(tempCollectiableCharaters[i]);
                    StageController.Instance.collecktedCount++;
                }

                foreach (var item in temp)
                {
                    CollectiableCharaters.Add(item);
                }
            }
            else
            {
                for (int i = tempCollectiableCharaters.Count - 1; i >= 0; i--)
                {
                    CollectiableCharaters.Add(tempCollectiableCharaters[i]);
                    StageController.Instance.collecktedCount++;
                }
            }
        }

        tempCollectiableCharaters = new List<GameObject>();
        continouSearching = false;
    }

    bool tempBool = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
        {
            if (!touchingPoses.Contains(other.transform))
            {
                touchingPoses.Add(other.transform);
                continouSearching = true;
                if (other.transform.TryGetComponent(out Box box))
                {

                    if (box.HasStickman)
                    {
                        CollectStickman(box.StickmanOnBox);
                        box.CollectStickman();
                        return;
                    }

                    if (inCharacter)
                    {
                        CollectiableAddToRealList();
                        inCharacter = false;
                        tempBool = false;
                    }
                }

                if (touchingPoses.Count >= 2)
                    CheckColoringDoor(touchingPoses[touchingPoses.Count - 2].transform.position, touchingPoses[touchingPoses.Count - 1].transform.position);

                CheckHitTail();
            }
            else
            {
                touchingPoses.Remove(other.transform);
                touchingPoses.Add(other.transform);
                continouSearching = true;
                if (other.transform.TryGetComponent(out Box box))
                {

                    if (box.HasStickman)
                    {
                        CollectStickman(box.StickmanOnBox);
                        box.CollectStickman();
                        return;
                    }

                    if (inCharacter)
                    {
                        CollectiableAddToRealList();
                        inCharacter = false;
                        tempBool = false;
                    }
                }
                CheckHitTail();

            }

            if (!gameFinish)
                CollectiableCharMover();
        }

        if (other.CompareTag("Wall"))
        {
            Punch();
        }

        if (other.CompareTag("Door"))
        {

            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            Door door = other.GetComponent<Door>();
            myColor = door.myColor;
            door.enterDoorParticle.Play();
            StartCoroutine(MateialLerp());
            Movement(lastDirection);
        }

        else if (other.CompareTag("Finish"))
        {
            FinishDoor finishDoor = other.GetComponent<FinishDoor>();
            if (finishDoor.myColor != myColor || !StageController.Instance.NextStage(this, finishDoor.transformMovingPos, finishDoor.animator))
            {
                Punch();
            }
            else
            {
                finishDoor.animator.SetTrigger("Play");
            }


        }
        if (other.CompareTag("FinalWall"))
        {
            MMVibrationManager.Haptic(HapticTypes.SoftImpact);
            transform.SetParent(LevelManager.Instance.ActiveLevel.transform);
            animator.ResetAll();
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
            tac.GetComponent<MeshRenderer>().materials[1].color = color;
            yield return null;
        }

        skinnedMeshRenderer.material.SetColor("_BaseColor", LevelSettings.Instance.colors[(int)myColor]);
        tac.GetComponent<MeshRenderer>().materials[1].color = LevelSettings.Instance.colors[(int)myColor];

    }

    IEnumerator CO_WaitFollow()
    {
        yield return null;
        CollectiableCharaters[0].GetComponent<CollectiableChar>().takePose = false;
    }

    void Punch()
    {
        animator.ResetAll();
        animator.SetTrigger("Idle");
        amIMove = false;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, .3f, -transform.up, out hit, 20, platformLayer))
        {
            if (Physics.Raycast(hit.transform.position, transform.up, out RaycastHit hit2, 2, collectiableCharLayer))
            {
                transform.DOMove(new Vector3(touchingPoses[touchingPoses.Count - 2].transform.position.x, transform.position.y, touchingPoses[touchingPoses.Count - 2].transform.position.z), punchSeccond).SetUpdate(UpdateType.Fixed).OnComplete(() =>
                {
                    animator.ResetAll();
                    animator.SetTrigger("Idle");
                    amIMove = false;
                });
                touchingPoses.Remove(touchingPoses[touchingPoses.Count - 1]);
            }
            else
            {
                transform.DOMove(new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z), punchSeccond).SetUpdate(UpdateType.Fixed).OnComplete(() =>
                {
                    animator.ResetAll();
                    animator.SetTrigger("Idle");
                    amIMove = false;

                });
            }
        }
    }


    void MovmentDirectionDetergent()
    {
        if (!inputClose)
        {
            if (swipeSc.SwipeRight)
            {
                Movement(Direction.Right);
                lastDirection = Direction.Right;
            }
            else if (swipeSc.SwipeLeft)
            {
                Movement(Direction.Left);
                lastDirection = Direction.Left;
            }
            else if (swipeSc.SwipeUp)
            {
                Movement(Direction.Up);
                lastDirection = Direction.Up;
            }
            else if (swipeSc.SwipeDown)
            {
                Movement(Direction.Down);
                lastDirection = Direction.Down;
            }
        }

    }

    Coroutine movementCoroutine;

    bool scale = false;
    void Movement(Direction direction)
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        RaycastHit hitForPunch;

        Vector3 directionPos = Vector3.zero;
        switch (direction)
        {
            case Direction.Right:
                directionPos = Vector3.right;
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.Left:
                directionPos = Vector3.left;
                transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case Direction.Up:
                directionPos = Vector3.forward;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Down:
                directionPos = Vector3.back;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }

        if (!scale)
        {
            if (Physics.Raycast(transform.position, directionPos, out hitForPunch, 1.3f, collectiableCharLayer))
            {
                if (CollectiableCharaters.Contains(hitForPunch.transform.gameObject))
                {
                    scale = true;
                    transform.DOPunchScale(Vector3.one * .5f, punchSeccond * 2, 1, 1).OnComplete(() =>
                    {
                        swipeSc.SwipeMeth();
                        scale = false;
                    });

                }
            }
        }



        amIMove = true;

        if (CheckHitTail())
            return;

        RaycastHit hit2;

        if (Physics.Raycast(transform.position, transform.forward, out hit2, Mathf.Infinity, hitLayer))
        {
            Vector3 movingPos = Vector3.zero;
            movingPos = new Vector3(hit2.transform.position.x, transform.position.y, hit2.transform.position.z) - transform.TransformDirection(new Vector3(0, 0, 1.9f));

            if (hit2.transform.CompareTag("Door"))
            {
                goto jump;
            }

            if (hit2.transform.CompareTag("CollectiableChar"))
            {
                if (hit2.transform.GetComponent<CollectiableChar>().myColor != myColor)
                {
                    movementCoroutine = StartCoroutine(CO_MoveCharacter(movingPos));
                    return;
                }

            }
        }

    jump:

        if (Physics.Raycast(transform.position + Vector3.up * .5f, transform.forward, out RaycastHit hit, Mathf.Infinity, wallLayer))
        {
            Vector3 movingPos = Vector3.zero;
            movingPos = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z) - transform.TransformDirection(new Vector3(0, 0, 1.9f));

            if (hit.transform.CompareTag("Finish"))
            {
                FinishDoor _finishDoor = hit.transform.GetComponent<FinishDoor>();
                if (_finishDoor.myColor == myColor)
                    movingPos = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
            }

            movementCoroutine = StartCoroutine(CO_MoveCharacter(movingPos));
            return;
        }
    }

    private bool CheckHitTail()
    {
        if (Physics.SphereCast(transform.position, .4f, transform.forward, out RaycastHit hit, 1.4f, collectiableCharLayer))
        {
            if (CollectiableCharaters.Contains(hit.transform.gameObject))
            {
                if (movementCoroutine != null)
                    StopCoroutine(movementCoroutine);

                Punch();
                return true;
            }
        }
        return false;
    }


    IEnumerator CO_MoveCharacter(Vector3 movingPos)
    {
        animator.SetTrigger("Run");

        var _time = 0f;
        var _travelTime = Vector3.Distance(transform.position, movingPos) / movmentSpeed;
        var _startPos = transform.position;
        while (_time < _travelTime)
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, movingPos, _time / _travelTime);
            yield return new WaitForFixedUpdate();
        }
        transform.position = movingPos;

        animator.ResetAll();
        animator.SetTrigger("Idle");

        Punch();
        swipeSc.SwipeMeth();
        Invoke("FailDedeckted", .3f);

    }

    void FailDedeckted()
    {
        RaycastHit hit;
        int Counter = 0;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1.51f))
            Counter += Dedeckted(hit);

        if (Physics.Raycast(transform.position, -transform.forward, out hit, 1.51f))
            Counter += Dedeckted(hit);

        if (Physics.Raycast(transform.position, transform.right, out hit, 1.51f))
            Counter += Dedeckted(hit);

        if (Physics.Raycast(transform.position, -transform.right, out hit, 1.51f))
            Counter += Dedeckted(hit);

        if (Counter >= 4)
            GameManager.Instance.LoseLevel();
    }
    int Dedeckted(RaycastHit hit)
    {
        if (hit.transform.CompareTag("CollectiableChar") && CollectiableCharaters.Contains(hit.transform.gameObject) || hit.transform.CompareTag("CollectiableChar") && hit.transform.GetComponent<CollectiableChar>().myColor != myColor)
            return 1;
        else if (hit.transform.CompareTag("Wall"))
            return 1;

        if (hit.transform.CompareTag("Finish"))
            if (hit.transform.GetComponent<FinishDoor>().myColor != myColor)
            {
                Debug.Log("Dedected");
                return 1;
            }



        return 0;
    }

    void CollectiableCharMover()
    {
        for (int i = 0; i < CollectiableCharaters.Count; i++)
        {
            Vector3 movingPos = touchingPoses[touchingPoses.Count - 2 - i].transform.position;
            Transform rotation;


            Vector3 movingPosLast = new Vector3(movingPos.x, CollectiableCharaters[i].transform.position.y, movingPos.z);

            if (CollectiableCharaters[i].GetComponent<CollectiableChar>().takePose)
            {
                if (i == 0)
                    rotation = transform;
                else rotation = CollectiableCharaters[i - 1].transform;


                CollectiableCharaters[i].GetComponent<CollectiableChar>().rotation = rotation;

                CollectiableCharaters[i].GetComponent<CollectiableChar>().movingPoses.Add(movingPosLast);
                CollectiableCharaters[i].GetComponent<CollectiableChar>().Move();
            }
        }
    }

}


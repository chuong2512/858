using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using GPHive.Core;
using MoreMountains.NiceVibrations;

public class StageController : Singleton<StageController>
{
    [SerializeField] LayerMask wallLayer;
    public List<StagesClass> Stages = new List<StagesClass>();
    [HideInInspector] public int collecktedCount;
    [HideInInspector] public int myStage = 0;

    public int CannonPer;
    [HideInInspector] bool cannonBall;
    [SerializeField] GameObject CannonSceene, DefaultSceene;
    [SerializeField] GameObject cannonMovmentPos, DefaultEndMovmentPos;
    [SerializeField] private GameObject gaint;
    [SerializeField] private GameObject cannon;



    private void OnEnable()
    {
        EventManager.LevelStarted += LevelEndChooseMeth;
    }
    private void OnDisable()
    {
        EventManager.LevelStarted -= LevelEndChooseMeth;
    }


    void LevelEndChooseMeth()
    {

        if ((float)SaveLoadManager.GetLevel() % (float)CannonPer == 0)
            cannonBall = true;
        else
            cannonBall = false;

        if (!cannonBall)
            DefaultSceene.SetActive(true);
        else
            CannonSceene.SetActive(true);
    }


    public bool NextStage(CharacterController characterController, Transform movingPos, Animator animator)
    {

        if (GameManager.Instance.TutorialCanvas.activeSelf)
            GameManager.Instance.TutorialCanvas.SetActive(false);

        MMVibrationManager.Haptic(HapticTypes.Success);
        StagesClass stage = Stages[myStage];
        bool returnBool = false;
        if (stage.collectiableCount <= collecktedCount)
        {
            returnBool = true;
            characterController.transform.DOComplete();
            characterController.amIMove = true;
            Stages[myStage + 1].stage.SetActive(true);


            List<GameObject> chars = new List<GameObject>();

            foreach (var item in characterController.CollectiableCharaters)
            {
                item.transform.SetParent(Stages[myStage + 1].stage.transform);
                chars.Add(item);
            }
            CameraManager.Instance.SwitchCamera(Stages[myStage + 1].camName);

            stage.myDoor.GetComponent<Collider>().enabled = false;
            characterController.animator.SetTrigger("Run");

            foreach (var item in characterController.CollectiableCharaters)
            {
                item.GetComponent<CollectiableChar>().animator.SetTrigger("Run");
                
            }


            Vector3 Pos;

            if (Stages[myStage + 1].lastStage && cannonBall)
            {
                foreach (var item in characterController.CollectiableCharaters)
                {
                    CollectiableChar _collectiableChar =item.GetComponent<CollectiableChar>();
                    _collectiableChar.IdleKiller = true;
                    _collectiableChar.animator.ResetTrigger("Idle");
                    _collectiableChar.animator.SetTrigger("Running");
                    //_collectiableChar.gameObject.transform.SetParent(characterController.transform);
                }

                characterController.movmentSpeed *= 1.5f;
                foreach (var item in characterController.CollectiableCharaters)
                {
                    item.GetComponent<CollectiableChar>().movmentSpeed *= 3;
                }

                characterController.CollectiableCounterTxt.transform.parent.gameObject.SetActive(false);
                characterController.inputClose = true;
                Pos = new Vector3(cannonMovmentPos.transform.position.x, characterController.transform.position.y, cannonMovmentPos.transform.position.z);
            }
            else if (Stages[myStage + 1].lastStage && !cannonBall)
            {

                foreach (var item in characterController.CollectiableCharaters)
                {
                    CollectiableChar _collectable =item.GetComponent<CollectiableChar>();
                    _collectable.IdleKiller = true;
                    _collectable.animator.ResetTrigger("Idle");
                    _collectable.animator.SetTrigger("Running");
                    //_collectiableChar.gameObject.transform.SetParent(characterController.transform);
                }

                characterController.movmentSpeed *= 1.5f;
                foreach (var item in characterController.CollectiableCharaters)
                {
                    item.GetComponent<CollectiableChar>().movmentSpeed *= 3;
                }

                characterController.CollectiableCounterTxt.transform.parent.gameObject.SetActive(false);
                characterController.inputClose = true;
                Pos = new Vector3(DefaultEndMovmentPos.transform.position.x, characterController.transform.position.y, DefaultEndMovmentPos.transform.position.z);
            }
            else
                Pos = new Vector3(movingPos.position.x, characterController.transform.position.y, movingPos.position.z);

            characterController.swipeSc.Reset();
            characterController.transform.DOMove(Pos, Vector3.Distance(characterController.transform.position, Pos) / (characterController.movmentSpeed / 2)).SetEase(Ease.Linear).OnComplete(() =>
            {
                stage.myDoor.GetComponent<Collider>().enabled = true;
                stage.myDoor.tag = "Wall";
                foreach (var item in characterController.CollectiableCharaters)
                {
                    CollectiableChar _collectableCharacter =item.GetComponent<CollectiableChar>();
                    _collectableCharacter.IdleKiller = false;
                    _collectableCharacter.animator.SetTrigger("Idle");
                }
                characterController.amIMove = false;
                myStage++;
                if (Stages[myStage].BridgeClosser != null)
                    Stages[myStage].BridgeClosser.SetActive(true);

                if (Stages[myStage].lastStage)
                {
                    if (!cannonBall)
                    {
                        CameraManager.Instance.SwitchCamera("LastCam2.1");
                        DefaultSceene.SetActive(true);
                        StartCoroutine(characterController.defaultFinal.DoArrangement(characterController.CollectiableCharaters, characterController, characterController.gameObject, 1));
                    }
                    else
                    {
                        CameraManager.Instance.SwitchCamera("LastCam2.2");
                        CannonSceene.SetActive(true);
                        StartCoroutine(cannon.GetComponent<CannonScript>().MoveToCannon(characterController.CollectiableCharaters, characterController, characterController.gameObject, 1));
                    }

                }
                else
                {
                    characterController.swipeSc.SwipeMeth();
                }
            });

        }
        return returnBool;
    }

}


[System.Serializable]
public class StagesClass
{
    public int stageCount, collectiableCount;
    public string camName;
    public GameObject stage, myDoor, BridgeClosser;
    public bool lastStage;
}

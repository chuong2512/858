using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GPHive.Core;
using Cinemachine;

public class DefaultFinal : MonoBehaviour
{
    [SerializeField] float heightDistance, weightDistance;
    [SerializeField] float jumpTime, jumpPower;
    [SerializeField] Transform triangleParent;
    [SerializeField] GameObject endCam;

    List<int> CalculateArrangement(int CollectiableCharactersCount)
    {
        List<int> defaultArrangement = new List<int>();
        int tempCounter = 0;
        int maxCount = 0;

        for (int i = 1; i < 5; i++)
        {
            if (CollectiableCharactersCount > tempCounter)
            {
                maxCount = i;
                if (CollectiableCharactersCount - tempCounter >= maxCount)
                {
                    defaultArrangement.Add(i);
                    tempCounter += i;
                }
                else
                {
                    defaultArrangement.Add(CollectiableCharactersCount - tempCounter);
                    tempCounter += CollectiableCharactersCount - tempCounter;
                }
            }
        }
        while (CollectiableCharactersCount > tempCounter)
        {
            maxCount = 5;
            if (CollectiableCharactersCount - tempCounter >= maxCount)
            {
                defaultArrangement.Add(5);
                tempCounter += 5;
            }
            else
            {
                defaultArrangement.Add(CollectiableCharactersCount - tempCounter);
                tempCounter += CollectiableCharactersCount - tempCounter;
            }
        }



        if (maxCount > defaultArrangement[defaultArrangement.Count - 1])
        {
            int remainder = defaultArrangement[defaultArrangement.Count - 1];
            defaultArrangement.RemoveAt(defaultArrangement.Count - 1);
            tempCounter = 0;
            for (int i = 0; i < remainder; i++)
            {
                if ((defaultArrangement.Count - 1) - tempCounter > -1)
                {
                    defaultArrangement[(defaultArrangement.Count - 1) - tempCounter]++;

                }
                else
                {
                    tempCounter = 0;
                    defaultArrangement[(defaultArrangement.Count - 1) - tempCounter]++;
                }
                tempCounter++;
            }
        }
        return defaultArrangement;
    }

    public IEnumerator DoArrangement(List<GameObject> CollectiableCharacters, CharacterController characterController, GameObject player, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        foreach (var item in characterController.CollectiableCharaters)
        {
            item.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Idle");
        }
        characterController.animator.SetTrigger("Idle");


        List<int> Arrangement = CalculateArrangement(CollectiableCharacters.Count + 1);
        List<GameObject> _CollectiableCharacters = CollectiableCharacters;
        characterController.gameFinish = true;

        foreach (var item in _CollectiableCharacters)
        {
            item.GetComponent<CollectiableChar>().levelEnd = true;
        }

        _CollectiableCharacters.Add(player);

        Vector3 playerPos = characterController.transform.position;

        characterController.tac.SetActive(false);
        characterController.enabled = false;

        float _heightDistance = 0;
        int collectiableCharacterCount = _CollectiableCharacters.Count - 1;
        float xPoseStartPos = playerPos.x - ((Arrangement[Arrangement.Count - 1] / 2) * weightDistance);


        for (int i = Arrangement.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < Arrangement[i]; j++)
            {

                float xPose = xPoseStartPos + (j * weightDistance);
                Vector3 movePosFlat = new Vector3(xPose, playerPos.y, playerPos.z);
                Vector3 movePos = new Vector3(xPose, playerPos.y + _heightDistance, playerPos.z);

                _CollectiableCharacters[collectiableCharacterCount].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Idle");
                _CollectiableCharacters[collectiableCharacterCount].transform.DORotate(new Vector3(0, 0, 0), jumpTime);
                Sequence _sequence = DOTween.Sequence();
                if (j == Arrangement[i] - 1 && i == 0)
                {

                    _sequence.Join(_CollectiableCharacters[collectiableCharacterCount].transform
                        .DOMove(movePosFlat, jumpTime * 2.5f));
                    _sequence.Append(_CollectiableCharacters[collectiableCharacterCount].transform
                        .DOMove(movePos, jumpTime * 2.5f)).OnComplete(() =>
                    {

                        foreach (var item in characterController.CollectiableCharaters)
                        {
                            item.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Idle");
                        }
                        characterController.animator.SetTrigger("Idle");

                        for (int i = 0; i < Arrangement[Arrangement.Count - 1]; i++)
                        {
                            _CollectiableCharacters[_CollectiableCharacters.Count - (i + 1)].transform.GetChild(0).GetComponent<Animator>().SetTrigger("Running");

                        }


                        triangleParent.transform.DOMoveZ(triangleParent.position.x + 510, 55);
                        triangleParent.GetComponent<TriengalParent>().makeJustOne = true;
                        endCam.SetActive(true);
                        AdjustCamera.Instance.myCount = Arrangement.Count;

                    });

                }
                else
                //_CollectiableCharacters[collectiableCharacterCount].transform.DOJump(movePos, jumpPower, 1, jumpTime);
                {
                    _sequence.Join(_CollectiableCharacters[collectiableCharacterCount].transform
                        .DOMove(movePosFlat, jumpTime * 2.5f));
                    _sequence.Append(_CollectiableCharacters[collectiableCharacterCount].transform
                        .DOMove(movePos, jumpTime * 2.5f));
                }

                _CollectiableCharacters[collectiableCharacterCount].transform.localRotation = Quaternion.identity;
                _CollectiableCharacters[collectiableCharacterCount].transform.SetParent(triangleParent);
                collectiableCharacterCount--;
            }

            yield return new WaitForSeconds(.2f);

            _heightDistance += heightDistance;
            if (i - 1 >= 0)
                if (Arrangement[i - 1] == Arrangement[i])
                    continue;

            xPoseStartPos += weightDistance / 2;

        }
    }
}

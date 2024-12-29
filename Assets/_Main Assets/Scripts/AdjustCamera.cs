using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AdjustCamera : Singleton<AdjustCamera>
{
    CinemachineVirtualCamera vCam;
    CinemachineTransposer cinemachineTransposer;

    public Vector3 minFollow;
    public Vector3 maxFollow;

    public int targetCount;
    public int myCount = 1;

    public float smooth;


    private Vector3 veloicty = Vector3.one;

    private void OnEnable()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        cinemachineTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        Vector3 _targetOffset = Vector3.Lerp(minFollow, maxFollow, (float)myCount / (float)targetCount);
        cinemachineTransposer.m_FollowOffset = Vector3.SmoothDamp(cinemachineTransposer.m_FollowOffset, _targetOffset, ref veloicty, smooth);

    }
}
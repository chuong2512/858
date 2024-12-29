using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    Vector3 Ofset;
    [SerializeField] GameObject Taxi;
    private void Start()
    {
        Ofset = transform.position - Taxi.transform.position;
    }
    void Update()
    {
        transform.position = Taxi.transform.position + Ofset;

    }
}

using System.Collections;
using UnityEngine;
using GPHive.Core;

[RequireComponent(typeof(Rigidbody))]
public class SwerveController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed,horizontalSpeed,sensitivity,swerveLimit,inputResetTime;
    private float lastFingerPosX,swerveAmount,difference;

    private Vector3 swerveInput;

    [SerializeField] Transform leftLimit,rightLimit;

    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody=GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(GameManager.Instance.GameState==GameState.Playing)
            Swerve();
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.GameState==GameState.Playing)
            Movement();
    }

    public void Swerve()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastFingerPosX=Input.mousePosition.x;
            swerveAmount=0;
            swerveInput=Vector3.zero;
            return;
        }

        if(Input.GetMouseButton(0))
        {
            difference=Input.mousePosition.x-lastFingerPosX;
            swerveAmount=Mathf.Clamp(difference*sensitivity*Time.fixedDeltaTime,-swerveLimit,swerveLimit);
            swerveInput=new Vector3(swerveAmount,0,0);
            lastFingerPosX=Input.mousePosition.x;
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            StartCoroutine(ResetInput(inputResetTime));
        }
    }

    IEnumerator ResetInput(float inputResetTime)
    {
        while (swerveInput.magnitude>.1f)
        {
            swerveInput=Vector3.Lerp(swerveInput,Vector3.zero,Time.deltaTime*inputResetTime);
            yield return new WaitForEndOfFrame();
        }

        swerveInput=Vector3.zero;
    }

    private void Movement()
    {
        Vector3 _verticalMovement;
        Vector3 _horizontalMovement;

        _verticalMovement=transform.forward*forwardSpeed*Time.deltaTime;
        _horizontalMovement=swerveInput*horizontalSpeed;


        Vector3 _finalMovement=_verticalMovement+_horizontalMovement;

        if((rigidbody.position+_finalMovement).x<leftLimit.position.x || (rigidbody.position+_finalMovement).x>rightLimit.position.x)
            _finalMovement.x=0;

        rigidbody.MovePosition(rigidbody.position+_finalMovement);
    }
}

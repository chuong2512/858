using UnityEngine;

public static class AnimatorExtensions
{
    public static void ResetTriggers(this Animator animator)
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(parameter.name);
        }
    }

    public static void ResetBools(this Animator animator)
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(parameter.name, false);
        }
    }

    public static void ResetFloats(this Animator animator)
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Float)
                animator.SetFloat(parameter.name, 0);
        }
    }


    public static void ResetIntegers(this Animator animator)
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Int)
                animator.SetInteger(parameter.name, 0);
        }
    }

    public static void ResetAll(this Animator animator)
    {
        animator.ResetTriggers();
        animator.ResetBools();
        animator.ResetFloats();
        animator.ResetIntegers();

    }

}
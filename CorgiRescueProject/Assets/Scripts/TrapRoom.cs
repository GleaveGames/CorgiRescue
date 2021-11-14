using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRoom : MonoBehaviour
{
    [SerializeField]
    protected bool triggered = false;

    private void Start()
    {
        StartCoroutine(WaitingForTrigger());
    }

    protected virtual void Effect()
    {
        //what happens when triggered;
    }

    protected virtual void Cause()
    {
        //if bla bla then triggered = true;
    }

    protected IEnumerator WaitingForTrigger()
    {
        while (!triggered)
        {
            Cause();
            yield return null;
        }
        Effect();
    }
}

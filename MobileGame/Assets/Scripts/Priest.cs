using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Unit
{
    public bool triggered = false;
    
    public override IEnumerator OnStartOfTurn()
    {
        triggered = false;
        yield return StartCoroutine(base.OnStartOfTurn());
    }
}

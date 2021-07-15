using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBig : Beetle
{
    protected override IEnumerator Turn()
    {
        crawlsound.Stop();
        ChangeAnimationState("BeetleIdleBig");
        midturn = true;
        yield return new WaitForSeconds(1);
        Rerotate();
        yield return new WaitForSeconds(0.5f);
        turning = false;
        midturn = false;
        ChangeAnimationState("BeetleMoveBig");
        crawlsound.Play();
    }
}

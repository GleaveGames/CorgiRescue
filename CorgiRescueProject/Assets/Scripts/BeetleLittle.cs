using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleLittle : Beetle
{
    protected override IEnumerator Turn()
    {
        crawlsound.Stop();
        ChangeAnimationState("BeetleIdleSmall");
        midturn = true;
        yield return new WaitForSeconds(1);
        Rerotate();
        yield return new WaitForSeconds(0.5f);
        turning = false;
        midturn = false;
        ChangeAnimationState("BeetleMoveSmall");
        crawlsound.Play();
    }
}

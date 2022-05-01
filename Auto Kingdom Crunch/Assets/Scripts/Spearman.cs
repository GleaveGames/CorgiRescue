using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Unit
{
    public override IEnumerator OnSell()
    {
        FindObjectOfType<GameController>().Gold += level;
        return base.OnSell();
    }
}

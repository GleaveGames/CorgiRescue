using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFromAnims : MonoBehaviour
{
    public void LoadFromAnim()
    {
        transform.parent.GetComponent<Menu>().LoadGame();
    }
}

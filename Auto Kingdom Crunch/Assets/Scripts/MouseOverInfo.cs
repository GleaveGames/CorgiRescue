using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverInfo : MonoBehaviour
{
    GameObject unitTextParent;
    List<Coroutine> mouseoverchecks;


    private void Start()
    {
        unitTextParent = transform.GetChild(0).gameObject;
        mouseoverchecks = new List<Coroutine>();
    }


    public void OnMouseEnter()
    {
        Coroutine i = StartCoroutine(MouseOverCheck());
        mouseoverchecks.Add(i);
    }
    public void OnMouseExit()
    {
        unitTextParent.SetActive(false);
        StopAllMouseOvers();
    }

    private IEnumerator MouseOverCheck()
    {
        yield return new WaitForSeconds(0.6f);
        unitTextParent.SetActive(true);
    }
    private void StopAllMouseOvers()
    {
        for (int i = mouseoverchecks.Count - 1; i >= 0; i--)
        {
            StopCoroutine(mouseoverchecks[i]);
            mouseoverchecks.RemoveAt(i);
        }
    }
}

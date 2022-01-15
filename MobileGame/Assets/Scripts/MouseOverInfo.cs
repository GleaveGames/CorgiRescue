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
    }


    public void OnMouseEnter()
    {
        Coroutine i = StartCoroutine(MouseOverCheck());
        mouseoverchecks.Add(i);
    }
    public void OnMouseExit()
    {
        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 11;
        unitTextParent.SetActive(false);
        StopAllMouseOvers();
    }

    private IEnumerator MouseOverCheck()
    {
        yield return new WaitForSeconds(0.6f);
        transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 12;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    [SerializeField]
    private List<Flames> flames;
    public bool FlamesOn;
    Coroutine coroutine;
    [SerializeField]
    private PressurePlate pp;
    private bool switched;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FlamesLineup");
    }

    // Update is called once per frame
    void Update()
    {
        if (!switched)
        {
            if (pp.triggered)
            {
                FlamesOn = false;
                switched = true;
                for (int i = 0; i < flames.Count; i++)
                {
                    for (int j = 0; j < flames[i].theseFlames.Length; j++)
                    {
                        flames[i].theseFlames[j].GetComponent<Animator>().Play("FlameOff");
                    }
                }
            }
        }
        
    }

    private IEnumerator FlamesLineup()
    {
        for (int i = 0; i < flames.Count; i++)
        {
            for (int j = 0; j < flames[i].theseFlames.Length; j++)
            {
                flames[i].theseFlames[j].GetComponent<Animator>().Play("FlameTrapOn", -1, normalizedTime: 0) ;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }



    [System.Serializable]
    public class Flames
    {
        public GameObject[] theseFlames;
    }
}

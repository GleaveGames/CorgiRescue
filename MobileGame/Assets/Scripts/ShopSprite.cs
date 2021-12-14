using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSprite : MonoBehaviour
{
    GameController gc;
    [HideInInspector]
    public Vector3 origin;
    [SerializeField]
    LayerMask squares;
    [HideInInspector]
    public bool beenPlaced;
    GameObject unitTextParent;
    [SerializeField]
    string UnitText1;
    [SerializeField]
    string UnitText2;
    [SerializeField]
    string UnitText3;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
        unitTextParent = transform.GetChild(0).GetChild(5).gameObject;
        unitTextParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            origin = transform.position;
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject && targetObject == this.gameObject.GetComponent<Collider2D>())
            {
                gc.draggingObj = this.gameObject;
                Collider2D square = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), squares);
                transform.GetChild(0).gameObject.SetActive(false);

                //Already On Board
                if(square != null)
                {
                    square.GetComponent<GameSquare>().occupied = false;
                    square.GetComponent<GameSquare>().occupier = null;
                }
            }
        }
    }

    public void Bought()
    {
        beenPlaced = true;
        transform.parent = transform.root;
        StartCoroutine(GetComponent<Unit>().OnBuy());
        unitTextParent.SetActive(false);
        StartCoroutine(GetComponent<Unit>().Jiggle());
    }

    public void OnMouseEnter()
    {
        /*
        if(!beenPlaced && transform.parent.name.Contains("ShopItem"))
        {
            StartCoroutine(MouseOverCheck());
        }
        else
        {
            unitTextParent.SetActive(false);
        }
        */

        if (!gc.Battling)
        {
            StartCoroutine(MouseOverCheck());
        }
    }
    public void OnMouseExit()
    {
        /*
        if(!beenPlaced && transform.parent.name.Contains("ShopItem"))
        {
            unitTextParent.SetActive(false);
        }
        */
        if (!gc.Battling)
        {
            unitTextParent.SetActive(false);
            if(!GetComponent<Unit>().actioning) StopAllCoroutines();
        }
    }

    private IEnumerator MouseOverCheck()
    {
        if (beenPlaced)
        {
            yield return new WaitForSeconds(1.2f);
        }
        else yield return new WaitForSeconds(0.6f);
        if (GetComponent<Unit>().level == 1) unitTextParent.transform.GetChild(0).GetComponent<Text>().text = UnitText1;
        if (GetComponent<Unit>().level == 2) unitTextParent.transform.GetChild(0).GetComponent<Text>().text = UnitText2;
        if (GetComponent<Unit>().level == 3) unitTextParent.transform.GetChild(0).GetComponent<Text>().text = UnitText3;
        unitTextParent.SetActive(true);
    }
}

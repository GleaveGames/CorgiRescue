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
    List<Coroutine> mouseoverchecks;
    int shopSpot = -100;
    Shop shop;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
        shop = FindObjectOfType<Shop>();
        unitTextParent = transform.GetChild(0).GetChild(3).gameObject;
        unitTextParent.SetActive(false);
        string[] UnitName = gameObject.name.Split('(');
        string name = UnitName[0];
        unitTextParent.transform.GetChild(1).GetComponent<Text>().text = name;
        mouseoverchecks = new List<Coroutine>();
        if (transform.parent.name.Contains("ShopItem"))
        {
            if (transform.parent.name.Contains("1")) shopSpot = 1;
            else if (transform.parent.name.Contains("2")) shopSpot = 2;
            else if (transform.parent.name.Contains("3")) shopSpot = 3;
            else shopSpot = 0;
        }
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
        if (shopSpot >= 0 && shop.ShopSlots[shopSpot].frozen) shop.ToggleFreeze(shopSpot);
    }

    public void OnMouseEnter()
    {
        if (!gc.Battling)
        {
            Coroutine i = StartCoroutine(MouseOverCheck());
            mouseoverchecks.Add(i);
        }
    }
    public void OnMouseExit()
    {
        if (!gc.Battling)
        {
            transform.GetChild(0).GetComponent<Canvas>().sortingOrder = 11;
            unitTextParent.SetActive(false);
            StopAllMouseOvers();
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

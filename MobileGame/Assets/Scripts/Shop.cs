using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<ShopSpot> ShopSlots;
    public List<GameObject> Units;
    GameController gc;
    [SerializeField]
    Transform BuildingParent;
    [SerializeField]
    AnimationCurve buildingY;
    [SerializeField]
    AnimationCurve buildingScaleX;
    [SerializeField]
    AnimationCurve buildingScaleY;
    [SerializeField]
    float buildingSpawnTime = 1.4f;
    [SerializeField]
    GameObject newunittext;

    List<GameObject> unitPool;
    List<GameObject> FarmUnits;
    List<GameObject> BarracksUnits;
    List<GameObject> ChurchUnits;
    List<GameObject> WorkshopUnits;
    List<GameObject> CastleUnits;

    private void Start()
    {
        unitPool = new List<GameObject>();
        FarmUnits = new List<GameObject>();
        BarracksUnits = new List<GameObject>();
        ChurchUnits = new List<GameObject>();
        WorkshopUnits = new List<GameObject>();
        CastleUnits = new List<GameObject>();

        foreach(GameObject u in Units)
        {
            int type = u.GetComponent<Unit>().quality;
            if (type == 1) FarmUnits.Add(u);
            else if (type == 2) BarracksUnits.Add(u);
            else if (type == 3) ChurchUnits.Add(u);
            else if (type == 4) WorkshopUnits.Add(u);
            else if (type == 5) CastleUnits.Add(u);
        }
        StartCoroutine(UnlockBuilding(0));

        //SHop spot GO is for the Shop spot not the occupier so need to redo code!

        ShopSlots = new List<ShopSpot>();
        gc = FindObjectOfType<GameController>();
        for (int i = 0; i < transform.childCount; i++) {
            ShopSpot newSpot = new ShopSpot();
            ShopSlots.Add(newSpot);
            newSpot.go = transform.GetChild(i).gameObject;
            newSpot.frozen = false;
            newSpot.sr = newSpot.go.transform.GetChild(1).GetComponent<SpriteRenderer>();
            newSpot.number = i;
        }
        gc.Gold++;
        ReRoll();
    }

    private void Update()
    {
        foreach(ShopSpot thisSpot in ShopSlots)
        {
            if (thisSpot.frozen) thisSpot.sr.enabled = true;
            else thisSpot.sr.enabled = false;
        }
    }

    public void ReRoll()
    {
        if (gc.Gold > 0)
        {
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (ShopSlots[i].frozen) continue;
                if (ShopSlots[i].go.transform.childCount > 2)
                {
                    Destroy(ShopSlots[i].go.transform.GetChild(2).gameObject);
                }
                GameObject unit = Instantiate(unitPool[Random.Range(0, unitPool.Count)], ShopSlots[i].go.transform.position, Quaternion.identity);
                unit.transform.parent = ShopSlots[i].go.transform;
                ShopSlots[i].unit = unit;
            }
            gc.Gold--;
        }
        else
        {
            StartCoroutine(gc.GoldJuice());
        }
    }

    public void ToggleFreeze(int spot)
    {
        ShopSpot sp = ShopSlots[spot];
        if(sp.unit == null)
        {
            if (sp.go.transform.childCount > 2) sp.unit = sp.go.transform.GetChild(2).gameObject;
        }
        if(sp.go != null && sp.unit != null)
        {
            sp.frozen = !sp.frozen;
        }
        else
        {
            sp.frozen = false;
        }
    }
    public IEnumerator UnlockBuilding(int buildingNumber)
    {
        GameObject building = BuildingParent.GetChild(buildingNumber).gameObject;
        if (buildingNumber == 0) foreach (GameObject u in FarmUnits) unitPool.Add(u);
        else if (buildingNumber == 1) foreach (GameObject u in BarracksUnits) unitPool.Add(u);
        else if(buildingNumber == 2) foreach (GameObject u in ChurchUnits) unitPool.Add(u);
        else if(buildingNumber == 3) foreach (GameObject u in WorkshopUnits) unitPool.Add(u);
        else if(buildingNumber == 4) foreach (GameObject u in CastleUnits) unitPool.Add(u);
        yield return new WaitForSeconds(1.4f);

        building.GetComponent<SpriteRenderer>().enabled = true;
        float timer = 0;
        Vector2 endpos = building.transform.position;
        bool trig = false;
        while (timer <= buildingSpawnTime)
        {
            if(!trig && timer > buildingSpawnTime / 2)
            {
                trig = true;
                Instantiate(newunittext, building.transform.position, Quaternion.identity);
            }
            building.transform.position = new Vector2(endpos.x, endpos.y + buildingY.Evaluate(timer/buildingSpawnTime));
            building.transform.localScale = new Vector2(buildingScaleX.Evaluate(timer/buildingSpawnTime), buildingScaleY.Evaluate(timer/buildingSpawnTime));
            timer += Time.deltaTime;
            yield return null;
        }

    }

}

public class ShopSpot
{
    public GameObject go;
    public bool frozen;
    public SpriteRenderer sr;
    public int number;
    public GameObject unit;
}

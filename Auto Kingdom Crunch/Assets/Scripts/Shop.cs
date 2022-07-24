﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject shopSpotPrefab;
    [SerializeField]
    Sprite specialSpotSprite;
    [SerializeField]
    Sprite ownedSprite;   
    [SerializeField]
    Sprite normalSprite;
    [SerializeField]
    AnimationCurve newSpotJuice;
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

    float musicVolumeInit;
    bool musicOn = true;
    [SerializeField]
    Sprite[] musicButtonSprites;
    [SerializeField]
    Image musicBut;

    [Header("AudioSources")]
    AudioSource music;
    [SerializeField]
    public AudioSource reroll;
    [SerializeField]
    AudioSource freeze;
    [SerializeField]
    Button QuitButton;

    private void Start()
    {
        unitPool = new List<GameObject>();
        FarmUnits = new List<GameObject>();
        BarracksUnits = new List<GameObject>();
        ChurchUnits = new List<GameObject>();
        WorkshopUnits = new List<GameObject>();
        CastleUnits = new List<GameObject>();
        music = MainMenu.Instance.GetComponent<AudioSource>();
        musicVolumeInit = 0.5f;

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

        if (!MainMenu.Instance.continuing)
        {
            ReRoll(true);
        }
        else
        {
            ContinueShop();
        }

        if(PlayerPrefs.GetInt("Music") == 0)
        {
            ToggleMusic();
        }
    }

    private void ContinueShop()
    {
        string allCharacters = null;
        foreach (GameObject u in Units)
        {
            allCharacters += u.GetComponent<Unit>().symbol;
        }
        string shopFormation = MainMenu.Instance.shopFormation;
        gc.Gold = MainMenu.Instance.gold;

        //SLOT 1
        if (shopFormation[0] != '@')
        {
            for (int z = 0; z < allCharacters.Length; z++)
            {
                if (shopFormation[0] == allCharacters[z])
                {
                    GameObject unit = Instantiate(Units[z], ShopSlots[0].go.transform.position, Quaternion.identity);
                    unit.transform.parent = ShopSlots[0].go.transform;
                }
            }
            if (shopFormation[1] == 'f')
            {
                ToggleFreeze(0);
            }
        }

        //SLOT 2
        if (shopFormation[2] != '@')
        {
            for (int z = 0; z < allCharacters.Length; z++)
            {
                if (shopFormation[2] == allCharacters[z])
                {
                    GameObject unit = Instantiate(Units[z], ShopSlots[1].go.transform.position, Quaternion.identity);
                    unit.transform.parent = ShopSlots[1].go.transform;
                }
            }
            if (shopFormation[3] == 'f')
            {
                ToggleFreeze(1);
            }
        }
        //SLOT 3
        if (shopFormation[4] != '@')
        {
            for (int z = 0; z < allCharacters.Length; z++)
            {
                if (shopFormation[4] == allCharacters[z])
                {
                    GameObject unit = Instantiate(Units[z], ShopSlots[2].go.transform.position, Quaternion.identity);
                    unit.transform.parent = ShopSlots[2].go.transform;
                }
            }
            if (shopFormation[5] == 'f')
            {
                ToggleFreeze(2);
            }
        }
        //SLOT 4
        if (shopFormation[6] != '@')
        {
            for (int z = 0; z < allCharacters.Length; z++)
            {
                if (shopFormation[6] == allCharacters[z])
                {
                    GameObject unit = Instantiate(Units[z], ShopSlots[3].go.transform.position, Quaternion.identity);
                    unit.transform.parent = ShopSlots[3].go.transform;
                }
            }
            if (shopFormation[7] == 'f')
            {
                ToggleFreeze(3);
            }
        }

    }

    private string GetShopFormation()
    {
        string shopFormation = "";
        //string shopFormation = "pnpfpnpf";
        for (int i = 0; i < 4; i++)
        {
            if (ShopSlots[i].go.transform.childCount < 3) shopFormation += "@";
            else shopFormation += ShopSlots[i].go.transform.GetChild(2).GetComponent<Unit>().symbol;
            if (ShopSlots[i].frozen) shopFormation += "f";
            else shopFormation += "n";
        }

        return shopFormation;
    }



    public void SaveShop()
    {
        MainMenu.Instance.SetShopInfo(GetShopFormation(), gc.Gold);
    }

    private void Update()
    {
        foreach(ShopSpot thisSpot in ShopSlots)
        {
            if (thisSpot.temperary && thisSpot.go.transform.childCount <= 2)
            {
                StartCoroutine(LoseSpot(thisSpot));
            }
            else if (thisSpot.temperary) continue;
            else if (thisSpot.go.transform.childCount <= 2) thisSpot.frozen = false;
            if (thisSpot.frozen) thisSpot.sr.enabled = true;
            else if(!thisSpot.temperary) thisSpot.sr.enabled = false;
        }
        for (int i = ShopSlots.Count-1; i >= 0; i--)
        {
            if (ShopSlots[i] == null || ShopSlots[i].remove) ShopSlots.RemoveAt(i);
        }
        CheckForOwned();
        if (gc.Battling)
        {
            QuitButton.interactable = false;
        }
        else
        {
            QuitButton.interactable = true;
        }
    }

    public void ReRoll(bool free = false)
    {
        if (gc.Gold > 0)
        {
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (ShopSlots[i].temperary)
                {
                    StartCoroutine(LoseSpot(ShopSlots[i]));
                    continue;
                }
                if (ShopSlots[i].frozen) continue;
                if (ShopSlots[i].go.transform.childCount > 2)
                {
                    Destroy(ShopSlots[i].go.transform.GetChild(2).gameObject);
                }
                GameObject unit = Instantiate(unitPool[Random.Range(0, unitPool.Count)], ShopSlots[i].go.transform.position, Quaternion.identity);
                unit.transform.parent = ShopSlots[i].go.transform;
            }
            if (!free)
            {
                gc.Gold--;
                reroll.Play();
            }
        }
        else
        {
            StartCoroutine(gc.GoldJuice());
        }
        SaveShop();
    }

    public void ToggleFreeze(int spot)
    {
        ShopSpot sp = ShopSlots[spot];
        if(sp.go.transform.childCount > 2)
        {
            freeze.Play();
            sp.frozen = !sp.frozen;
        }
        else
        {
            sp.frozen = false;
        }
        SaveShop();
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
        building.GetComponent<BoxCollider2D>().enabled = true;

    }

    public void ToggleMusic()
    {
        if(musicOn)
        {
            StartCoroutine(LerpMusic(music.volume, 0));
            music.volume = 0;
            musicOn = false;
            musicBut.sprite = musicButtonSprites[1];
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            StartCoroutine(LerpMusic(music.volume, 0.5f));
            music.volume = musicVolumeInit;
            musicOn = true;
            musicBut.sprite = musicButtonSprites[0];
            PlayerPrefs.SetInt("Music", 1);
        }
    }

    private IEnumerator LerpMusic(float startVol, float endVol)
    {
        float timer = 0;
        while(timer < 1)
        {
            timer += Time.deltaTime;
            music.volume = Mathf.Lerp(startVol, endVol, timer);
            yield return null;
        }
        music.volume = endVol;
    }

    public IEnumerator SpawnNewShopSpot()
    {
        Vector2 newSpotPos = ShopSlots[ShopSlots.Count - 1].go.transform.position;
        newSpotPos.x += 1.6f;

        Vector2 endPos = newSpotPos;

        newSpotPos.y -= 5f;

        GameObject newspot = Instantiate(shopSpotPrefab, newSpotPos, Quaternion.identity);
        newspot.GetComponent<SpriteRenderer>().sprite = specialSpotSprite;
        newspot.transform.parent = transform;
        newspot.transform.GetChild(0).gameObject.SetActive(false);
        int unitType = gc.round / 2 + 1;
        List<GameObject> pool = new List<GameObject>();
        if (unitType == 1) pool = BarracksUnits;
        else if (unitType == 2) pool = ChurchUnits;
        else if (unitType == 3) pool = WorkshopUnits;
        else pool = CastleUnits;
        GameObject unit = Instantiate(pool[Random.Range(0, pool.Count)], newspot.transform.position, Quaternion.identity);
        unit.transform.parent = newspot.transform;
        float timer = 0;
        while(timer<=0.6f)
        {
            newspot.transform.position = Vector2.Lerp(newSpotPos, endPos, newSpotJuice.Evaluate(timer/0.6f));
            timer += Time.deltaTime;
            yield return null;
        }
        ShopSpot thisSpot = new ShopSpot();
        thisSpot.go = newspot;
        thisSpot.number = ShopSlots[ShopSlots.Count - 1].number + 1;
        thisSpot.temperary = true;
        ShopSlots.Add(thisSpot);
        
    }

    public IEnumerator LoseSpot(ShopSpot spot)
    {
        spot.remove = true;
        Vector2 endpos = spot.go.transform.position;
        Vector2 startpos = endpos;
        endpos.y -= 5;
        float timer = 0;
        while (timer <= 0.6f)
        {
            spot.go.transform.position = Vector2.Lerp(startpos, endpos, newSpotJuice.Evaluate(timer / 0.6f));
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(spot.go);
    }

    public void CheckForOwned()
    {
        string chars = "";
        List<GameObject> list = GetPlayerUnits();
        foreach (GameObject u in list)
        {
            if(u.GetComponent<Unit>().level != 3) chars += u.GetComponent<Unit>().symbol;
        }

        foreach (ShopSpot ss in ShopSlots)
        {
            if (ss.temperary) continue;
            if (ss.go.transform.childCount > 2 && ss.go.transform.GetChild(2) != null && chars.Contains(ss.go.transform.GetChild(2).GetComponent<Unit>().symbol.ToString())) ss.go.GetComponent<SpriteRenderer>().sprite = ownedSprite;
            else ss.go.GetComponent<SpriteRenderer>().sprite = normalSprite;
        }
    }

    public List<GameObject> GetPlayerUnits()
    {
        //get player Units
        List<GameObject> playerUnits = new List<GameObject>();
        //make an array more units and enemy units

        for (int i = 3; i < gc.transform.childCount; i++)
        {
            if (gc.transform.GetChild(i).GetComponent<Unit>().playerUnit) playerUnits.Add(gc.transform.GetChild(i).gameObject);
        }

        return playerUnits;
    }

    public void Quit()
    {
        if (!gc.Battling) SceneManager.LoadScene(0);
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

}

public class ShopSpot
{
    public GameObject go;
    public bool frozen;
    public SpriteRenderer sr;
    public int number;
    public bool temperary = false;
    public bool remove = false;
}
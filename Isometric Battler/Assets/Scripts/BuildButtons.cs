using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;
public class BuildButtons : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;
    GameManager gm;
    public PlayerInput pi;
    ManaBar manabar;
    int[] buildnumber;
    [SerializeField]
    Color unaffordable;
    [SerializeField]
    Color affordable;
    int tempmana;
    public int guild;

    // Start is called before the first frame update
    void Start()
    {
        buildnumber = new int[4];
        manabar = transform.GetChild(1).GetComponent<ManaBar>();
        gm = FindObjectOfType<GameManager>();
        for(int i = 1; i < transform.childCount; i++) 
        {
            buttons.Add(transform.GetChild(i).gameObject);
        }
        for (int z = 0; z < buttons.Count; z++)
        {
            StartCoroutine(GetNewUnit(z));
        }
        CheckAffordability();
    }

    void SetBuild(int buttonNumber) 
    {
        int buildchoice = Random.Range(1, gm.guilds[guild].builds.Length);
        while (buildchoice == buildnumber[buttonNumber])
        {
            buildchoice = Random.Range(1, gm.guilds[guild].builds.Length);
        }
        buildnumber[buttonNumber] = buildchoice;
        buttons[buttonNumber].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = gm.guilds[guild].builds[buildchoice].sprite;
        buttons[buttonNumber].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = gm.guilds[guild].builds[buildchoice].cost.ToString();
        buttons[buttonNumber].transform.GetChild(0).GetComponent<Image>().color = unaffordable;
        buttons[buttonNumber].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = unaffordable;
    }

    IEnumerator GetNewUnit(int buttonNumber) 
    {
        int buildchoice = Random.Range(1, gm.guilds[guild].builds.Length);
        buildnumber[buttonNumber] = buildchoice;
        buttons[buttonNumber].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = gm.guilds[guild].builds[buildchoice].sprite;
        buttons[buttonNumber].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = gm.guilds[guild].builds[buildchoice].cost.ToString();
        float counter = 0;
        float minicounter = 0;
        while (counter < 1)
        {
            if(minicounter >= 0.05f) 
            {
                SetBuild(buttonNumber);
                minicounter = 0;
            }
            counter += Time.deltaTime;
            minicounter += Time.deltaTime;
            yield return null;
        }
        float climbingcount = 0.05f;
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i <= 8; i++) 
        {
            while (minicounter < climbingcount)
            {
                minicounter += Time.deltaTime;
                yield return null;
            }
            climbingcount *= 1.4f;
            SetBuild(buttonNumber);
            minicounter += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
        while (CheckIfButtonAlreadyExists(buildchoice, buttonNumber)) 
        {
            buildchoice = Random.Range(1, gm.guilds[guild].builds.Length);
            yield return null;
        }
        buildnumber[buttonNumber] = buildchoice;
        buttons[buttonNumber].transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = gm.guilds[guild].builds[buildchoice].sprite;
        buttons[buttonNumber].transform.GetChild(0).GetChild(0).GetComponent<Text>().text = gm.guilds[guild].builds[buildchoice].cost.ToString();
        CheckAffordability();
    }

    private bool CheckIfButtonAlreadyExists(int build, int buttonnumber) 
    {
        bool answer = false;
        for (int i = 0; i < buildnumber.Length; i++) 
        {
            if (i == buttonnumber) continue;
            else if(build == buildnumber[i]) 
            {
                answer = true;
            }
        }
        return answer;
    }

    public void ButtonPress(int buttonNumber) 
    {
        if(manabar.mana >= gm.guilds[guild].builds[buildnumber[buttonNumber]].cost) 
        {
            pi.loaded = true;
            pi.build = gm.guilds[guild].builds[buildnumber[buttonNumber]].build;
            manabar.UseMana(gm.guilds[guild].builds[buildnumber[buttonNumber]].cost);
            StartCoroutine(GetNewUnit(buttonNumber));
            CheckAffordability();
            StartCoroutine(pi.GhostBuild());
        }
        else
        {
            Debug.Log("can't afford build");
        }
    }

    public void CheckAffordability() 
    {
        Debug.Log("Checking Affordability");
        for (int i = 0; i < buttons.Count; i++)
        {
            if (manabar.mana >= gm.guilds[guild].builds[buildnumber[i]].cost)
            {
                buttons[i].transform.GetChild(0).GetComponent<Image>().color = affordable;
                buttons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = affordable;
            }
            else
            {
                buttons[i].transform.GetChild(0).GetComponent<Image>().color = unaffordable;
                buttons[i].transform.GetChild(0).GetChild(1).GetComponent<Image>().color = unaffordable;
            }
        }
        tempmana = manabar.mana;
    }
    

    //For GreyMask Stuff
    private void Update()
    {
        for(int i = 0; i < buttons.Count; i++) 
        {
            if(manabar.mana < gm.guilds[guild].builds[buildnumber[i]].cost) 
            {
                buttons[i].transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().localPosition = new Vector2(0, 60*manabar.manaFloat / gm.guilds[guild].builds[buildnumber[i]].cost);
                buttons[i].transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120 * (1 - manabar.manaFloat / gm.guilds[guild].builds[buildnumber[i]].cost));
            }
            else 
            {
                buttons[i].transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
                buttons[i].transform.GetChild(0).GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(120, 0);
            }
        }
    }
}

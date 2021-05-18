using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        buildnumber = new int[4];
        manabar = transform.GetChild(0).GetComponent<ManaBar>();
        gm = FindObjectOfType<GameManager>();
        for(int i = 1; i < transform.childCount; i++) 
        {
            buttons.Add(transform.GetChild(i).gameObject);
        }
        for (int z = 0; z < buttons.Count; z++)
        {
            int buildchoice = Random.Range(1, gm.builds.Length);
            buildnumber[z] = buildchoice;
            buttons[z].transform.GetChild(0).GetComponent<Image>().sprite = gm.builds[buildchoice].sprite;
            buttons[z].transform.GetChild(1).GetComponent<Text>().text = gm.builds[buildchoice].cost.ToString();
        }
    }

    public void ButtonPress(int buttonNumber) 
    {
        if(manabar.mana > gm.builds[buildnumber[buttonNumber]].cost) 
        {
            pi.loaded = true;
            pi.build = gm.builds[buildnumber[buttonNumber]].build;
            manabar.UseMana(gm.builds[buildnumber[buttonNumber]].cost);
            int buildchoice = Random.Range(1, gm.builds.Length);
            buildnumber[buttonNumber] = buildchoice;
            buttons[buttonNumber].transform.GetChild(0).GetComponent<Image>().sprite = gm.builds[buildchoice].sprite;
            buttons[buttonNumber].transform.GetChild(1).GetComponent<Text>().text = gm.builds[buildchoice].cost.ToString();
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
        for (int i = 0; i < buttons.Count; i++)
        {
            if (manabar.mana > gm.builds[buildnumber[i]].cost)
            {
                buttons[i].GetComponent<Image>().color = affordable;
                buttons[i].transform.GetChild(0).GetComponent<Image>().color = affordable;
            }
            else
            {
                buttons[i].GetComponent<Image>().color = unaffordable;
                buttons[i].transform.GetChild(0).GetComponent<Image>().color = unaffordable;
            }
        }
        tempmana = manabar.mana;
    }

}

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
        for(int z = 0; z < buttons.Count; z++) 
        {
            int buildchoice = Random.Range(0, gm.builds.Length);
            buildnumber[z] = buildchoice;
            buttons[z].transform.GetChild(0).GetComponent<Image>().sprite = gm.builds[buildchoice].sprite;
        }
    }

    public void ButtonPress(int buttonNumber) 
    {
        if(manabar.mana > gm.builds[buildnumber[buttonNumber]].cost) 
        {
            pi.loaded = true;
            pi.build = gm.builds[buildnumber[buttonNumber]].build;
            manabar.UseMana(gm.builds[buildnumber[buttonNumber]].cost);
        }
        else
        {
            Debug.Log("can't afford build");
        }
    }

}

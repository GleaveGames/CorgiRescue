using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public int mana;
    public float manaFloat;
    Image[] sprites;
    [SerializeField]
    Color emtpyColor;
    [SerializeField]
    Color fullColor;
    [SerializeField]
    float manaspeed;
    private BuildButtons bb;

    // Start is called before the first frame update
    void Start()
    {
        bb = transform.parent.GetComponent<BuildButtons>();
        sprites = new Image[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) 
        {
            sprites[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
        }
        StartCoroutine(ManaBuild());
    }

    private IEnumerator ManaBuild()
    {
        if (mana == transform.childCount)
        {
            StartCoroutine(ManaBuild());
            yield break;
        }
        float counter = 0;
        while(counter < manaspeed) 
        {
            manaFloat = mana + counter / manaspeed;
            sprites[mana].color = Color.Lerp(emtpyColor, fullColor, counter / manaspeed);
            counter += Time.deltaTime;
            yield return null;
        }
        mana++;
        manaFloat = mana;
        bb.CheckAffordability();
        StartCoroutine(ManaBuild());
    }

    public void UseMana(int cost) 
    {
        if(mana != transform.childCount) 
        {
            for (int i = mana; i > mana - 1 - cost && i >= 0; i--)
            {
                sprites[i].color = emtpyColor;
            }
        }
        else 
        {
            for (int i = mana-1; i > mana - cost - 1 && i >= 0; i--)
            {
                sprites[i].color = emtpyColor;
            }
        }
        mana -= cost;
        manaFloat = mana;
    }
}

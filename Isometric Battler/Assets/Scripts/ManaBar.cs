using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public int mana;
    Image[] sprites;
    [SerializeField]
    Color emtpyColor;
    [SerializeField]
    Color fullColor;
    [SerializeField]
    float manaspeed;

    // Start is called before the first frame update
    void Start()
    {
        sprites = new Image[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) 
        {
            sprites[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if mana is full then move it up slightly?
        if(sprites[transform.childCount-1].color != fullColor) 
        {
            for (int i = 0; i <transform.childCount; i++)  
            {
                if (sprites[i].color != fullColor) 
                {
                    sprites[i].color = Color.Lerp(sprites[i].color, fullColor, manaspeed * Time.deltaTime);
                    mana = i;
                    return;
                }
            }
        }
    }

    public void UseMana(int cost) 
    {
        if(mana != transform.childCount-1) 
        {
            for (int i = mana + 1; i > mana - 1 - cost; i--)
            {
                sprites[i].color = emtpyColor;
            }
        }
        else 
        {
            for (int i = mana; i > mana - cost; i--)
            {
                sprites[i].color = emtpyColor;
            }
        }
    }
}

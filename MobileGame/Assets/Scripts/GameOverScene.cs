using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverScene : MonoBehaviour
{
    public List<Image> images;
    public List<GameObject> units;
    public TextMeshProUGUI Wins;
    public TextMeshProUGUI Lives;
    public TextMeshProUGUI Round;
    [SerializeField]
    string allCharacters;
    [SerializeField]
    string formation;

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        formation = PlayerPrefs.GetString("formation");
        string[] sections = formation.Split('[');
        allCharacters = null;
        foreach (GameObject u in units)
        {
            allCharacters += u.GetComponent<Unit>().symbol;
        }

        int characterSelect = 0;
        int image = 0;
        for (int i = 0; i < sections[1].Length; i++)
        {
            for (int z = 0; z < allCharacters.Length; z++)
            {
                if (sections[1][i] == allCharacters[z])
                {
                    images[image].enabled = true;
                    characterSelect++;
                    images[image].sprite = units[z].GetComponent<SpriteRenderer>().sprite;

                    //set Stats

                    //string[] enemyStats = sections[characterSelect + 1].Split(',');
                    //newUnit.GetComponent<Unit>().level = int.Parse(enemyStats[0]);
                    //newUnit.GetComponent<Unit>().attack = int.Parse(enemyStats[1]);
                    //newUnit.GetComponent<Unit>().health = int.Parse(enemyStats[2].Substring(0, enemyStats[2].Length - 1));
                    image++;
                }
            }
        }

        Lives.text = "" + PlayerPrefs.GetInt("lives");
        Wins.text = "" + PlayerPrefs.GetInt("wins");
        Round.text = "" + PlayerPrefs.GetInt("round");

        int trophiesGained = PlayerPrefs.GetInt("wins");
        if (trophiesGained == 10) trophiesGained = 30;

        MainMenu.Instance.trophies += trophiesGained;
        MainMenu.Instance.ResetAfterGameEnd();
        MainMenu.Instance.SetDBInfo();
    }

}

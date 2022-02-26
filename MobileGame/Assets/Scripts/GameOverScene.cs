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
    [SerializeField]
    List<Sprite> confettiSprites;
    [SerializeField]
    AnimationCurve waddle;
    [SerializeField]
    Sprite king;
    [SerializeField]
    Sprite peasant;
    public float chaseTime = 0;

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

        if (PlayerPrefs.GetInt("lives") == 0)
        {
            StartCoroutine(UnitChaseLeft());
        }
        else StartCoroutine(Confetti(12));
    }

    IEnumerator Waddle(Transform t, float speed, bool left)
    {
        if (chaseTime >= 7)
        {
            Destroy(t.gameObject);
            yield break;
        }
        float waddleTime = Random.Range(0.4f, 0.6f);
        float timer = 0;
        Vector3 rot = t.eulerAngles;
        while (timer <= waddleTime)
        {
            rot.z = waddle.Evaluate(timer / waddleTime);
            t.eulerAngles = rot;
            timer += Time.deltaTime;
            if(!left) t.position += Vector3.right * (speed + Mathf.Abs(waddle.Evaluate(timer / waddleTime)) / 2);
            else t.position += Vector3.left * (speed + Mathf.Abs(waddle.Evaluate(timer / waddleTime)) / 2);
            yield return null;
        }
        StartCoroutine(Waddle(t, speed, left));
    }

    public IEnumerator Confetti(int length)
    {
        float timer = 0;
        int confettiCount = 100;
        List<Transform> confettis = new List<Transform>();
        List<Vector2> confettiDisplacements = new List<Vector2>();
        float v = GetComponent<RectTransform>().position.x + GetComponent<RectTransform>().rect.xMin;
        float minX = v;
        float maxY = GetComponent<RectTransform>().position.y + GetComponent<RectTransform>().rect.yMax;
        float z = GetComponent<RectTransform>().position.z;
        float speed = 14f;
        float depth = 0.002f;

        Vector3 topLeft = new Vector3(minX, maxY, z);
        for (int i = 0; i < confettiCount; i++)
        {
            GameObject confetti = new GameObject();
            confetti.AddComponent<Image>();
            confetti.transform.parent = transform;
            confettis.Add(confetti.transform);
            confetti.GetComponent<Image>().sprite = confettiSprites[Random.Range(0, confettiSprites.Count)];
            Vector3 spawnPos = topLeft;
            spawnPos.x += Random.Range(0, 2 * GetComponent<RectTransform>().rect.xMax);
            spawnPos.y += Random.Range(0, 700);
            confetti.transform.position = spawnPos;
            confetti.transform.localScale = new Vector3(0.2f, 0.2f, 1);
            confettiDisplacements.Add(new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000)));

        }
        while (timer <= length)
        {
            for (int i = 0; i < confettis.Count; i++)
            {
                confettis[i].position += Vector3.right * (Mathf.PerlinNoise((confettis[i].position.x + confettiDisplacements[i].x) * depth, (confettis[i].position.y + confettiDisplacements[i].y) * depth) - 0.45f) * speed;
                confettis[i].position += Vector3.down * speed * (0.3f - (Mathf.Abs(Mathf.PerlinNoise((confettis[i].position.x + confettiDisplacements[i].x) * depth, (confettis[i].position.y + confettiDisplacements[i].y) * depth) - 0.45f)));
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (Transform t in confettis)
        {
            Destroy(t.gameObject);
        }
        StartCoroutine(Confetti(length));
    }

    public IEnumerator UnitChaseLeft()
    {
        chaseTime = 0;
        float timer = 0;
        float length = 7;
        List<Transform> confettis = new List<Transform>();
        List<Vector2> confettiDisplacements = new List<Vector2>();
        float minY2 = GetComponent<RectTransform>().position.y - GetComponent<RectTransform>().rect.yMax;
        float z = GetComponent<RectTransform>().position.z;
        float v = GetComponent<RectTransform>().position.x + GetComponent<RectTransform>().rect.xMin;
        float minX = v;
        Vector3 bottomLeft = new Vector3(minX, minY2, z);
        int peasantCount = Random.Range(4, 10);



        for (int i = 0; i < peasantCount; i++)
        {
            
            GameObject confetti = new GameObject();
            confetti.AddComponent<Image>();
            confetti.transform.parent = transform;
            confettis.Add(confetti.transform);
            Vector3 spawnPos = bottomLeft;
            if (i == 0)
            {
                confetti.GetComponent<Image>().sprite = king;
                spawnPos.x = bottomLeft.x - 100;
            }
            else
            {
                confetti.GetComponent<Image>().sprite = peasant;
                spawnPos.x += Random.Range(-800, -400);
            }
            spawnPos.y += Random.Range(50, 150);
            confetti.transform.position = spawnPos;
            confetti.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(Waddle(confetti.transform, Random.Range(1.5f, 2.5f), false));
        }   
        while (timer <= length)
        {
            timer += Time.deltaTime;
            chaseTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(UnitChaseRight());
       

    }

    public IEnumerator UnitChaseRight()
    {
        chaseTime = 0;
        float timer = 0;
        float length = 7;
        List<Transform> confettis = new List<Transform>();
        List<Vector2> confettiDisplacements = new List<Vector2>();
        float minY2 = GetComponent<RectTransform>().position.y - GetComponent<RectTransform>().rect.yMax;
        float z = GetComponent<RectTransform>().position.z;
        float v = GetComponent<RectTransform>().position.x + GetComponent<RectTransform>().rect.xMax;
        float minX = v;
        Vector3 bottomLeft = new Vector3(minX, minY2, z);
        int peasantCount = Random.Range(4, 10);



        for (int i = 0; i < peasantCount; i++)
        {

            GameObject confetti = new GameObject();
            confetti.AddComponent<Image>();
            confetti.transform.parent = transform;
            confettis.Add(confetti.transform);
            Vector3 spawnPos = bottomLeft;
            if (i == 0)
            {
                confetti.GetComponent<Image>().sprite = king;
                spawnPos.x = bottomLeft.x + 100;
            }
            else
            {
                confetti.GetComponent<Image>().sprite = peasant;
                spawnPos.x += Random.Range(400, 800);
            }
            spawnPos.y += Random.Range(50, 150);
            confetti.transform.position = spawnPos;
            confetti.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(Waddle(confetti.transform, Random.Range(1.5f, 2.5f), true));
        }
        while (timer <= length)
        {
            timer += Time.deltaTime;
            chaseTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(UnitChaseLeft());

    }

}



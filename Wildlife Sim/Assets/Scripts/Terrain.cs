using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Terrain : MonoBehaviour
{

    public TextMeshProUGUI terrainText;
    Vector2Int roundedPosition;
    LevelGeneration lg;
    public Transform playerTransform;
    string currentTerrain;

    // Start is called before the first frame update
    void Start()
    {
        lg = GetComponent<LevelGeneration>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Russle
        if (lg.mapLoaded)
        {
            roundedPosition = new Vector2Int(Mathf.RoundToInt(playerTransform.position.x), Mathf.RoundToInt(playerTransform.position.y));
            if (roundedPosition.x < 0 || roundedPosition.x >= lg.mapWidth || roundedPosition.y < 0 || roundedPosition.y >= lg.mapHeight)
            {
                terrainText.text = "Out of Map";
            }
            else
            {
                currentTerrain = lg.Tiles[lg.confirmedTiles[roundedPosition.x, roundedPosition.y]].name;
                terrainText.text = currentTerrain;
            }
        }

        if (playerTransform.GetComponent<PlayerMovement>().currentlyMoving)
        {
            if (currentTerrain == "sTree" || currentTerrain == "bTree")
            {
                playerTransform.GetComponent<PlayerMovement>().speed = 2;
                GameObject currentTile = lg.tileObjects[roundedPosition.x, roundedPosition.y];

                if (currentTile.GetComponent<Animator>() == null)
                {
                    Animator animator = currentTile.AddComponent<Animator>();
                    animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("TileAnimatorController");
                    if (currentTile.GetComponent<SpriteRenderer>().sprite.name == "BigTreeA1") animator.Play("BigTreeA", 0, 0f);
                    if (currentTile.GetComponent<SpriteRenderer>().sprite.name == "BigTreeB1") animator.Play("BigTreeB", 0, 0f);
                    if (currentTile.GetComponent<SpriteRenderer>().sprite.name == "BigTreeC1") animator.Play("BigTreeC", 0, 0f);
                    if (currentTile.GetComponent<SpriteRenderer>().sprite.name == "BigTreeD1") animator.Play("BigTreeD", 0, 0f);


                    StartCoroutine(KillAnimator(animator));
                }

            }
            else playerTransform.GetComponent<PlayerMovement>().speed = 4;
        }
    }


    private IEnumerator KillAnimator(Animator animator)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.2f);
        Destroy(animator);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Animator ani;
    public Vector2Int pos;
    public LevelGeneration lg;
    public float speed;
    public float pauseTime;
    public AnimationCurve jumpAnimation;
    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    private IEnumerator Move() {
        ani.SetBool("Walk", true);
        int choice = Random.Range(1, 5);
        Vector3 movePos = transform.position;
        //up
        if(choice == 1)
        {
            Debug.Log("Up");
            if (pos.y < lg.mapHeight - 1)
            {
                movePos.z += 1;
                movePos.y = lg.heightMap[pos.x, pos.y + 1] * lg.block.transform.localScale.y + 0.5f * lg.block.transform.localScale.y;
                pos.y += 1;
                transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
            else choice += 1;
        }
        //right
        if(choice == 2)
        {
            if(pos.x < lg.mapWidth-1)
            {
                movePos.x += 1;
                movePos.y = lg.heightMap[pos.x + 1, pos.y] * lg.block.transform.localScale.y + 0.5f * lg.block.transform.localScale.y;
                pos.x += 1;
                transform.rotation = Quaternion.LookRotation(Vector3.right);
            }
            else choice += 1;
        }
        //down
        if (choice == 3)
        {
            if (pos.y > 0)
            {
                movePos.z -= 1;
                movePos.y = lg.heightMap[pos.x, pos.y - 1] * lg.block.transform.localScale.y + 0.5f * lg.block.transform.localScale.y;
                pos.y -= 1;
                transform.rotation = Quaternion.LookRotation(Vector3.back);
            }
            else choice += 1;
        }
        //left
        if (choice == 4)
        {
            if (pos.x > 0)
            {
                movePos.x -= 1;
                movePos.y = lg.heightMap[pos.x - 1, pos.y] * lg.block.transform.localScale.y + 0.5f * lg.block.transform.localScale.y;
                pos.x -= 1;
                transform.rotation = Quaternion.LookRotation(Vector3.left);
            }
            else choice = 1;
        }
        Debug.Log(transform.position - movePos);
        float timeElapsed = 0;
        Vector3 startpos = transform.position;
        Vector3 jumpPos = movePos;
        while (timeElapsed < speed)
        {
            jumpPos.y = Vector3.Lerp(startpos, movePos, timeElapsed / speed).y - 1 + jumpAnimation.Evaluate(timeElapsed / speed);
            jumpPos.x = Vector3.Lerp(startpos, movePos, timeElapsed / speed).x;
            jumpPos.z = Vector3.Lerp(startpos, movePos, timeElapsed / speed).z;
            transform.position = jumpPos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = movePos;
        ani.SetBool("Walk", false);
        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(Move());
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    int RotationDir;
    [SerializeField]
    LayerMask tiles;
    // Start is called before the first frame update
    void Start()
    {
        RotationDir = Random.Range(1, 5);
        if (RotationDir == 1) transform.up = Vector2.up;
        else if (RotationDir == 2) transform.up = Vector2.right;
        else if (RotationDir == 3) transform.up = Vector2.down;
        else if (RotationDir == 4) transform.up = Vector2.left;

        RaycastHit2D wall = ClosestWall(transform.up);
        transform.position = wall.point;
        transform.up = -transform.up;
    }

    private RaycastHit2D ClosestWall(Vector2 direction)
    {
        //working YOU MUST USE A DISTANCE FOR LAYERMASK TO WORK
        RaycastHit2D closestWall = Physics2D.Raycast(transform.position, direction, 9999, tiles);
        return closestWall;
    }
}

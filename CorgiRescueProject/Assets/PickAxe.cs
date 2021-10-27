using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxe : Miner
{
    [SerializeField]
    playerMovement pm;
    AudioSource dingSound;

    protected override void Start()
    {
        base.Start();
        dingSound = GetComponent<AudioSource>();
    }

    protected override void RockTileUpdate(string collisionSprite, UnityEngine.Vector3 hitPosition)
    {
        if (canMine)
        {
            base.RockTileUpdate(collisionSprite, hitPosition);
            canMine = false;
            pm.ChangeAnimationState("Ding");
            dingSound.Play();
        }
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (canMine)
        {
            int contactCount = collision.contactCount;
            ContactPoint2D[] contacts = new ContactPoint2D[contactCount];
            collision.GetContacts(contacts);
            if (contactCount > contacts.Length) contacts = new ContactPoint2D[contactCount];
            UnityEngine.Vector3 hitPosition = UnityEngine.Vector3.zero;
            for (int i = 0; i != contactCount; ++i)
            {
                hitPosition.x = contacts[i].point.x;
                hitPosition.y = contacts[i].point.y;
                hitPosition += (hitPosition - transform.position).normalized * 0.1f;
                string collisionSprite = null;
                //Below actually gives true for any tile in that position in any of the tilemaps.
                if (tilemaps[0].WorldToCell(hitPosition) != null)
                {
                    if (tilemaps[0].GetSprite(tilemaps[0].WorldToCell(hitPosition)) != null)
                    {
                        collisionSprite = tilemaps[0].GetSprite(tilemaps[0].WorldToCell(hitPosition)).name;
                        if (collisionSprite.Contains("Wood"))
                        {
                            canMine = false;
                            pm.ChangeAnimationState("Ding");
                            dingSound.Play();
                            break;
                        }
                    }
                }
            }
        }
        base.OnCollisionStay2D(collision);
    }
}

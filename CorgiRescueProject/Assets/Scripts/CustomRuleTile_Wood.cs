using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomRuleTile_Wood: RuleTile<CustomRuleTile_Wood.Neighbor> {

    public bool customField;
    public SibingGroup siblingGroup;
    public bool checkSelf = true;

    public enum SibingGroup
    {
        Wood,
        Rock,
        Dirt
    }


    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase other) {

        if (other.name.Contains("Wood")) return other != this;

        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            
            case TilingRule.Neighbor.This:
                {
                    return other is CustomRuleTile_Wood
                        && (other as CustomRuleTile_Wood).siblingGroup == this.siblingGroup;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !(other is CustomRuleTile_Wood
                        && (other as CustomRuleTile_Wood).siblingGroup == this.siblingGroup);
                }
        }
        return base.RuleMatch(neighbor, other);
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        if(!Application.isPlaying)
        {
            base.RefreshTile(position, tilemap);
        }
        else if (!FindObjectOfType<LevelGenerator>().finishedLoading)
        {
            base.RefreshTile(position, tilemap);
        }
        else
        {
            //base.RefreshTile(position, tilemap);
        }
    }

}

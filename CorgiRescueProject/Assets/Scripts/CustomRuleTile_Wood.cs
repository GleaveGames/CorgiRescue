using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomRuleTile_Wood: RuleTile<CustomRuleTile_Wood.Neighbor> {

    public bool customField;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }
        return base.RuleMatch(neighbor, tile);
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

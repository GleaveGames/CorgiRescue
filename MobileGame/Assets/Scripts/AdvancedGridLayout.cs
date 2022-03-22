﻿using UnityEngine;
using UnityEngine.UI;

public class AdvancedGridLayout : GridLayoutGroup
{
    [SerializeField] protected int cellsPerLine = 3;
    [SerializeField] protected float aspectRatio = 1;

    public override void SetLayoutVertical()
    {
        float width = (this.GetComponent<RectTransform>()).rect.width;
        float useableWidth = width - this.padding.horizontal - (this.cellsPerLine - 1) * this.spacing.x;
        float cellWidth = useableWidth / cellsPerLine;
        this.cellSize = new Vector2(cellWidth, cellWidth * this.aspectRatio);
        base.SetLayoutVertical();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class TruongCellSpawnerAbstract : TruongSpawner
{
    [SerializeField] protected List<Sprite> sprites;
    public List<Sprite> Sprites => sprites;
    [SerializeField] protected int cellsOnEdgeSquare;
    [SerializeField] protected float spacing;
    [SerializeField] protected TruongSquareLayout squareLayout;
    [SerializeField] protected float cellSize;
    public int CellsOnEdgeSquare => cellsOnEdgeSquare;

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        SetSprites(null);
        SetSpacing();
    }

    protected abstract void SetSpacing();

    public void SpawnWithLevel(int level)
    {
        SetSpritesWithLevel(level);
        SetCellsOnEdgeSquare(TruongVirtual.GetSquareRoot(this.sprites.Count));
        SetupSquareLayout();
        SetCellSize();
        Spawn();
        SetPositionCells();
    }

    [Button]
    protected abstract void Spawn();

    protected void SetSpritesWithLevel(int level)
    {
        var spriteList = Resources
            .LoadAll<Sprite>(Path.Combine(TruongPath.GetSpriteInResourcePath(TruongConstant.LEVEL), level.ToString()))
            .ToList();
        SetSprites(spriteList);
    }

    protected void SetSprites(List<Sprite> spriteList)
    {
        this.sprites = spriteList;
    }

    protected void SetPositionCells()
    {
        this.squareLayout.SetPositionChildren();
    }

    protected void SetCellSize()
    {
        this.cellSize = this.squareLayout.CellSize;
    }

    protected abstract void SetupSquareLayout();


    protected void SetCellsOnEdgeSquare(int value)
    {
        this.cellsOnEdgeSquare = value;
    }

    protected void SetSpacing(float value)
    {
        this.spacing = value;
    }
}
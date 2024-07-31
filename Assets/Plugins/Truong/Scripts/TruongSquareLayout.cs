using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TruongSquareLayout : TruongMonoBehaviour
{
    [TitleGroup("Input")]
    [Tooltip(
        "This point is on the edge of the square. To determine the side length of a square is 2 times the length from the center of the square to the edge")]
    [SerializeField] private Transform xPointOnEdge;
    [SerializeField] private float spacing;
    [SerializeField] private int cellsOnEdge;
    [TitleGroup("Calculations")]
    [SerializeField] private float cellSize;
    public float CellSize => cellSize;
    [SerializeField] private float edge;
    [SerializeField] private float top;
    [SerializeField] private float left;

    protected override void SetVariableToDefault()
    {
        base.SetVariableToDefault();
        SetUp(null, 0, 0);
    }

    public void SetUp(Transform xHorizontalAxisVar, int cellsOnEdgeValue, float spacingValue)
    {
        this.xPointOnEdge = xHorizontalAxisVar;
        this.cellsOnEdge = cellsOnEdgeValue;
        this.spacing = spacingValue;
        OnSetUpComplete();
    }

    private void OnSetUpComplete()
    {
        if (IsObjNull(this.xPointOnEdge)) return;
        Calculate();
    }


    [Button]
    private void Calculate()
    {
        CalculateEdge();
        CalculateCellSize();
        CalculateTopAndLeft();
    }

    private void CalculateCellSize()
    {
        this.cellSize = (this.edge - this.spacing * (this.cellsOnEdge - 1)) / cellsOnEdge;
    }

    public void SetPositionChildren()
    {
        StartCoroutine(IESetPositionChildren());
    }

    IEnumerator IESetPositionChildren()
    {
        yield return null; //Wait until the child is created before setting the position
        var index = 0;

        for (int rowIndex = 0; rowIndex < this.cellsOnEdge; rowIndex++)
        {
            if (index > this.transform.childCount - 1) yield break;
            for (int columnIndex = 0; columnIndex < this.cellsOnEdge; columnIndex++)
            {
                if (index > this.transform.childCount - 1) yield break;
                SetPositionChild(index, columnIndex, rowIndex);
                index++;
            }
        }
    }

    private void SetPositionChild(int index, int columnIndex, int rowIndex)
    {
        var child = this.transform.GetChild(index);
        child.transform.localPosition = new Vector3(columnIndex * (this.cellSize + this.spacing) - this.left,
            rowIndex * -(this.cellSize + this.spacing) + this.top, 0);
    }

    private void CalculateEdge()
    {
        var haftEdge = Vector3.Distance(this.xPointOnEdge.position, this.transform.position);
        this.edge = haftEdge * 2;
    }

    private void OnTransformChildrenChanged()
    {
        SetPositionChildren();
    }

    private void CalculateTopAndLeft()
    {
        this.left = this.top = this.edge / 2 - this.cellSize / 2;
    }
}
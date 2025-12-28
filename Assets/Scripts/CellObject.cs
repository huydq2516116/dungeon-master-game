using System;
using Unity.VisualScripting;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    private void Start()
    {
        TickManager.Instance.StartCellObject += StartCellObject;
    }
    protected virtual void StartCellObject()
    {

    }
}

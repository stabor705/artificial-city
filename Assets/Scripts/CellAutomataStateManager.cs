using System.Collections.Generic;
using UnityEngine;

public class CellAutomataStateManager : MonoBehaviour {
    public List<CellState> cells;

    public void refreshCellList() {
        foreach (Transform child in transform) {
            if (child.gameObject.name.StartsWith("Cell")) {
                cells.Add(child.gameObject.GetComponent<CellState>());
            }
        }
    }

    public void SetOccupied(int idx) {
        cells[idx].GoToOccupiedState();
    }

    public void SetEmpty(int idx) {
        cells[idx].GoToEmptyState();
    }

    public bool IsOccupied(int idx) {
        return cells[idx].GetState() == CellState.State.Occupied;
    }

    public bool IsEmpty(int idx) {
        return cells[idx].GetState() == CellState.State.Empty;
    }
}

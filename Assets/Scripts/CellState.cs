using System.Collections.Generic;
using UnityEngine;
using VectorShapes;

[RequireComponent(typeof(SpriteRenderer))]
public class CellState : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private List<Color> colors = new List<Color> {
        Color.yellow,
        Color.grey,
        Color.magenta,
        Color.cyan,
        Color.black,
        Color.blue
    };
    private Color crossingColor = Color.red;
    public enum State {
        Empty,
        Occupied
    }

    private State state = State.Empty; 

    public void GoToEmptyState() {
        state = State.Empty;
        _spriteRenderer.color = Color.white;
        _spriteRenderer.sortingOrder = 1;
    }

    public void GoToOccupiedState(int color) {
        state = State.Occupied;
        Color occupiedColor;
        if (color == -1) {
            occupiedColor = crossingColor;
        } else {
            occupiedColor = colors[color % colors.Count];
        }
        _spriteRenderer.color = occupiedColor;
        _spriteRenderer.sortingOrder = 1000;
    }

    public State GetState() {
        return state;
    }

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

using UnityEngine;
using VectorShapes;

[RequireComponent(typeof(SpriteRenderer))]
public class CellState : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    public enum State {
        Empty,
        Occupied
    }

    private State state = State.Empty; 

    public void GoToEmptyState() {
        state = State.Empty;
        _spriteRenderer.color = Color.white;
    }

    public void GoToOccupiedState() {
        state = State.Occupied;
        _spriteRenderer.color = Color.red;
    }

    public State GetState() {
        return state;
    }

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

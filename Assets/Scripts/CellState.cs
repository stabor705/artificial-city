using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellState : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public enum State {
        Empty,
        Occupied
    }

    private State state = State.Empty; 

    public void GoToEmptyState() {
        state = State.Empty;
        spriteRenderer.color = Color.white;
    }

    public void GoToOccupiedState() {
        state = State.Occupied;
        spriteRenderer.color = Color.red;
    }

    public State GetState() {
        return state;
    }
}

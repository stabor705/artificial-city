using UnityEngine;

public class GeometryMapper : MonoBehaviour {
    public float xOffset;
    public float yOffset;
    public float scale;

    public Vector2 map(PointDto point) {
        float x = (point.x - xOffset) * scale;
        float y = (point.y - yOffset) * scale;
        return new Vector2(x, y);
    }
}

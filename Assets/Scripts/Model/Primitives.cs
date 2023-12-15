using System;
using Unity.Collections;

[Serializable]
public class PointDto {
    public float x;
    public float y;

    public PointDto(float x, float y) {
        this.x = x;
        this.y = y;
    }
}

[Serializable]
public class Line {
    public PointDto start;
    public PointDto end;
}
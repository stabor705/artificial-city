using System;
using Unity.Collections;

[Serializable]
public class PointDto {
    public float x;
    public float y;
}

[Serializable]
public class Line {
    public PointDto start;
    public PointDto end;
}
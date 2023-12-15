using System;
using UnityEngine;

[Serializable]
public class NodeDto {
    public int id;
    public float lat;
    public float lon;
    public NodeTagsDto tags;
}

[Serializable]
public class NodeTagsDto {
    public string crossing;
}

public class Node {
    public int id;
    public Vector2 pos;
    public bool isCrossing;

    public Node(int id, Vector2 pos, bool isCrossing) {
        this.id = id;
        this.pos = pos;
        this.isCrossing = isCrossing;
    }

    public static Node FromDto(NodeDto dto, GeometryMapper mapper) {
        return new Node(
            dto.id,
            mapper.map(new PointDto(dto.lon, dto.lat)),
            dto.tags != null
        );
    }
}
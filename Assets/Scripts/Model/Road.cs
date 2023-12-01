using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
public class RoadDto {
    public uint id;
    public List<Line> coordinates;
}

public class Road {
    public uint id;
    public List<Vector2> geometry;

    public Road(uint id, List<Vector2> geometry)
    {
        this.id = id;
        this.geometry = geometry;
    }

    public static Road FromDto(RoadDto dto, GeometryMapper mapper) {
        List<Vector2> geometry = new List<Vector2>();
        foreach (var line in dto.coordinates) {
            geometry.Add(mapper.map(line.start));
            geometry.Add(mapper.map(line.end));
        }
        return new Road(
            dto.id,
            geometry
        );
    }
}
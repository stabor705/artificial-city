using System.Collections.Generic;
using System;
using UnityEngine;
using NagelSchreckenbergDemo.DirectedGraph;

[Serializable]
public class RoadDto {
    public uint id;
    public List<Line> coordinates;
    public string priority;
    public string oneway;
}

public class Road {
    public uint id;
    public List<Vector2> geometry;
    public Priority priority;
    public bool oneway;

    public Road(uint id, List<Vector2> geometry, Priority priority, bool oneway)
    {
        this.id = id;
        this.geometry = geometry;
        this.priority = priority;
        this.oneway = oneway;
    }

    public static Road FromDto(RoadDto dto, GeometryMapper mapper) {
        List<Vector2> geometry = new List<Vector2>();
        foreach (var line in dto.coordinates) {
            geometry.Add(mapper.map(line.start));
            geometry.Add(mapper.map(line.end));
        }
        var priority = (dto.priority == "minor") ? Priority.MINOR : Priority.MAJOR;
        var oneway = dto.oneway == "yes";
        return new Road(
            dto.id,
            geometry,
            priority,
            oneway
        );
    }
}
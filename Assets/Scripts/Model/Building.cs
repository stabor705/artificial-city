using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BuildingDto {
    public uint id;
    public string building;
    public List<PointDto> coordinates;
}

public class Building {

    public uint id;
    public List<Vector2> geometry;

    public Building(uint id, List<Vector2> geometry)
    {
        this.id = id;
        this.geometry = geometry;
    }

    public static Building FromDto(BuildingDto dto, GeometryMapper mapper) {
        return new Building(
            dto.id,
            dto.coordinates.Select(point => mapper.map(point)).ToList()
        );
    }
}
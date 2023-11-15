using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[Serializable]
class RoadDto {
    public uint id;
    public List<float> geometry_x;
    public List<float> geometry_y;
}

class Road {
    public uint id;
    public List<Vector2> geometry;

    public Road(uint id, List<Vector2> geometry)
    {
        this.id = id;
        this.geometry = geometry;
    }

    public static Road FromDto(RoadDto dto) {
        return new Road(dto.id, dto.geometry_x.Zip(dto.geometry_y, (x, y) => new Vector2(x, y)).ToList());
    }
    }
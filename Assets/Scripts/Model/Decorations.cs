using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class DecorationDto {
    public uint id;
    public List<float> geometry_x;
    public List<float> geometry_y;
    public Tags tags;
}

class Decoration {

    public uint id;
    public List<Vector2> geometry;
    public Tags tags;

    public Decoration(uint id, List<Vector2> geometry, Tags tags)
    {
        this.id = id;
        this.geometry = geometry;
        this.tags = tags;
    }

    public static Decoration FromDto(DecorationDto dto) {
        return new Decoration(dto.id, dto.geometry_x.Zip(dto.geometry_y, (x, y) => new Vector2(x, y)).ToList(), dto.tags);
    }

}
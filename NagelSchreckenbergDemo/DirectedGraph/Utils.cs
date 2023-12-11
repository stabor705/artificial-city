using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace NagelSchreckenbergDemo.DirectedGraph
{
    public enum Direction
    {
        LEFT,
        STRAIGHT,
        RIGHT,
        UNKNOWN
    }

    public enum Priority
    {
        MAJOR,
        MINOR
    }

    public static class ReservationDistance
    {
        public const int MAJOR_STRAIGHT = 30;
        public const int MAJOR_RIGHT = 30;
        public const int MAJOR_LEFT = 20;
        public const int MINOR_RIGHT = 20;
        public const int MINOR_STRAIGHT = 10;
        public const int MINOR_LEFT = 10;

        public static int GetReservationDistance(Direction direction, Priority priority)
        {
            return direction switch {
                Direction.LEFT => priority == Priority.MAJOR ? MAJOR_LEFT : MINOR_LEFT,
                Direction.RIGHT => priority == Priority.MAJOR ? MAJOR_RIGHT : MINOR_RIGHT,
                Direction.STRAIGHT => priority == Priority.MAJOR ? MAJOR_STRAIGHT : MINOR_STRAIGHT,
                _ => 0
            };
        }
    }

    public static class VertexState
    {
        // UInt16 representation of state of Vertex
        // unused x 7
        // Major road straight
        // Major road right
        // Major road left
        // Minor road right
        // Minor road straight
        // Minor road left
        // unused x 2
        // Free
        public const ushort MAJOR_STRAIGHT = 256;
        public const ushort MAJOR_RIGHT = 128;
        public const ushort MAJOR_LEFT = 64;
        public const ushort MINOR_RIGHT = 32;
        public const ushort MINOR_STRAIGHT = 16;
        public const ushort MINOR_LEFT = 8;

        public static ushort SetState(ushort previousState, ushort stateToSet)
        {
            return (ushort)(previousState | stateToSet);
        }

        public static ushort UnsetState(ushort previousState, ushort stateToUnset)
        {
            return (ushort)(previousState & ~stateToUnset);
        }

        public static ushort GetState(Direction direction, Priority priority)
        {
            return direction switch {
                Direction.LEFT => priority == Priority.MAJOR ? MAJOR_LEFT : MINOR_LEFT,
                Direction.RIGHT => priority == Priority.MAJOR ? MAJOR_RIGHT : MINOR_RIGHT,
                Direction.STRAIGHT => priority == Priority.MAJOR ? MAJOR_STRAIGHT : MINOR_STRAIGHT,
                _ => 0
            };
        }

        public static bool IsAvailable(ushort state, Direction direction, Priority priority)
        {
            return direction switch {
                Direction.LEFT => priority == Priority.MAJOR ? state < MAJOR_RIGHT : state < MINOR_STRAIGHT,
                Direction.RIGHT => priority == Priority.MAJOR || state < MAJOR_LEFT,
                Direction.STRAIGHT => priority == Priority.MAJOR || state < MINOR_RIGHT,
                _ => false
            };
        }
    }
}

using NagelSchreckenbergDemo;
using NagelSchreckenbergDemo.DirectedGraph;
using System;
using System.Diagnostics;

namespace NagelSchreckenbergDemo
{
    public class CalculateDirections
    {
        private static double CrossProduct(Edge currentEdge, Edge nextEdge)
        {
            var (currX, currY, currZ) = GetVectorCoordinates(currentEdge);
            var (nextX, nextY, nextZ) = GetVectorCoordinates(nextEdge);

            double resultX = currY * nextZ - nextY * currZ;
            double resultY = currZ * nextX - nextZ * currX;
            double resultZ = currX * nextY - nextX * currY;

            return resultX + resultY + resultZ;
        }

        private static (double, double, double) GetVectorCoordinates(Edge edge)
        {
            Vertex end = edge.endV;
            Vertex start = edge.startV;
            return (end.lng - start.lng, end.lat - start.lat, end.z - start.z);
        }

        private static double GetVectorLength(double x, double y, double z)
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static double GetAngle(Edge currentEdge, Edge nextEdge)
        {
            var (currentEdgeX, currentEdgeY, currentEdgeZ) = GetVectorCoordinates(currentEdge);
            var (nextEdgeX, nextEdgeY, nextEdgeZ) = GetVectorCoordinates(nextEdge);

            return CrossProduct(currentEdge, nextEdge) /
                (GetVectorLength(currentEdgeX, currentEdgeY, currentEdgeZ) *
                GetVectorLength(nextEdgeX, nextEdgeY, nextEdgeZ));
        }
    }
}

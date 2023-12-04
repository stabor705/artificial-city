using System.Globalization;
using NagelSchreckenbergDemo.DirectedGraph;
using System;
using System.Text;

namespace NagelSchreckenbergDemo
{
    public class Vehicle
    {
        public int id;
        public int length;

        public Edge edge;
        public Edge? nextEdge;
        public Direction nextEdgeDirection;

        public int velocity = 0;
        public int toDeleteCountdown = -1;

        public Vehicle(int length, Edge edge)
        {
            this.id = TrafficSimulation.nextVehicleIndex;
            TrafficSimulation.nextVehicleIndex++;
            this.length = length;
            this.edge = edge;
            for (int i = 0; i < length; i++)
                edge.cells[i] = id;

            this.nextEdge = this.PickNewEdge();
        }

        private void IncreaseVelocity()
        {
            if (velocity < 6)
                this.velocity++;
        }

        private void DecreaseVelocity(int velocity)
        {
            this.velocity = Math.Max(0, this.velocity - velocity);
        }

        private void RandomlyDecreaseVelocity()
        {
            if (new Random().NextDouble() >= 0.5)
                this.DecreaseVelocity(1);
        }

        private int SpaceInFront()
        {
            int index = this.FrontPosition() + 1;
            int start = index;

            while (index < this.edge.length && this.edge.cells[index] == 0)
                index++;

            /*
            TODO Here goes the logic for setting the state for current edge's end Vertex
            */
            if (index - start == ReservationDistance.GetReservationDistance(nextEdgeDirection, this.edge.priority))
            {
                // TODO console log
                this.edge.endV.state = VertexState.SetState(
                    this.edge.endV.state,
                    VertexState.GetState(nextEdgeDirection, this.edge.priority)
                );
            }

            if (
                nextEdge is not null
                && index >= this.edge.length
                && nextEdge.startV.IsAvailable(this.nextEdgeDirection, this.edge.priority)
            )
            {
                int nextIndex = index - this.edge.length;
                // Console.WriteLine("New edge " + id + " " + nextIndex);
                while (nextIndex < this.nextEdge.length && this.nextEdge.cells[nextIndex] == 0)
                {
                    nextIndex++;
                    index++;
                }
            }

            return index - start;
        }

        private void EvaluateVelocity()
        {
            int spaceInFront = SpaceInFront();
            // Console.WriteLine("Space in front of vehicle " + this.id + ": " + spaceInFront);
            if (this.velocity < spaceInFront)
                this.IncreaseVelocity();
            else
                this.DecreaseVelocity(this.velocity - spaceInFront);
            // TODO maybe decrease v not that rapidly? maybe (v - space) / 2?

            this.RandomlyDecreaseVelocity();
        }

        private void MoveOneCell()
        {
            // Console.WriteLine("vehicle: " + this.id + " front position: " + this.FrontPosition());
            if (this.nextEdge is not null && this.FrontPosition() >= this.edge.length + this.length - 1)
            {
                ChangeEdge();
                return;
            }

            if (this.velocity == 0)
                return;

            if (this.FrontPosition() + 1 < this.edge.cells.Length)
                this.edge.cells[this.FrontPosition() + 1] = id;
            else if (nextEdge is not null)
                this.nextEdge.cells[this.FrontPosition() - this.edge.cells.Length + 1] = id;

            int backPosition = this.BackPosition();
            // Console.WriteLine("current edge: " + this.edge.id + " " + string.Join("", this.edge.cells));
            if (backPosition != -1)
                this.edge.cells[backPosition] = 0;

            if (this.nextEdge is null && this.FrontPosition() >= this.edge.length - 1)
            {
                toDeleteCountdown = 30;
                return;
            }
        }

        public void SingleStep(int time)
        {
            if ((this.velocity * time % 60) == 0)
            {
                EvaluateVelocity();
                // Console.WriteLine("vehicle: " + this.id + " velocity: " + this.velocity + " edge: " + this.edge.id);
                MoveOneCell();
            }

        }

        private int BackPosition()
        {
            return this.edge.GetIndexOfVehicle(this.id);
        }

        private int FrontPosition()
        {
            if (this.edge.GetIndexOfVehicle(this.id) == -1)
                return this.edge.length + this.length - 1;

            return this.edge.GetIndexOfVehicle(this.id) + this.length - 1;
        }

        private Direction determineDirection(List<Edge> candidates, Edge pickedEdge)
        {
            if (candidates.Count == 1)
            {
                return Direction.STRAIGHT;
            }
            else if (candidates.Count == 2)
            {
                int index = candidates.OrderBy(
                    e => -CalculateDirections.GetAngle(edge, e)
                    ).ToList().IndexOf(pickedEdge);
                return index switch
                {
                    0 => Direction.LEFT,
                    1 => Direction.RIGHT,
                    _ => Direction.UNKNOWN,
                };
            }
            else if (candidates.Count == 3)
            {
                int index = candidates.OrderBy(
                    e => -CalculateDirections.GetAngle(edge, e)
                    ).ToList().IndexOf(pickedEdge);
                return index switch
                {
                    0 => Direction.LEFT,
                    1 => Direction.STRAIGHT,
                    2 => Direction.RIGHT,
                    _ => Direction.UNKNOWN,
                };
            }
            else
            {
                return Direction.UNKNOWN;
            }
        }

        private Edge? PickNewEdge()
        {
            List<Edge>? candidates = new List<Edge>(this.edge.endV.OutEdges);
            candidates.RemoveAll(edge => edge.endV == this.edge.startV);
            if (candidates is null || candidates.Count == 0)
            {
                this.nextEdgeDirection = Direction.UNKNOWN;
                return null;
            }
            // Console.WriteLine("Candidates for vehicle " + this.id + ": " + string.Join("", edge.id));

            int candidateIndex = new Random().Next(0, candidates.Count);
            Edge pickedEdge = candidates[candidateIndex];
            this.nextEdgeDirection = determineDirection(candidates, pickedEdge);
            Console.WriteLine("Selected next edge id: " + candidates[candidateIndex].id);
            Console.WriteLine("Next direction is: " + this.nextEdgeDirection);
            return pickedEdge;
        }

        private void ChangeEdge()
        {
            Console.WriteLine("changing old edge: " + this.edge + " to next edge: " + this.nextEdge);
            this.edge.RemoveVehicle(this);

            this.edge.endV.state = VertexState.UnsetState(
                this.edge.endV.state,
                VertexState.GetState(nextEdgeDirection, this.edge.priority)
            );

            if (this.nextEdge is null) throw new Exception("There is no next edge for " + this);
            this.nextEdge.AddVehicle(this);

            this.edge = this.nextEdge;
            Edge? nE = PickNewEdge();
            this.nextEdge = nE;
            Console.WriteLine("change completed - current edge: " + this.edge + " and new selected next edge: " + this.nextEdge);
        }

        public override string ToString()
        {
            return string.Format("Vehicle {0}: length {1}, velocity: {2} on edge: {3}", id, length, velocity, edge);
        }
    }
}

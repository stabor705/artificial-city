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

        public Edge? edge;
        public Edge? nextEdge;

        public int velocity = 0;
        public bool toDelete = false;

        public Vehicle(int id, int length, Edge edge)
        {
            this.id = id;
            this.length = length;
            this.edge = edge;
            for (int i = 0; i < length; i++)
                edge.cells[i] = id;

            this.nextEdge = this.PickNewEdge();
            edge.AddVehicle(this);
        }

        private void IncreaseVelocity()
        {
            if (velocity < 7)
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

            while (index < this.edge.cells.Length && this.edge.cells[index] == 0)
                index++;

            if (index == this.edge.cells.Length && nextEdge is not null)
            {
                int nextIndex = 0;
                while (nextIndex < this.nextEdge.cells.Length && this.nextEdge.cells[nextIndex] == 0)
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
            Console.WriteLine(spaceInFront);
            if (this.velocity < spaceInFront)
                this.IncreaseVelocity();
            else
                this.DecreaseVelocity(this.velocity - spaceInFront);
            // TODO maybe decrease v not that rapidly? maybe (v - space) / 2?

            this.RandomlyDecreaseVelocity();
        }

        private void MakeMove()
        {
            Console.WriteLine("velocity " + this.velocity + "edge: " + this.edge.id);
            if (this.velocity == 0 && this.FrontPosition() + 1 == this.edge.cells.Length)
            {
                toDelete = true;
                return;
            }
            for (int i = 0; i < this.velocity; i++)
            {
                MoveOneCell();
                if (toDelete)
                    return;
            }
        }

        private void MoveOneCell()
        {
            if (this.FrontPosition() + 1 < this.edge.cells.Length)
                this.edge.cells[this.FrontPosition() + 1] = id;
            else if (nextEdge is not null)
            {
                Console.WriteLine("no jest kurwa not null");
                this.nextEdge.cells[this.FrontPosition() - this.edge.cells.Length + 1] = id;
            }

            int backPosition = this.BackPosition();
            Console.WriteLine("aaaaa" + backPosition);
            if (backPosition != -1)
                this.edge.cells[backPosition] = 0;
            else
                ChangeEdge();
        }

        public void SingleStep()
        {
            EvaluateVelocity();
            MakeMove();
        }

        private int BackPosition()
        {
            return this.edge.GetIndexOfVehicle(this.id);
        }

        private int FrontPosition()
        {
            return this.edge.GetIndexOfVehicle(this.id) + this.length - 1;
        }

        private Edge? PickNewEdge()
        {
            List<Edge>? candidates = this.edge.endV.OutEdges;
            if (candidates is null || candidates.Count == 0)
                return null;

            int candidateIndex = new Random().Next(0, candidates.Count);
            return candidates[candidateIndex];
        }

        private void ChangeEdge()
        {
            this.edge.RemoveVehicle(this);
            this.nextEdge.AddVehicle(this);

            Edge? nextEdge = PickNewEdge();
            this.edge = this.nextEdge;
            this.nextEdge = nextEdge;
        }
    }
}

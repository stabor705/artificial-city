using NagelSchreckenbergDemo.DirectedGraph;
using System;

namespace NagelSchreckenbergDemo
{
    public class Vehicle
    {
        public int id;
        public int length;

        // References to Graph's values
        public Vertex? startPoint;
        public Vertex? endPoint;
        public Edge? edge;

        // distance from the beginning of the Edge
        public int distance;

        public int velocity = 0;

        public int[] cells;

        public Vehicle(int id, int length, ref int[] cells) 
        {
            this.id = id;
            this.length = length;
            this.distance = length - 1;
            this.cells = cells;
            for (int i = 0; i < length; i++)
                cells[i] = id;
        }

        public Vehicle(int id, int length, ref Vertex startPoint, ref Vertex endPoint, Edge edge, int distance, int velocity)
        {
            this.id = id;
            this.length = length;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.edge = edge;
            this.distance = distance;
            this.velocity = velocity;
        }
    
        private void IncreaseVelocity()
        {
            if (velocity == 7) return;
            this.velocity += 1;
        }

        private void DecreaseVelocity(int velocity)
        {
            if (this.velocity == 0) return;
            if (velocity == 0) return;
            if (this.velocity < velocity)
            {
                this.velocity = 0;
                return;
            }
            this.velocity -= velocity;
        }
        private void RandomlyDecreaseVelocity()
        {
            if (this.velocity == 0) return;
            if (new Random().NextDouble() >= 0.5)
                this.DecreaseVelocity(1);
        }
        private void EvaluateVelocity()
        {
            var spaceInFront = cells.TakeWhile(x => x == 0).Count();
            if (this.velocity < spaceInFront)
                this.IncreaseVelocity();
            else
                this.DecreaseVelocity(this.velocity - spaceInFront);
            // TODO maybe decrease v not that rapidly? maybe (v - space) / 2?

            this.RandomlyDecreaseVelocity();
        }

        private bool IsOnCrossroads()
        {
            return this.distance == this.cells.Length - 1;
        }

        private void TurnOnCrossroads()
        {
            if (new Random().NextDouble() < 0.5) return;

            // TODO
        }

        private void MakeMove()
        {
            var newDistance = this.distance + this.velocity;
            for (int i = 0; i < this.length; i++)
            {
                if (this.cells[this.distance + this.velocity] != 0)
                    Console.WriteLine("sth is no yes");

                if (newDistance - i < this.cells.Length)
                    this.cells[newDistance - i] = this.id;

                if (this.distance - i < this.cells.Length)
                    this.cells[this.distance - i] = 0;
            }
            this.distance += this.velocity;
        }

        public void SingleStep()
        {
            if (IsOnCrossroads()) TurnOnCrossroads();

            EvaluateVelocity();

            MakeMove();
        }
    }
}

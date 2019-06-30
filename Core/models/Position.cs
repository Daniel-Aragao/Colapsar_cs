using System;


namespace Core.models
{
    public class Position
    {
        public double X { get;}
        public double Y { get;}
        public Func<Position, Position, double> DistanceFunction { get;}

        public Position(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.DistanceFunction = Position.EucledeanDistance;
        }

        public Position(double x, double y,Func<Position, Position, double> distanceFunction)
        {
            this.X = x;
            this.Y = y;
            this.DistanceFunction = distanceFunction;
        }

        public static double EucledeanDistance(Position source, Position target)
        {
            return Math.Sqrt(Math.Pow(source.X - target.X, 2) + Math.Pow(source.Y - target.Y, 2));
        }

        public static double StraightLineOnEarthDistance(Position source, Position target)
        {
            throw new NotImplementedException();
        }

    }
}
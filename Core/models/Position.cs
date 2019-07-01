using System;
using GeoCoordinatePortable;
using Core.extensions;


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

        public double Distance(Position target)
        {
            return this.DistanceFunction(this, target);
        }

        public static double EucledeanDistance(Position source, Position target)
        {
            return Math.Sqrt(Math.Pow(source.X - target.X, 2) + Math.Pow(source.Y - target.Y, 2));
        }

        public static double HaversineDistance(Position source, Position target)
        {
            return HaversineDistance(source, target, 6371);
        }
        public static double HaversineDistance(Position source, Position target, double radius)
        {
            var latSource = source.Y.ToRadians();
            var latTarget = target.Y.ToRadians();
            var deltaLat = (source.Y - target.Y).ToRadians();
            var deltaLon = (source.X - target.X).ToRadians();

            var a = Math.Pow(Math.Sin(deltaLat / 2), 2);
            var b = a + (Math.Cos(latSource) * Math.Cos(latTarget) * Math.Pow(deltaLon / 2, 2));
            var c = 2 * Math.Asin(Math.Sqrt(b));
            
            var distance =  radius * c;
            
            return distance;
        }

        private static GeoCoordinate GenerateGeoCoodinateFromPosition(Position pos)
        {
            return new GeoCoordinate(pos.Y, pos.X);
        }

        public static double GeoCoordinateDistance(Position source, Position target)
        {
            var sourceGeo = GenerateGeoCoodinateFromPosition(source);
            var targetGeo = GenerateGeoCoodinateFromPosition(target);

            return sourceGeo.GetDistanceTo(targetGeo) / 1000;
        }

    }
}
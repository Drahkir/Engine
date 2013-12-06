using System.Collections.Generic;

namespace Engine
{
    public class Spline
    {
        private readonly List<Vector> _points = new List<Vector>();
        private double _segmentSize;

        public void AddPoint(Vector point)
        {
            _points.Add(point);
            _segmentSize = 1/(double) _points.Count;
        }

        private int LimitPoints(int point)
        {
            if (point < 0)
            {
                return 0;
            }
            if (point > _points.Count - 1)
            {
                return _points.Count - 1;
            }
            return point;
        }

        // t ranges from 0 - 1
        public Vector GetPositionOnLine(double t)
        {
            if (_points.Count <= 1)
            {
                return new Vector(0, 0, 0);
            }

            // Get the segment of the line we're dealing with.
            var interval = (int) (t/_segmentSize);

            // Get the points around the segment
            int p0 = LimitPoints(interval - 1);
            int p1 = LimitPoints(interval);
            int p2 = LimitPoints(interval + 1);
            int p3 = LimitPoints(interval + 2);

            // Scale t to the current segment
            double scaledT = (t - _segmentSize*interval)/_segmentSize;
            return CalculateCatmullRom(scaledT, _points[p0], _points[p1], _points[p2], _points[p3]);
        }

        private Vector CalculateCatmullRom(double t, Vector p1, Vector p2, Vector p3, Vector p4)
        {
            double t2 = t*t;
            double t3 = t2*t;

            double b1 = 0.5*(-t3 + 2*t2 - t);
            double b2 = 0.5*(3*t3 - 5*t2 + 2);
            double b3 = 0.5*(-3*t3 + 4*t2 + t);
            double b4 = 0.5*(t3 - t2);

            return (p1*b1 + p2*b2 + p3*b3 + p4*b4);
        }
    }
}
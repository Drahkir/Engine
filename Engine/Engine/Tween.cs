using System;

namespace Engine
{
    public class Tween
    {
        public delegate double TweenFunction(double timePassed, double start, double distance, double duration);

        private double _current;
        private double _distance;
        private bool _finished;
        private double _original;
        private double _totalDuration = 5;
        private double _totalTimePassed;
        private TweenFunction _tweenF;

        public Tween(double start, double end, double time)
        {
            Construct(start, end, time, Linear);
        }

        public Tween(double start, double end, double time, TweenFunction tweenF)
        {
            Construct(start, end, time, tweenF);
        }

        public double Value()
        {
            return _current;
        }

        public bool IsFinished()
        {
            return _finished;
        }

        public static double Linear(double timePassed, double start, double distance, double duration)
        {
            return distance*timePassed/duration + start;
        }

        public void Construct(double start, double end, double time, TweenFunction tweenF)
        {
            _distance = end - start;
            _original = start;
            _current = start;
            _totalDuration = time;
            _tweenF = tweenF;
        }

        public void Update(double elapsedTime)
        {
            _totalTimePassed += elapsedTime;
            _current = _tweenF(_totalTimePassed, _original, _distance, _totalDuration);

            if (_totalTimePassed > _totalDuration)
            {
                _current = _original + _distance;
                _finished = true;
            }
        }

        public static double EaseOutExpo(double timePassed, double start, double distance, double duration)
        {
            if (timePassed == duration)
            {
                return start + distance;
            }

            return distance*(-Math.Pow(2, -10*timePassed/duration) + 1) + start;
        }

        public static double EaseInExpo(double timePassed, double start, double distance, double duration)
        {
            if (timePassed == 0)
            {
                return start;
            }
            return distance*Math.Pow(2, 10*(timePassed/duration - 1)) + start;
        }

        public static double EaseOutCirc(double timePassed, double start, double distance, double duration)
        {
            return distance*Math.Sqrt(1 - (timePassed = timePassed/duration - 1)*timePassed) + start;
        }

        public static double EaseInCirc(double timePassed, double start, double distance, double duration)
        {
            return -distance*(Math.Sqrt(1 - (timePassed /= duration)*timePassed) - 1) + start;
        }
    }
}
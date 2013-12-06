using System.Runtime.InteropServices;
using System.Security;

namespace Engine
{
    public class PreciseTimer
    {
        private readonly long _ticksPerSecond;
        private long _previousElapsedTime;

        public PreciseTimer()
        {
            QueryPerformanceFrequency(ref _ticksPerSecond);
            GetElapsedTime(); // Get rid of first rubbish result
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceCounter(ref long PerformanceCount);

        public double GetElapsedTime()
        {
            long time = 0;
            QueryPerformanceCounter(ref time);

            double elapsedTime = (time - _previousElapsedTime)/(double) _ticksPerSecond;
            _previousElapsedTime = time;

            return elapsedTime;
        }
    }
}
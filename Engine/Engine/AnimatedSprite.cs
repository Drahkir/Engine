namespace Engine
{
    public class AnimatedSprite : Sprite
    {
        private int _currentFrame;
        private double _currentFrameTime = 0.03;
        private int _framesX;
        private int _framesY;

        public AnimatedSprite()
        {
            Looping = false;
            Finished = false;
            AnimatedSpeed = 0.03; // 30 fps
            _currentFrameTime = Speed;
        }

        public double AnimatedSpeed { get; set; } // Seconds per frame
        public bool Looping { get; set; }
        public bool Finished { get; set; }

        public System.Drawing.Point GetIndexFromFrame(int frame)
        {
            var point = new System.Drawing.Point {Y = frame/_framesX};
            point.X = frame - (point.Y*_framesY);
            return point;
        }

        private void UpdateUVs()
        {
            System.Drawing.Point index = GetIndexFromFrame(_currentFrame);
            float frameWidth = 1.0f/_framesX;
            float frameHeight = 1.0f/_framesY;
            SetUVs(new Point(index.X*frameWidth, index.Y*frameHeight),
                new Point((index.X + 1)*frameWidth, (index.Y + 1)*frameHeight));
        }

        public void SetAnimation(int framesX, int framesY)
        {
            _framesX = framesX;
            _framesY = framesY;
            UpdateUVs();
        }

        private int GetFrameCount()
        {
            return _framesX*_framesY;
        }

        public void AdvanceFrame()
        {
            int numberOfFrames = GetFrameCount();
            _currentFrame = (_currentFrame + 1)%numberOfFrames;
        }

        public int GetCurrentFrame()
        {
            return _currentFrame;
        }

        public void Update(double elapsedTime)
        {
            if (_currentFrame == GetFrameCount() - 1 && Looping == false)
            {
                Finished = true;
                return;
            }

            _currentFrameTime -= elapsedTime;
            if (!(_currentFrameTime < 0)) return;
            AdvanceFrame();
            _currentFrameTime = Speed;
            UpdateUVs();
        }
    }
}
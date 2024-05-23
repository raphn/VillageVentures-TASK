using System;


namespace VillageVentures
{
    public enum Location { Village, Shop, Saloon };

    public class CountDownTimer
    {
        private float _time = 0;
        private float _timer = 0;
        private Action<float> _onUpdated;
        private Action _onTimeout;

        /// <summary>
        /// From 0.0 to 1.0 how far along is the timer
        /// </summary>
        public float Progress => _timer/_time;

        public CountDownTimer(float time, Action onTimeout, Action<float> onUpdated=null)
        {
            _time = time;
            _timer = 0;
            _onTimeout = onTimeout;
            _onUpdated = onUpdated;
        }

        public void Update(float delta)
        {
            _timer += delta;
            _onUpdated?.Invoke(Progress);

            if (_timer > _time)
                Timeout();
        }
        
        private void Timeout()
        {
            _onTimeout?.Invoke();

            // Cleanup references
            _onTimeout = null;
            _onUpdated = null;
        }
    }
}
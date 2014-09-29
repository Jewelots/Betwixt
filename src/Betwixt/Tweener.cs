using Betwixt.Annotations;

using System;

namespace Betwixt
{
    /// <summary>
    /// Ease function that takes a percent and returns a scaled (eased) percent
    /// </summary>
    /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
    /// <returns>Scaled percent according to the ease function</returns>
    public delegate float EaseFunc(float percent);

    /// <summary>
    /// Lerp function to designate how to interpolate between <paramref name="start"/> and <paramref name="end"/>
    /// </summary>
    /// <typeparam name="T">Generic type to interpolate</typeparam>
    /// <param name="start">Start to interpolate from (0%)</param>
    /// <param name="end">End to interpolate to (100%)</param>
    /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
    /// <returns>Interpolation from start to end, percent of the way in between</returns>
    public delegate T LerpFunc<T>(T start, T end, float percent);

    ///////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Tweener to help tween between two values
    /// </summary>
    /// <typeparam name="T">Type to tween</typeparam>
    /// <example>
    /// <para>
    /// General Use:
    /// <code>
    /// // Initialisation
    /// Tweener&lt;float&gt; tweener = new Tweener&lt;float&gt;(0, 10, 2, Ease.Elastic.Out);
    /// // Update
    /// tweener.Update(deltaTime);
    /// // Anywhere
    /// float newValue = tweener.Value;
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// You can also use your own custom type, with it's own lerp function (or let generics handle it)
    /// <code>
    /// Tweener&lt;Vector2&gt; tweener = new Tweener&lt;Vector2&gt;(startVector, endVector, TimeSpan.FromSeconds(3), Ease.Linear, Vector2.Lerp);
    /// </code>
    /// </para>
    /// 
    /// <para>
    /// You can also specify your own ease function and make it into a set (or use the function directly)
    /// <code>
    /// IEase myEaseSet = Generic.CreateFromOut(myEaseOutFunction);
    /// Tweener&lt;float&gt; tweener = new Tweener&lt;float&gt;(0, 10, 2, myEaseSet.InOut);
    /// </code>
    /// </para>
    /// </example>
    ///////////////////////////////////////////////////////////////////////////
    [UsedImplicitly]
    public class Tweener<T>
    {
        /// <summary>
        /// Create a new Tweener
        /// </summary>
        /// <param name="start">Value to start at</param>
        /// <param name="end">Target value</param>
        /// <param name="duration">Time (in seconds) to get to the target</param>
        /// <param name="easeFunc">Ease function to use (defaults to linear if unspecified)</param>
        /// <param name="lerpFunc">Lerp function to use (defaults to generic if unspecified)</param>
        public Tweener(T start, T end, double duration, EaseFunc easeFunc = null, LerpFunc<T> lerpFunc = null)
        {
            _elapsed = 0.0f;
            _start = start;
            _end = end;
            _duration = duration;

            // If there's no ease function specified, use Linear
            _easeFunc = easeFunc ?? Ease.Linear;

            // If there's no lerp function specified, use Generic Default
            _lerpFunc = lerpFunc ?? LerpFuncDefault;

            Value = _start;
            Running = true;
        }

        /// <summary>
        /// Create a new Tweener
        /// </summary>
        /// <param name="start">Value to start at</param>
        /// <param name="end">Target value</param>
        /// <param name="duration">Time (in seconds) to get to the target</param>
        /// <param name="easeFunc">Ease function to use (defaults to linear if unspecified)</param>
        /// <param name="lerpFunc">Lerp function to use (defaults to generic if unspecified)</param>
        public Tweener(T start, T end, float duration, EaseFunc easeFunc = null, LerpFunc<T> lerpFunc = null)
            : this(start, end, (double)duration, easeFunc, lerpFunc)
        {
        }

        /// <summary>
        /// Create a new Tweener
        /// </summary>
        /// <param name="start">Value to start at</param>
        /// <param name="end">Target value</param>
        /// <param name="duration">How long to get to the target</param>
        /// <param name="easeFunc">Ease function to use (defaults to linear if unspecified)</param>
        /// <param name="lerpFunc">Lerp function to use (defaults to generic if unspecified)</param>
        public Tweener(T start, T end, TimeSpan duration, EaseFunc easeFunc = null, LerpFunc<T> lerpFunc = null)
            : this(start, end, duration.TotalSeconds, easeFunc, lerpFunc)
        {
        }

        #region Properties
        /// <summary>
        /// Value of the current tween
        /// </summary>
        [UsedImplicitly] public T Value { get; private set; }

        /// <summary>
        /// Bool designating if the Tweener is running or not
        /// </summary>
        [UsedImplicitly] public bool Running { get; private set; }

        [UsedImplicitly] private T _start;
        [UsedImplicitly] private T _end;
        [UsedImplicitly] private double _elapsed;
        [UsedImplicitly] private double _duration;
        [UsedImplicitly] private EaseFunc _easeFunc;
        [UsedImplicitly] private LerpFunc<T> _lerpFunc;

        /// <summary>
        /// Delegate called when the Tweener is finished
        /// </summary>
        /// <param name="sender">The Tweener who called the event</param>
        /// <param name="e"></param>
        public delegate void OnEndHandler(object sender, EventArgs e);

        /// <summary>
        /// Handler that's called when the Tweener is finished (not when stopped manually)
        /// </summary>
        [UsedImplicitly] public event OnEndHandler OnEnd;
        #endregion

        #region Methods
        /// <summary>
        /// Update the Tweener
        /// </summary>
        /// <param name="deltaTime">Elapsed time between frame (in seconds)</param>
        [UsedImplicitly]
        public void Update(double deltaTime)
        {
            // Don't update if not running
            if (!Running) return;

            _elapsed += deltaTime;

            // Stop the Tween if it's finished
            if (_elapsed >= _duration)
            {
                _elapsed = _duration;
                Value = Calculate(_start, _end, 1, _easeFunc, _lerpFunc); // Set it to end point

                Stop();
                Ended();

                return;
            }

            // Calculate new value based on current Lerp percent
            Value = Calculate(_start, _end, (float)(_elapsed / _duration), _easeFunc, _lerpFunc);
        }

        /// <summary>
        /// Update the Tweener
        /// </summary>
        /// <param name="deltaTime">Elapsed time between frame (in seconds)</param>
        [UsedImplicitly]
        public void Update(float deltaTime)
        {
            Update((double)deltaTime);
        }

        /// <summary>
        /// If there's no lerp function specified, use Generic Math to calculate start + ((end - start) * percent)
        /// </summary>
        private static T LerpFuncDefault(T start, T end, float percent)
        {
            return GenericMath.Add(start, GenericMath.Multiply(GenericMath.Subtract(end, start), percent));
        }

        /// <summary>
        /// Calculate the new value using the Ease and Lerp functions
        /// </summary>
        /// <param name="start">Value to start at</param>
        /// <param name="end">Target Value</param>
        /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
        /// <param name="easeFunc">Function to use when Easing</param>
        /// <param name="lerpFunc">Function to use when Interpolating</param>
        /// <returns>Eased value</returns>
        private static T Calculate(T start, T end, float percent, EaseFunc easeFunc, LerpFunc<T> lerpFunc)
        {
            // Scale the percent based on the ease
            float scaledPercent = easeFunc(percent);
            
            // Pass in scaled percent to interpolation
            return lerpFunc(start, end, scaledPercent);
        }

        private void Ended()
        {
            if (OnEnd != null)
            {
                OnEnd(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Start the Tweener
        /// </summary>
        [UsedImplicitly]
        public void Start()
        {
            Running = true;
        }

        /// <summary>
        /// Stop the Tweener
        /// </summary>
        [UsedImplicitly]
        public void Stop()
        {
            Running = false;
        }

        /// <summary>
        /// Reset the Tweener (moves back to beginning)
        /// </summary>
        [UsedImplicitly]
        public void Reset()
        {
            _elapsed = 0;
            Value = _start;
        }

        /// <summary>
        /// Reset the Tweener to move to new value (current location is new start location)
        /// </summary>
        /// <param name="to">New value to move towards</param>
        [UsedImplicitly]
        public void Reset(T to)
        {
            _elapsed = 0;
            _start = Value;
            _end = to;
        }

        /// <summary>
        /// Reset the tweener to go from End to Start instead (and resets to beginning)
        /// </summary>
        [UsedImplicitly]
        public void Reverse()
        {
            _elapsed = 0;

            T tmp = _end;
            _end = _start;
            _start = tmp;
        }

        /// <summary>
        /// Overrides ToString operator
        /// </summary>
        /// <returns>Formatted string containing Tweener info</returns>
        public override string ToString()
        {
            return String.Format("{0}.{1}.\n{2}.{3}.\nTween {4} -> {5} in {6}s. _elapsed {7:##0.##}s",
                (_easeFunc.Method.DeclaringType != null) ? _easeFunc.Method.DeclaringType.Name : "null",
                _easeFunc.Method.Name,
                (_lerpFunc.Method.DeclaringType != null) ? _lerpFunc.Method.DeclaringType.Name : "null",
                _lerpFunc.Method.Name,
                _start, 
                _end, 
                _duration, 
                _elapsed);
        }
        #endregion
    }
}

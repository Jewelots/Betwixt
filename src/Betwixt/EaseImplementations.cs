using System;

using Betwixt.Annotations;

namespace Betwixt
{
    /// <summary>
    /// Generic Implementation to help create IEase function sets, and pipes ease functions to eachother automatically
    /// </summary>
    [UsedImplicitly]
    internal class GenericImpl : IEase
    {
        private readonly EaseFunc _easeInFunc;
        private readonly EaseFunc _easeOutFunc;
        private readonly EaseFunc _easeInOutFunc;

        private GenericImpl(EaseFunc easeInFunc, EaseFunc easeOutFunc, EaseFunc easeInOutFunc)
        {
            if (easeInFunc == null && easeOutFunc == null)
            {
                throw new Exception("Both in and out arguments none, this should not happen! This is bad.");
            }

            // If there's no In function, create one generically (from Out)
            _easeInFunc    = easeInFunc    ?? GenericIn;

            // If there's no Out function, create one generically (from In)
            _easeOutFunc   = easeOutFunc   ?? GenericOut;

            // If there's no InOut function, create one generically (from Out)
            _easeInOutFunc = easeInOutFunc ?? GenericInOut;
        }

        /// <summary>
        /// Create a new Generic Implementation set from an Out ease, and an optional InOut ease.
        /// </summary>
        /// <param name="easeOutFunc">Out ease function</param>
        /// <param name="easeInOutFunc">Optional InOut ease function</param>
        /// <returns>A new Generic Ease Set</returns>
        public static GenericImpl FromOut(EaseFunc easeOutFunc, EaseFunc easeInOutFunc = null)
        {
            return new GenericImpl(null, easeOutFunc, easeInOutFunc);
        }

        /// <summary>
        /// Create a new Generic Implementation set from an In ease, and an optional InOut ease.
        /// </summary>
        /// <param name="easeInFunc">In ease function</param>
        /// <param name="easeInOutFunc">Optional InOut ease function</param>
        /// <returns>A new Generic Ease Set</returns>
        public static GenericImpl FromIn(EaseFunc easeInFunc, EaseFunc easeInOutFunc = null)
        {
            return new GenericImpl(easeInFunc, null, easeInOutFunc);
        }

        /// <summary>
        /// Create a new Generic Implementation set from a full set of ease functions, and an optional InOut ease.
        /// </summary>
        /// <param name="easeInFunc">In ease function</param>
        /// <param name="easeOutFunc">Out ease function</param>
        /// <param name="easeInOutFunc">Optional InOut ease function</param>
        /// <returns>A new Generic Ease Set</returns>
        public static GenericImpl From(EaseFunc easeInFunc, EaseFunc easeOutFunc, EaseFunc easeInOutFunc = null)
        {
            return new GenericImpl(easeInFunc, easeOutFunc, easeInOutFunc);
        }

        #region Generic Non-Specified Functions
        /// <summary>
        /// Create an In ease from an Out ease
        /// </summary>
        private float GenericIn(float percent)
        {
            return Ease.Generic.Reverse(percent, Out);
        }

        /// <summary>
        /// Create an Out ease from an In ease
        /// </summary>
        private float GenericOut(float percent)
        {
            return Ease.Generic.Reverse(percent, In);
        }

        /// <summary>
        /// Create an InOut ease from an Out ease
        /// </summary>
        private float GenericInOut(float percent)
        {
            return Ease.Generic.InOut(percent, Out);
        }
        #endregion

        /// <summary>
        /// Call the stored Ease In function
        /// </summary>
        public float In(float percent)
        {
            return _easeInFunc(percent);
        }

        /// <summary>
        /// Call the stored Ease Out function
        /// </summary>
        public float Out(float percent)
        {
            return _easeOutFunc(percent);
        }

        /// <summary>
        /// Call the stored Ease InOut function
        /// </summary>
        public float InOut(float percent)
        {
            return _easeInOutFunc(percent);
        }
    }

    // Below are implementations.
    // The implementations only specify an In or an Out (depending on which is easier to write)
    // And the rest of the set is created via Generic Ease Creation

    #region Ease Implementations
    /// <summary>
    /// Implementation of Quadratic Ease
    /// </summary>
    internal static class QuadraticImpl
    {
        public static float In(float percent)
        {
            return (float)Math.Pow(percent, 2);
        }
    }

    /// <summary>
    /// Implementation of Cubic Ease
    /// </summary>
    internal static class CubicImpl
    {
        public static float In(float percent)
        {
            return (float)Math.Pow(percent, 3);
        }
    }

    /// <summary>
    /// Implementation of Quartic Ease
    /// </summary>
    internal static class QuarticImpl
    {
        public static float In(float percent)
        {
            return (float)Math.Pow(percent, 4);
        }
    }

    /// <summary>
    /// Implementation of Quintic Ease
    /// </summary>
    internal static class QuinticImpl
    {
        public static float In(float percent)
        {
            return (float)Math.Pow(percent, 5);
        }
    }

    /// <summary>
    /// Implementation of Sine Ease
    /// </summary>
    internal static class SineImpl
    {
        public static float Out(float percent)
        {
            return (float)Math.Sin(percent * (Math.PI / 2));
        }
    }

    /// <summary>
    /// Implementation of Exponential Ease
    /// </summary>
    internal static class ExponentialImpl
    {
        public static float Out(float percent)
        {
            return (float)Math.Pow(2, 10 * (percent - 1));
        }
    }

    /// <summary>
    /// Implementation of Circlic Ease
    /// </summary>
    internal static class CircImpl
    {
        public static float Out(float percent)
        {
            return (float)Math.Sqrt(1 - Math.Pow(percent - 1, 2));
        }
    }

    /// <summary>
    /// Implementation of "Back" Ease
    /// </summary>
    internal static class BackImpl
    {
        public static float In(float percent)
        {
            const float s = 1.70158f;
            return (float)Math.Pow(percent, 2) * ((s + 1) * percent - s);
        }
    }

    /// <summary>
    /// Implementation of "Elastic" Ease
    /// </summary>
    [UsedImplicitly]
    internal static class ElasticImpl
    {
        public static float Out(float percent)
        {
            return (float)(1 + Math.Pow(2, -10 * percent) * Math.Sin((percent - 0.075) * (2 * Math.PI) / 0.3));
        }
    }

    /// <summary>
    /// Implementation of "Bounce" Ease
    /// </summary>
    [UsedImplicitly]
    internal static class BounceImpl
    {
        public static float Out(float percent)
        {
            const float s = 7.5625f;
            const float p = 2.75f;

            if (percent < (1 / p))
            {
                return (float)(s * Math.Pow(percent, 2));
            }

            if (percent < (2 / p))
            {
                percent -= (1.5f / p);
                return (float)(s * Math.Pow(percent, 2) + .75);
            }

            if (percent < (2.5f / p))
            {
                percent -= (2.25f / p);
                return (float)(s * Math.Pow(percent, 2) + .9375);
            }

            percent -= (2.625f / p);
            return (float)(s * Math.Pow(percent, 2) + .984375);
        }
    }
    #endregion
}

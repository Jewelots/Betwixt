using System;

using Betwixt.Annotations;

namespace Betwixt
{
    /// <summary>
    /// Generic Implementation to help create IEase function sets, and pipes ease functions to eachother automatically
    /// </summary>
    [UsedImplicitly]
    internal class GenericEaseImpl : IEase
    {
        private readonly EaseFunc _easeInFunc;
        private readonly EaseFunc _easeOutFunc;
        private readonly EaseFunc _easeInOutFunc;

        private GenericEaseImpl(EaseFunc easeInFunc, EaseFunc easeOutFunc, EaseFunc easeInOutFunc)
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
        public static GenericEaseImpl FromOut(EaseFunc easeOutFunc, EaseFunc easeInOutFunc = null)
        {
            return new GenericEaseImpl(null, easeOutFunc, easeInOutFunc);
        }

        /// <summary>
        /// Create a new Generic Implementation set from an In ease, and an optional InOut ease.
        /// </summary>
        /// <param name="easeInFunc">In ease function</param>
        /// <param name="easeInOutFunc">Optional InOut ease function</param>
        /// <returns>A new Generic Ease Set</returns>
        public static GenericEaseImpl FromIn(EaseFunc easeInFunc, EaseFunc easeInOutFunc = null)
        {
            return new GenericEaseImpl(easeInFunc, null, easeInOutFunc);
        }

        /// <summary>
        /// Create a new Generic Implementation set from a full set of ease functions, and an optional InOut ease.
        /// </summary>
        /// <param name="easeInFunc">In ease function</param>
        /// <param name="easeOutFunc">Out ease function</param>
        /// <param name="easeInOutFunc">Optional InOut ease function</param>
        /// <returns>A new Generic Ease Set</returns>
        public static GenericEaseImpl From(EaseFunc easeInFunc, EaseFunc easeOutFunc, EaseFunc easeInOutFunc = null)
        {
            return new GenericEaseImpl(easeInFunc, easeOutFunc, easeInOutFunc);
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
}
using Betwixt.Annotations;

namespace Betwixt
{
    /// <summary>
    /// Interface that provides easy use of In, Out, and InOut easing that a standard ease "set" provides
    /// </summary>
    /// <remarks>
    /// IEase isn't strictly neccesary at all, but it does help give the ability to swap out and
    /// store which "set" to use in an object.
    /// </remarks>
    /// <example>
    /// <code>
    /// IEase easeFunc = Ease.Cubic;
    /// // Use easeFunc.Out in another method
    /// easeFunc = Ease.Quad;
    /// </code>
    /// </example>
    public interface IEase
    {
        /// <summary>
        /// Ease In by a percent
        /// </summary>
        /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
        /// <returns>Eased percent value</returns>
        [UsedImplicitly]
        float In(float percent);

        /// <summary>
        /// Ease Out by a percent
        /// </summary>
        /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
        /// <returns>Eased percent value</returns>
        [UsedImplicitly]
        float Out(float percent);

        /// <summary>
        /// Ease InOut by a percent
        /// </summary>
        /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
        /// <returns>Eased percent value</returns>
        [UsedImplicitly]
        float InOut(float percent);
    }

    /// <summary>
    /// Holder class namespace to store each ease type.
    /// </summary>
    public static class Ease
    {
        /// <summary>
        /// Quadratic ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Quad = Generic.CreateFromIn(QuadraticImpl.In);

        /// <summary>
        /// Cubic ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Cubic = Generic.CreateFromIn(CubicImpl.In);

        /// <summary>
        /// Quartic ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Quart = Generic.CreateFromIn(QuarticImpl.In);

        /// <summary>
        /// Quintic ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Quint = Generic.CreateFromIn(QuinticImpl.In);

        /// <summary>
        /// Sine ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Sine = Generic.CreateFromOut(SineImpl.Out);

        /// <summary>
        /// Exponential ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Expo = Generic.CreateFromOut(ExponentialImpl.Out);

        /// <summary>
        /// Circlic ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Circ = Generic.CreateFromOut(CircImpl.Out);

        /// <summary>
        /// "Back" ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Back = Generic.CreateFromIn(BackImpl.In);

        /// <summary>
        /// "Elastic" ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Elastic = Generic.CreateFromOut(ElasticImpl.Out);

        /// <summary>
        /// "Bounce" ease set
        /// </summary>
        [UsedImplicitly]
        public static readonly IEase Bounce = Generic.CreateFromOut(BounceImpl.Out);

        /// <summary>
        /// Linear ease equation
        /// </summary>
        [UsedImplicitly]
        public static float Linear(float percent)
        {
            return percent;
        }

        /// <summary>
        /// Generic class can be used for creating a full IEase set from one or two scale functions,
        /// and also provides helper functions for creating these other ease types
        /// </summary>
        public static class Generic
        {
            /// <summary>
            /// Reverse an ease function to go from Out -> In, or In -> Out
            /// </summary>
            /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
            /// <param name="easeFunc">Ease function to reverse</param>
            /// <returns>The reverse of the usual output from easeFunc</returns>
            public static float Reverse(float percent, EaseFunc easeFunc)
            {
                return 1 - easeFunc(1 - percent);
            }

            /// <summary>
            /// Transform an ease function by reversing it and appending it to itself to create an InOut from an Out
            /// </summary>
            /// <param name="percent">Progress along ease function where 0-1 is 0%-100%</param>
            /// <param name="Out">Out Ease function to create InOut from (must be OUT)</param>
            /// <returns>An InOut ease function</returns>
            public static float InOut(float percent, EaseFunc Out)
            {
                // If less than halfway
                if (percent < 0.5)
                {
                    // Reverse the Out to create an In, and scale it down by half
                    return Reverse(percent * 2, Out) / 2;
                }

                // Shift over the out to the halfway point and scale it down by half
                return (Out(percent * 2 - 1) / 2) + 0.5f;
            }

            /// <summary>
            /// Create a full IEase set from an In ease, and an optional InOut ease.
            /// </summary>
            /// <param name="easeInFunc">In ease function</param>
            /// <param name="easeInOutFunc">Optional InOut ease function</param>
            /// <returns>A full IEase set</returns>
            [UsedImplicitly]
            public static IEase CreateFromIn(EaseFunc easeInFunc, EaseFunc easeInOutFunc = null)
            {
                return GenericImpl.FromIn(easeInFunc, easeInOutFunc);
            }

            /// <summary>
            /// Create a full IEase set from an Out ease, and an optional InOut ease.
            /// </summary>
            /// <param name="easeOutFunc">Out ease function</param>
            /// <param name="easeInOutFunc">Optional InOut ease function</param>
            /// <returns>A full IEase set</returns>
            [UsedImplicitly]
            public static IEase CreateFromOut(EaseFunc easeOutFunc, EaseFunc easeInOutFunc = null)
            {
                return GenericImpl.FromOut(easeOutFunc, easeInOutFunc);
            }

            /// <summary>
            /// Create a full IEase set from a full set of ease functions, and an optional InOut ease.
            /// </summary>
            /// <remarks>
            /// Filling in all 3 arguments might be better off as you creating your own class inheriting from IEase
            /// But there's no real downside to doing it this way
            /// </remarks>
            /// <param name="easeInFunc">In ease function</param>
            /// <param name="easeOutFunc">Out ease function</param>
            /// <param name="easeInOutFunc">Optional InOut ease function</param>
            /// <returns>A full IEase set</returns>
            [UsedImplicitly]
            public static IEase Create(EaseFunc easeInFunc, EaseFunc easeOutFunc, EaseFunc easeInOutFunc = null)
            {
                return GenericImpl.From(easeInFunc, easeOutFunc, easeInOutFunc);
            }
        }
    }
}

using System;

namespace FF12RngHelper.RNGHelper
{
    /// <summary>
    /// Represents the state of the Mersenne Twister RNG
    /// </summary>
    public struct RNGState
    {
        /// <summary>
        /// Index into the mt state array.
        /// </summary>
        public int mti;

        /// <summary>
        /// The mt state array.
        /// </summary>
        public UInt32[] mt;

        public int position;

    }
}

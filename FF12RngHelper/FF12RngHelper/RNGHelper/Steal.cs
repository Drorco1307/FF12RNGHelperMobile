using System;
using System.Collections.Generic;
using System.Text;

namespace FF12RngHelper.RNGHelper
{
    public static class Steal
    {
        #region Members
        // Strings for display in the UI
        private const string RARE = "Rare";
        private const string UNCOMMON = "Uncommon";
        private const string COMMON = "Common";
        private const string NONE = "None";
        private const string LINKER = " + ";

        // Steal chances
        private const int CommonChance = 55;
        private const int UncommonChance = 10;
        private const int RareChance = 3;
        private const int CommonChanceCuffs = 80;
        private const int UncommonChanceCuffs = 30;
        private const int RareChanceCuffs = 6;
        #endregion

        #region Properties

        #endregion

        #region Constructors

        #endregion

        #region Methods
        /// <summary>
        /// Check if you steal anything while not wearing the Thief's Cuffs
        /// When not wearing Thief's Cuffs you may only steal one item.
        /// Once you are successful, you get that item and that's it.
        /// </summary>
        public static string CheckSteal(uint prng1, uint prng2, uint prng3)
        {
            if (StealSuccessful(prng1, RareChance))
            {
                return RARE;
            }
            if (StealSuccessful(prng2, UncommonChance))
            {
                return UNCOMMON;
            }
            if (StealSuccessful(prng3, CommonChance))
            {
                return COMMON;
            }
            return NONE;
        }

        /// <summary>
        /// Check if you steal anything while wearing the Thief's Cuffs
        /// When not wearing Thief's Cuffs you may steal more than one
        /// item, and you have better odds. Roll against all 3 and get
        /// everything you successfully steal.
        /// </summary>
        public static string CheckStealCuffs(uint prng1, uint prng2, uint prng3)
        {
            string returnStr = string.Empty;

            if (StealSuccessful(prng1, RareChanceCuffs))
            {
                returnStr += RARE;
            }
            if (StealSuccessful(prng2, UncommonChanceCuffs))
            {
                returnStr += LINKER + UNCOMMON;
            }
            if (StealSuccessful(prng3, CommonChanceCuffs))
            {
                returnStr += LINKER + COMMON;
            }
            if (returnStr == string.Empty)
            {
                returnStr = NONE;
            }
            return returnStr.TrimStart(LINKER.ToCharArray());
        }

        /// <summary>
        /// Calculate if a steal attempt was successful
        /// </summary>
        private static bool StealSuccessful(uint prng, int chance)
        {
            return RandToPercent(prng) < chance;
        }

        /// <summary>
        /// Convert an RNG value into a percentage
        /// </summary>
        private static int RandToPercent(uint toConvert)
        {
            return (int)(toConvert % 100);
        }
        #endregion

        #region Misc

        #endregion
    }
}

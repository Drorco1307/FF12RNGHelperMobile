using System;
using System.Collections.Generic;
using System.Text;

namespace FF12RngHelper
{
    public class RngDataViewModel:BaseViewModel
    {
        #region string Position
        private int mPosition;
        /// <summary>
        /// The position int the rng
        /// </summary>
        public int Position
        {
            get { return mPosition; }
            set
            {
                if (mPosition == value)
                    return;
                mPosition = value;
                NotifyPropertyChanged(nameof(Position));
            }
        }
        #endregion

        #region string Value
        private int mValue;
        /// <summary>
        /// The heal value
        /// </summary>
        public int Value
        {
            get { return mValue; }
            set
            {
                if (mValue == value)
                    return;
                mValue = value;
                NotifyPropertyChanged(nameof(Value));
            }
        }
        #endregion

        #region string Percent
        private double mPercent;
        /// <summary>
        /// chest item percantage
        /// </summary>
        public double Percent
        {
            get { return mPercent; }
            set
            {
                if (mPercent == value)
                    return;
                mPercent = value;
                NotifyPropertyChanged(nameof(Percent));
            }
        }
        #endregion

        #region string OneIn256
        private bool mOneIn256;
        /// <summary>
        /// I have no idea what this is :), but it's in the original ff12rnghelper so...
        /// </summary>
        public bool OneIn256
        {
            get { return mOneIn256; }
            set
            {
                if (mOneIn256 == value)
                    return;
                mOneIn256 = value;
                NotifyPropertyChanged(nameof(OneIn256));
            }
        }
        #endregion

        #region string Steal
        private string mSteal;
        /// <summary>
        /// Represents the ...
        /// </summary>
        public string Steal
        {
            get { return mSteal; }
            set
            {
                if (mSteal == value)
                    return;
                mSteal = value;
                NotifyPropertyChanged(nameof(Steal));
            }
        }
        #endregion

        #region string StealCuffs
        private string mStealCuffs;
        /// <summary>
        /// Represents the ...
        /// </summary>
        public string StealCuffs
        {
            get { return mStealCuffs; }
            set
            {
                if (mStealCuffs == value)
                    return;
                mStealCuffs = value;
                NotifyPropertyChanged(nameof(StealCuffs));
            }
        }
        #endregion
    }
}

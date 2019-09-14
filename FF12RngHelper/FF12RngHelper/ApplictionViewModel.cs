using FF12RngHelper.DataModel;
using FF12RngHelper.RNGHelper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FF12RngHelper
{
    public class ApplictionViewModel : BaseViewModel
    {
        #region Members
        private double? m_level;
        private double? m_mag;
        private SpellTypes m_selectedSpell;
        private bool m_isBusy;
        private PlatformsTypes m_selectedPlatform;
        private Page mPage;
        private const int SEARCH_BUFFER_SIZE = 1000000;
        private int mIndex;	// Current index in the PRNG list
        private const int HISTORY_TO_DISPLAY = 5;
        private const int FIND_NEXT_TIMEOUT = 60;
        private IRNG mSearchRng;
        private IRNG mDispRng;
        private CircularBuffer<uint> mSearchBuff;	// buffer of PRNG numbers
        private List<int> mHealVals;  // List of heal values input by user
        private CharacterGroup mGroup = new CharacterGroup();
        private const int MAX_SEARCH_INDEX_SUPPORTED = (int)1e7; // 10 million
        private bool m_isSerenity;
        private int m_lastHeal;
        private ObservableCollection<RngDataViewModel> mRngGridData;
        private RngDataViewModel mSelectedGridRow;
        #endregion

        #region Properties

        #region double Level
        /// <summary>
        /// The level of the character used
        /// </summary>
        public double? Level
        {
            get { return m_level; }
            set
            {
                if (m_level == value)
                    return;
                m_level = value;
                NotifyPropertyChanged(nameof(Level));
            }
        }
        #endregion

        #region double Mag
        /// <summary>
        /// The magic power of the character used
        /// </summary>
        public double? Mag
        {
            get { return m_mag; }
            set
            {
                if (m_mag == value)
                    return;
                m_mag = value;
                NotifyPropertyChanged(nameof(Mag));
            }
        }
        #endregion

        #region Spell SelectedSpell
        /// <summary>
        /// The selected spell to use in rng generation
        /// </summary>
        public SpellTypes SelectedSpell
        {
            get { return m_selectedSpell; }
            set
            {
                if (m_selectedSpell == value)
                    return;
                m_selectedSpell = value;
                NotifyPropertyChanged(nameof(SelectedSpell));
            }
        }
        #endregion

        #region string SelectedPlatform
        /// <summary>
        /// The selected platroms to use in rng generation
        /// </summary>
        public PlatformsTypes SelectedPlatform
        {
            get { return m_selectedPlatform; }
            set
            {
                if (m_selectedPlatform == value)
                    return;
                m_selectedPlatform = value;
                NotifyPropertyChanged(nameof(SelectedPlatform));
            }
        }
        #endregion

        #region bool IsBusy
        /// <summary>
        /// True when the rng is calculating
        /// </summary>
        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                if (m_isBusy == value)
                    return;
                m_isBusy = value;
                NotifyPropertyChanged(nameof(IsBusy));
            }
        }
        #endregion

        #region bool IsSerenity
        /// <summary>
        /// A multiplierflag of the rng. False = 1, True = 1.5
        /// </summary>
        public bool IsSerenity
        {
            get { return m_isSerenity; }
            set
            {
                if (m_isSerenity == value)
                    return;
                m_isSerenity = value;
                NotifyPropertyChanged(nameof(IsSerenity));
            }
        }
        #endregion

        #region uint LastHeal
        /// <summary>
        /// The value of the last heal performed.
        /// </summary>
        public int LastHeal
        {
            get { return m_lastHeal; }
            set
            {
                if (m_lastHeal == value)
                    return;
                m_lastHeal = value;
                NotifyPropertyChanged(nameof(LastHeal));
            }
        }
        #endregion

        #region RngDataViewModel RngGridData
        /// <summary>
        /// The actual data for the rng
        /// </summary>
        public ObservableCollection<RngDataViewModel> RngGridData
        {
            get { return mRngGridData; }
            set
            {
                if (mRngGridData == value)
                    return;
                mRngGridData = value;
            }
        }
        #endregion

        #region RngDataViewModel SelectedGridRow
        /// <summary>
        /// When rng found a potision, should be set to this row. (currently not working)
        /// </summary>
        public RngDataViewModel SelectedGridRow
        {
            get { return mSelectedGridRow; }
            set
            {
                if (mSelectedGridRow == value)
                    return;
                mSelectedGridRow = value;
                NotifyPropertyChanged(nameof(SelectedGridRow));
            }
        }
        #endregion

        /// <summary>
        /// List of available spells to cast
        /// </summary>
        public List<SpellTypes> Spells => new List<SpellTypes>
        {
            SpellTypes.Cure,
            SpellTypes.Cura,
            SpellTypes.Curaga,
            SpellTypes.Curaja,
            SpellTypes.CuraIzjsTza,
            SpellTypes.CuragaIzjsTza,
            SpellTypes.CurajaIzjsTza
        };

        /// <summary>
        /// List of platforms to use.
        /// </summary>
        public List<PlatformsTypes> Platforms => new List<PlatformsTypes>()
        {
            PlatformsTypes.PS2,
            PlatformsTypes.PS4
        };

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="page">The page that's bound to this view model</param>
        public ApplictionViewModel(Page page)
        {
            // Initialize commands
            ContinueCommand = new RelayCommand(async () => await ContinueAsync());
            BeginSearchCommand = new RelayCommand(async () => await BeginSearchAsync());

            // Initialize view model members and properties
            RngGridData = new ObservableCollection<RngDataViewModel>();
            mHealVals = new List<int>();
            mSearchRng = InitializeRNG();
            mDispRng = InitializeRNG();
            mPage = page;

            // Dummy character stats
            Level = 3;
            Mag = 21;
            LastHeal = 81;
            SelectedSpell = SpellTypes.Cure;

            // load character stats
            mGroup.AddCharacter(new Character(Level ?? 0, Mag ?? 0, SelectedSpell, IsSerenity));
        }
        #endregion

        #region Methods
        /// <summary>
        /// generate Initial rng data based on heal value entered.
        /// </summary>
        /// <returns></returns>
        private async Task BeginSearchAsync()
        {
            await RunCommandAsync(() => IsBusy, async () =>
             {
                 // TODO: make all this logic asyncronous.
                 mGroup.ResetIndex();
                 mHealVals.Clear();
                 mSearchBuff = new CircularBuffer<uint>(SEARCH_BUFFER_SIZE);
                 mSearchRng.sgenrand();
                 mSearchBuff.Add(mSearchRng.genrand());
                 mIndex = 0;
                 if (!FindNext(LastHeal))
                     await mPage.DisplayAlert("Alert", "Impossible Heal Value entered", "OK");
                 else
                     DisplayRNG(mIndex, mIndex + 1000, true);
             });
        }

        /// <summary>
        /// Continue rng search based on last heal value and current heal value
        /// </summary>
        /// <returns></returns>
        private async Task ContinueAsync()
        {
            await RunCommandAsync(() => IsBusy, async () =>
             {
                 // TODO: make all this logic asyncronous.
                 int groupIndex_temp = mGroup.GetIndex();
                 int index_temp = mIndex;
                 List<int> healVals_temp = new List<int>(mHealVals);
                 CircularBuffer<uint> searchBuff_temp = mSearchBuff.DeepClone();
                 IRNG rng_temp = mSearchRng.DeepClone();
                 mGroup.IncrimentIndex();
                 if (!FindNext(LastHeal))
                 {
                     mGroup.SetIndex(groupIndex_temp);
                     mIndex = index_temp;
                     mHealVals = healVals_temp;
                     mSearchBuff = searchBuff_temp;
                     mSearchRng = rng_temp;
                     await mPage.DisplayAlert("Alert", "Impossible Heal Value entered", "OK");
                 }
                 else
                     DisplayRNG(mIndex - mHealVals.Count + 1, mIndex + 1000);
             });
        }

        /// <summary>
        /// The purpose of this method is to find our next spot in the RNG
        /// </summary>
        /// <param name="value">New heal value to process</param>
        private bool FindNext(int value)
        {
            // Do a range check before trying this out to avoid entering an infinite loop.
            if (!mGroup.ValidateHealValue(value))
                return false;

            // Store the current character while searching:
            int indexStatic = mGroup.GetIndex();

            // Add the given heal value to the heal list.
            mHealVals.Add(value);
            mIndex++;

            // Pull an extra PRNG draw to see whether it matches.
            mSearchBuff.Add(mSearchRng.genrand());

            // Otherwise, continue moving through the RNG to find the next matching position
            bool match = false;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (!match)
            {
                // Quit if it's taking too long.
                if (timer.Elapsed.TotalSeconds > FIND_NEXT_TIMEOUT || mIndex > MAX_SEARCH_INDEX_SUPPORTED)
                {
                    timer.Stop();
                    //return false;
                }

                mGroup.ResetIndex();
                for (int i = 0; i < mHealVals.Count; i++)
                {
                    // index of first heal:
                    int index0 = mIndex - mHealVals.Count + 1;

                    if (!(match = mGroup.GetHealValue(mSearchBuff[index0 + i]) == mHealVals[i]))
                    {
                        break;
                    }
                }
                if (!match)
                {
                    mSearchBuff.Add(mSearchRng.genrand());
                    mIndex++;
                }
            }
            timer.Stop();

            mGroup.SetIndex(indexStatic);
            return true;
        }

        /// <summary>
        /// Initialize rng object based on selected platform
        /// </summary>
        /// <returns></returns>
        private IRNG InitializeRNG()
        {
            if (SelectedPlatform == PlatformsTypes.PS2)
                return new RNG1998();
            else
                return new RNG2002();
        }

        /// <summary>
        /// This method pre-rolls the RNG to the correct point
        /// so we can start matching on our cure list
        /// </summary>
        /// <param name="start">index of first cure</param>
        /// <param name="end">future RNGs to simulate</param>
        private void DisplayRNG(int start, int end, bool firstLoad = false)
        {
            mDispRng = InitializeRNG();
            //Consume RNG seeds before our desired index
            //This can take obscene amounts of time.
            for (int i = 0; i < start; i++)
            {
                mDispRng.genrand();
            }

            displayRNGHelper(start, end - start, firstLoad);
        }

        /// <summary>
        /// The purpose of this method is to display to chest information for the future
        /// based on our current location in the RNG
        /// </summary>
        /// <param name="displayRNG">RNG numbers to use</param>
        /// <param name="start">index where our first matching heal is</param>
        /// <param name="rowsToRender">How many rows to display</param>
        private void displayRNGHelper(int start, int rowsToRender, bool firstLoad)
        {
            //Clear datagridview
            mRngGridData.Clear();

            uint firstRNGVal = mDispRng.genrand();
            uint secondRNGVal = mDispRng.genrand();
            uint thirdRNGVal = mDispRng.genrand();
            // We want to preserve the character index, since this loop is just for display:
            int indexStatic = mGroup.GetIndex();
            mGroup.ResetIndex();

            int end = start + rowsToRender;
            for (int index = start; index < end; index++)
            {
                // Index starting at 0
                int loopIndex = index - start;

                // Get the heal value once:
                int currentHeal = mGroup.GetHealValue(firstRNGVal);
                int nextHeal = mGroup.PeekHealValue(secondRNGVal);

                // Put the next expected heal in the text box
                if (index == start + mHealVals.Count - 1)
                {
                    LastHeal = nextHeal;
                }

                // Advance the RNG before starting the loop in case we want to skip an entry
                uint firstRNGVal_temp = firstRNGVal;
                uint secondRNGVal_temp = secondRNGVal;
                uint thirdRNGVal_temp = thirdRNGVal;

                firstRNGVal = secondRNGVal;
                secondRNGVal = thirdRNGVal;
                thirdRNGVal = mDispRng.genrand();

                // Skip the entry if it's too long ago
                if (loopIndex < mHealVals.Count - HISTORY_TO_DISPLAY)
                    continue;

                RngDataViewModel rngData = new RngDataViewModel
                {
                    Position = index,
                    Value = currentHeal,
                    Percent = firstRNGVal_temp % 100,
                    Steal = Steal.CheckSteal(firstRNGVal_temp, secondRNGVal_temp, thirdRNGVal_temp),
                    StealCuffs = Steal.CheckStealCuffs(firstRNGVal_temp, secondRNGVal_temp, thirdRNGVal_temp),
                    OneIn256 = firstRNGVal_temp < 0x1000000
                };
                //Start actually displaying
                RngGridData.Add(rngData);
            }
            mGroup.SetIndex(indexStatic);
            if (RngGridData.Count > 0)
            {
                if (firstLoad)
                    SelectedGridRow = RngGridData.Where(x => x.Position == start).FirstOrDefault();
            }
        }
        #endregion

        #region Commands
        public ICommand ContinueCommand { get; set; }
        public ICommand BeginSearchCommand { get; set; }
        #endregion
    }
}

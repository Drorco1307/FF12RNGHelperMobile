using System;

namespace FF12RngHelper.RNGHelper
{
    public interface IRNG : IDeepCloneable<IRNG>
    {
        void sgenrand();
        void sgenrand(UInt32 s);
        UInt32 genrand();

        RNGState saveState();
        void loadState(int inmti, UInt32[] inmt, int position);
        void loadState(RNGState state);
        int getPosition();
    }
}

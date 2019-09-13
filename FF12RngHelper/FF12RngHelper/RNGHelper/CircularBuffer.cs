namespace FF12RngHelper.RNGHelper
{
    public class CircularBuffer<T>: IDeepCloneable<CircularBuffer<T>>
    {
        private T[] mBuffer;
        private int mNextFree;
        private int mLength;

        public CircularBuffer(int length)
        {
            mBuffer = new T[length];
            mNextFree = 0;
        }

        public void Add(T o)
        {
            mBuffer[mNextFree] = o;
            mNextFree = (mNextFree + 1) % mBuffer.Length;
        }

        public CircularBuffer<T> DeepClone()
        {
            CircularBuffer<T> copy = new CircularBuffer<T>(mLength)
            {
                mBuffer = mBuffer.Clone() as T[],
                mNextFree = mNextFree,
                mLength = mLength
            };
            return copy;
        }

        object IDeepCloneable.DeepClone()
        {
            return DeepClone();
        }

        public T this[long index]
        {
            get
            {
                int index1 = (int)(index % mBuffer.Length);
                if (index1 < 0)
                    index1 = mBuffer.Length + index1;
                return mBuffer[index1];
            }
            set
            {
                mBuffer[index % mBuffer.Length] = value;
            }
        }

        public T this[ulong index]
        {
            get
            {
                return mBuffer[index % (ulong)mBuffer.Length];
            }
            set
            {
                mBuffer[index % (ulong)mBuffer.Length] = value;
            }
        }
    }
}

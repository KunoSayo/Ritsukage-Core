using System.Runtime.CompilerServices;

namespace RUCore.Common.Utils
{
    /// <summary>
    /// Lock for uint
    /// </summary>
    public struct UInt32Lock
    {
        private uint _lock;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint CompareExchange(ref uint location1, uint value, uint comparand)
        {
            return Interlocked.CompareExchange(ref location1, value, comparand);
        }

        /// <summary>
        /// Enter write lock
        /// </summary>
        public void EnterWriteLock()
        {
            uint lastLock;
            do
            {
                while ((lastLock = _lock) != 0) ;
            } while (CompareExchange(ref _lock, 0x80000000, lastLock) != lastLock);
        }

        /// <summary>
        /// Exit write lock
        /// </summary>
        public void ExitWriteLock()
        {
            Volatile.Write(ref _lock, 0u);
        }

        /// <summary>
        /// Enter read lock
        /// </summary>
        public void EnterReadLock()
        {
            uint lastLock, currentLock;
            do
            {
                while (((lastLock = _lock) >> 31) != 0) ;
                currentLock = lastLock + 1;
            } while (CompareExchange(ref _lock, currentLock, lastLock) != lastLock);
        }

        /// <summary>
        /// Exit read lock
        /// </summary>
        public void ExitReadLock()
        {
            uint lastLock;
            do
            {
                lastLock = _lock;
            } while (CompareExchange(ref _lock, lastLock - 1, lastLock) != lastLock);
        }
    }
}

using RUCore.Common.Handlers;
using RUCore.Common.Utils;
using System.Runtime.CompilerServices;

namespace RUCore.Common.Invoking
{
    public partial class MessageSubscription
    {
        /// <summary>
        /// Represents a node in a doubly-linked list representing the registered handlers.
        /// </summary>
        protected internal sealed class Registrations
        {
            /// <summary>
            /// Source of the registrations.
            /// </summary>
            public readonly MessageSubscription Source;

            /// <summary>
            /// Node representing the list of registrations.
            /// </summary>
            public RegistrationNode? EffictiveNodeList;

            /// <summary>
            /// Node representing the list of free registrations.
            /// </summary>
            public RegistrationNode? FreeNodeList;

            /// <summary>
            /// A monotonically increasing value that is used to generate unique ids for registrations.
            /// </summary>
            public long NextAvailableId = 1;

            /// <summary>
            /// Lock used to protect access to the registrations.
            /// </summary>
            readonly UInt32Lock _lock;

            /// <summary>
            /// Returns the number of registrations.
            /// </summary>
            /// <param name="source"></param>
            public Registrations(MessageSubscription source) => Source = source;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void Recycle(RegistrationNode node)
            {
                node.Id = 0;
                node.Handler = null;
                node.Prev = null;
                node.Next = FreeNodeList;
                FreeNodeList = node;
            }

            /// <summary>
            /// Returns the number of registrations.
            /// </summary>
            /// <param name="handler"></param>
            /// <returns></returns>
            public RegistrationNode Register(IMessageHandler handler)
            {
                RegistrationNode? node = null;
                if (FreeNodeList != null)
                {
                    EnterLock();
                    try
                    {
                        node = FreeNodeList;
                        if (node != null)
                        {
                            FreeNodeList = node.Next;
                            node.Id = NextAvailableId++;
                            node.Handler = handler;
                            node.Next = EffictiveNodeList;
                            EffictiveNodeList = node;
                            if (node.Next != null)
                                node.Next.Prev = node;
                        }
                    }
                    finally
                    {
                        ExitLock();
                    }
                }
                if (node == null)
                {
                    node = new RegistrationNode(this)
                    {
                        Handler = handler
                    };
                    EnterLock();
                    try
                    {
                        node.Id = NextAvailableId++;
                        node.Next = EffictiveNodeList;
                        if (node.Next != null)
                            node.Next.Prev = node;
                        EffictiveNodeList = node;
                    }
                    finally
                    {
                        ExitLock();
                    }
                }
                return node;
            }

            /// <summary>
            /// Unregisters the registration represented by the specified node.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="node"></param>
            /// <returns></returns>
            public bool Unregister(long id, RegistrationNode node)
            {
                if (id == 0)
                    return false;
                EnterLock();
                try
                {
                    if (node.Id != id)
                        return false;
                    if (EffictiveNodeList == node)
                        EffictiveNodeList = node.Next;
                    else
                        node.Prev!.Next = node.Next;
                    if (node.Next != null)
                        node.Next.Prev = node.Prev;
                    Recycle(node);
                    return true;
                }
                finally
                {
                    ExitLock();
                }
            }

            /// <summary>
            /// Unregisters all registrations.
            /// </summary>
            public void UnregisterAll()
            {
                EnterLock();
                try
                {
                    RegistrationNode? node = EffictiveNodeList;
                    EffictiveNodeList = null;
                    while (node != null)
                    {
                        RegistrationNode? next = node.Next;
                        Recycle(node);
                        node = next;
                    }
                }
                finally
                {
                    ExitLock();
                }
            }

            /// <summary>
            /// Enters the lock protecting access to the registrations.
            /// </summary>
            public void EnterLock()
                => _lock.EnterWriteLock();

            /// <summary>
            /// Exits the lock protecting access to the registrations.
            /// </summary>
            public void ExitLock()
                => _lock.ExitWriteLock();
        }
    }
}

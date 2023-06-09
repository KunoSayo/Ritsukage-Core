using RUCore.Common.Handlers;
using System.Collections;

namespace RUCore.Common.Invoking
{
    public partial class MessageSubscription
    {
        /// <summary>
        /// Enumerates the handlers of the subscription.
        /// </summary>
        public struct Enumerator : IEnumerator<IMessageHandler>
        {
            private readonly MessageSubscription _subscription;
            private IMessageHandler? _current;
            private int _staticIdx;
            private RegistrationNode? _dynamicNode;

            /// <summary>
            /// Enumerates the handlers of the subscription.
            /// </summary>
            /// <param name="subscription"></param>
            public Enumerator(MessageSubscription subscription)
            {
                _subscription = subscription;
                _current      = null;
                _staticIdx    = 0;
                _dynamicNode  = subscription._registrations?.EffictiveNodeList;
            }

            /// <summary>
            /// Returns the current handler.
            /// </summary>
            public readonly IMessageHandler Current => _current!;

            /// <summary>
            /// Returns the current handler.
            /// </summary>
            readonly object IEnumerator.Current => _current!;

            /// <summary>
            /// Moves to the next handler.
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                if (_staticIdx < _subscription.StaticHandlers.Length)
                {
                    _current = _subscription.StaticHandlers[_staticIdx++];
                    return true;
                }

                if (_dynamicNode != null)
                {
                    _current     = _dynamicNode.Handler;
                    _dynamicNode = _dynamicNode.Next;
                    return _current != null;
                }

                return false;
            }

            /// <summary>
            /// Resets the enumerator to its initial position.
            /// </summary>
            public void Reset()
            {
                _current     = null;
                _staticIdx   = 0;
                _dynamicNode = _subscription._registrations?.EffictiveNodeList;
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public readonly void Dispose()
            {
            }
        }
    }
}

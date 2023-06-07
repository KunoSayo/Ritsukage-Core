using RUCore.Common.Handlers;

namespace RUCore.Common.Invoking
{
    public partial class MessageSubscription
    {
        /// <summary>
        /// Node in the linked list of registrations.
        /// </summary>
        protected internal sealed class RegistrationNode
        {
            /// <summary>
            /// Registrations that owns this node.
            /// </summary>
            public readonly Registrations Registrations;

            /// <summary>
            /// Previous nodes in the linked list.
            /// </summary>
            public RegistrationNode? Prev;

            /// <summary>
            /// Next nodes in the linked list.
            /// </summary>
            public RegistrationNode? Next;

            /// <summary>
            /// Id of this node.
            /// </summary>
            public long Id;

            /// <summary>
            /// Handler of this node.
            /// </summary>
            public IMessageHandler? Handler;

            /// <summary>
            /// RegistrationNode constructor.
            /// </summary>
            /// <param name="registrations"></param>
            public RegistrationNode(Registrations registrations)
            {
                Registrations = registrations;
            }
        }
    }
}

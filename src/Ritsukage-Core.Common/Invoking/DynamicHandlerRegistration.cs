namespace RUCore.Common.Invoking
{
    /// <summary>
    /// Describes a registration of a dynamic message handler.
    /// </summary>
    public struct DynamicHandlerRegistration : IEquatable<DynamicHandlerRegistration>, IDisposable
    {
        private readonly long _id;
        private readonly MessageSubscription.RegistrationNode _node;

        internal DynamicHandlerRegistration(long id, MessageSubscription.RegistrationNode node)
        {
            _id   = id;
            _node = node;
        }

        /// <summary>
        /// Disposes of the registration.
        /// </summary>
        public readonly void Dispose()
        {
            if (_node is MessageSubscription.RegistrationNode node)
                _node.Registrations.Unregister(_id, node);
        }

        /// <summary>
        /// Returns true if the specified registrations are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(DynamicHandlerRegistration left, DynamicHandlerRegistration right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Returns true if the specified registrations are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(DynamicHandlerRegistration left, DynamicHandlerRegistration right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns true if the specified registration is equal to this one.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is DynamicHandlerRegistration other && Equals(other);
        }

        /// <summary>
        /// Returns true if the specified registration is equal to this one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DynamicHandlerRegistration other)
        {
            return _node == other._node && _id == other._id;
        }

        /// <summary>
        /// Gets the hash code for this registration.
        /// </summary>
        /// <returns></returns>
        public override readonly int GetHashCode()
        {
            return _node != null ? _node.GetHashCode() ^ _id.GetHashCode() : _id.GetHashCode();
        }
    }
}

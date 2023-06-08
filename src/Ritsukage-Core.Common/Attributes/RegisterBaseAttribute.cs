using Microsoft.Extensions.DependencyInjection;

namespace RUCore.Common.Attributes
{
    /// <summary>
    /// Register service attribute
    /// </summary>
    public abstract class RegisterBaseAttribute : Attribute
    {
        /// <summary>
        /// Service type
        /// </summary>
        public Type ServiceType { get; }

        /// <summary>
        /// Implementation type
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// Service lifetime
        /// </summary>
        public ServiceLifetime? Lifetime { get; }

        /// <summary>
        /// Register service
        /// </summary>
        /// <param name="implementationType"></param>
        protected RegisterBaseAttribute(Type implementationType) : this(null, implementationType)
        {
        }

        /// <summary>
        /// Register service
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        protected RegisterBaseAttribute(Type implementationType, ServiceLifetime? lifetime) : this(
            null, implementationType, lifetime)
        {
        }

        /// <summary>
        /// Register service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        protected RegisterBaseAttribute(Type? serviceType, Type implementationType)
        {
            ServiceType        = serviceType ?? GetServiceType(implementationType);
            ImplementationType = implementationType;
        }

        /// <summary>
        /// Register service
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        protected RegisterBaseAttribute(Type? serviceType, Type implementationType, ServiceLifetime? lifetime) : this(
            serviceType, implementationType)
        {
            Lifetime = lifetime;
        }

        /// <summary>
        /// Get service type
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        protected virtual Type GetServiceType(Type implementationType)
        {
            throw new NotSupportedException();
        }
    }
}

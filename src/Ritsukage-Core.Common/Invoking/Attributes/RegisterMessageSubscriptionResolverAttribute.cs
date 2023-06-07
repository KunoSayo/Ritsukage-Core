using Microsoft.Extensions.DependencyInjection;
using RUCore.Common.Attributes;

namespace RUCore.Common.Invoking.Attributes
{
    /// <summary>
    /// Register a message subscription resolver.
    /// </summary>
    public class RegisterMessageSubscriptionResolverAttribute : RegisterBaseAttribute
    {
        /// <summary>
        /// Register a message subscription resolver.
        /// </summary>
        /// <param name="implementationType"></param>
        public RegisterMessageSubscriptionResolverAttribute(Type implementationType) : this(implementationType, null)
        { }

        /// <summary>
        /// Register a message subscription resolver.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        public RegisterMessageSubscriptionResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        { }

        /// <summary>
        /// Get the service type from the implementation type.
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected override Type GetServiceType(Type implementationType)
        {
            Type openGeneric = typeof(IMessageSubscriptionResolver<,>);
            foreach (Type interfaceType in implementationType.GetInterfaces())
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                    return interfaceType;
            throw new ArgumentException($"Given {implementationType.Name} does not implement {openGeneric.Name}", nameof(implementationType));
        }
    }
}

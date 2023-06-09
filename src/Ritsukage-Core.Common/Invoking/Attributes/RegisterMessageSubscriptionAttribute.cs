using Microsoft.Extensions.DependencyInjection;
using RUCore.Common.Attributes;

namespace RUCore.Common.Invoking.Attributes
{
    /// <summary>
    /// Register a message subscription
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RegisterMessageSubscriptionAttribute : RegisterBaseAttribute
    {
        /// <summary>
        /// Register a message subscription
        /// </summary>
        /// <param name="implementationType"></param>
        public RegisterMessageSubscriptionAttribute(Type implementationType) : this(null, implementationType)
        {
        }

        /// <summary>
        /// Register a message subscription
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        public RegisterMessageSubscriptionAttribute(Type implementationType, ServiceLifetime? lifetime) : this(
            null, implementationType, lifetime)
        {
        }

        /// <summary>
        /// Register a message subscription
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        public RegisterMessageSubscriptionAttribute(Type? serviceType, Type implementationType) : this(
            serviceType, implementationType, null)
        {
        }

        /// <summary>
        /// Register a message subscription
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        public RegisterMessageSubscriptionAttribute(Type? serviceType, Type implementationType,
                                                    ServiceLifetime? lifetime) : base(
            serviceType, implementationType, lifetime)
        {
        }

        /// <summary>
        /// Get service type
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        protected override Type GetServiceType(Type implementationType)
        {
            return typeof(IMessageSubscription);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using RUCore.Common.Attributes;

namespace RUCore.Common.Parsers.Attributes
{
    /// <summary>
    /// Targeting <see cref="IMessageParser{TRawData, TMessage}"/> implementations
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class RegisterParserAttribute : RegisterBaseAttribute
    {
        /// <summary>
        /// Register a <see cref="IMessageParser{TRawData, TMessage}"/> for the given <paramref name="implementationType"/>
        /// </summary>
        /// <param name="implementationType"></param>
        public RegisterParserAttribute(Type implementationType) : this(implementationType, null)
        {
        }

        /// <summary>
        /// Register a <see cref="IMessageParser{TRawData, TMessage}"/> for the given <paramref name="implementationType"/> with the given <paramref name="lifetime"/>
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="lifetime"></param>
        public RegisterParserAttribute(Type implementationType, ServiceLifetime? lifetime) : base(
            implementationType, lifetime)
        {
        }

        /// <summary>
        /// Gets the service type of the given <paramref name="implementationType"/>
        /// </summary>
        /// <param name="implementationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected override Type GetServiceType(Type implementationType)
        {
            Type openGeneric = typeof(IMessageParser<,>);
            foreach (Type interfaceType in implementationType.GetInterfaces())
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                    return interfaceType;
            throw new ArgumentException(
                $"The given {implementationType.FullName} does not implement {openGeneric.FullName}",
                nameof(implementationType));
        }
    }
}

namespace RUCore.Common.Invoking
{
    /// <summary>
    /// PluginResistration
    /// </summary>
    public struct PluginResistration : IDisposable
    {
        internal LinkedList<DynamicHandlerRegistration>? _registrations;

        /// <summary>
        /// PluginResistration
        /// </summary>
        /// <param name="registrations"></param>
        public PluginResistration(LinkedList<DynamicHandlerRegistration> registrations)
        {
            _registrations = registrations;
        }

        /// <summary>
        /// Dispose all registrations.
        /// </summary>
        public readonly void Dispose()
        {
            if (_registrations is LinkedList<DynamicHandlerRegistration> registrations)
                foreach (var registration in registrations)
                    registration.Dispose();
        }
    }
}

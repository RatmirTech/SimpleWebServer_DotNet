namespace SimpleWebServer.Core.IoC
{
    public class IoCContainer
    {
        private enum Lifetime
        {
            Singleton,
            Scoped,
            Transient
        }

        private class Registration
        {
            public Type ConcreteType { get; }
            public Lifetime Lifetime { get; }

            // for Singleton & Scoped
            public object Instance { get; set; }

            public Registration(Type concreteType, Lifetime lifetime)
            {
                ConcreteType = concreteType;
                Lifetime = lifetime;
            }
        }

        private static readonly Dictionary<Type, Registration> _registrations = new();

        [ThreadStatic]
        private static Dictionary<Type, object> _scopedInstances = new();

        public static void RegisterSingleton<Tkey, TConcrete>()
        {
            _registrations[typeof(Tkey)] = new Registration(typeof(TConcrete), Lifetime.Singleton);
        }

        public static void RegisterScoped<Tkey, TConcrete>()
        {
            _registrations[typeof(Tkey)] = new Registration(typeof(TConcrete), Lifetime.Scoped);
        }

        public static void RegisterTransient<Tkey, TConcrete>()
        {
            _registrations[typeof(Tkey)] = new Registration(typeof(TConcrete), Lifetime.Transient);
        }

        public static Tkey Resolve<Tkey>()
        {
            // only for customers, don't using it in system
            return (Tkey)Resolve(typeof(Tkey));
        }

        private static dynamic Resolve(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("Resolve: type is null.");

            if (!_registrations.ContainsKey(type))
                throw new InvalidOperationException($"Resolve: type {type.Name} not registered.");

            var registration = _registrations[type];

            switch (registration.Lifetime)
            {
                case Lifetime.Singleton:
                    if (registration.Instance == null)
                    {
                        if (registration.ConcreteType != null)
                        {
                            registration.Instance = Activator.CreateInstance(registration.ConcreteType);
                        }
                    }
                    return registration.Instance;

                case Lifetime.Scoped:
                    if (_scopedInstances == null)
                    {
                        _scopedInstances = new();
                    }

                    if (!_scopedInstances.ContainsKey(type))
                    {
                        if (registration.ConcreteType != null)
                        {
                            _scopedInstances[type] = Activator.CreateInstance(registration.ConcreteType);
                        }
                    }

                    return _scopedInstances[type];

                case Lifetime.Transient:
                    return Activator.CreateInstance(registration.ConcreteType);

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

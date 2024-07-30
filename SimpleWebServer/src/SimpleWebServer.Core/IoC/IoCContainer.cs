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
        private static Dictionary<Type, object> _scopedInstances;

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
            //TODO: Сделать рабочий Resolve
            return null;
        }
    }
}

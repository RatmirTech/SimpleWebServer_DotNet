namespace SimpleWebServer.Core.IoC
{
    public class IoCContainer
    {
        private static readonly Dictionary<Type, Type> _registrations = new();

        private static void Register<Tkey, TConcrete>()
        {
            _registrations[typeof(TConcrete)] = typeof(TConcrete);
        }

        private static dynamic Resolve<Tkey>()
        {
            return Activator.CreateInstance(_registrations[typeof(Tkey)]);
        }
    }
}

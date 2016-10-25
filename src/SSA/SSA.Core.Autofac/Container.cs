using System;
using Autofac;

namespace SSA.Core.Autofac
{
    public static class Container
    {
        private static IContainer _kernel;

        public static void Initialize(IContainer kernel)
        {
            _kernel = kernel;
        }

        public static T Get<T>()
        {
            return _kernel.Resolve<T>();
        }

        public static object Get(Type type)
        {
            return _kernel.Resolve(type);
        }
    }
}
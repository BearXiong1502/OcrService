using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Configuration;
using Autofac.Core;

namespace OcrServices
{
    public class Containers
    {
        private static IContainer _container;
        public static T Resolve<T>(string serviceName)
        {
            if (_container == null) InitializeComponent();
            return _container.ResolveNamed<T>(serviceName);
        }
        public static T Resolve<T>(string serviceName,params Parameter[] parameters)
        {
            if (_container == null) InitializeComponent();
            return _container.ResolveNamed<T>(serviceName, parameters);
        }

        private static void InitializeComponent()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            _container = builder.Build();
        }
    }
}

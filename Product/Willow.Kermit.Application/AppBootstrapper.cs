﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Caliburn.Micro;
using Willow.Kermit.Shell.Interfaces;
using System.Windows;

namespace Willow.Kermit.Application
{
    public class AppBootstrapper : Bootstrapper<KermitShell>
	{
		CompositionContainer _container;

		/// <summary>
		/// By default, we are configured to use MEF
		/// </summary>
		protected override void Configure() {
            //Retrieve all the assemblies
            var pluginAssemblies = new[] { typeof(KermitShell).Assembly };
            
            //Add the assemblies to Caliburn's source & our own catalogue
            AssemblySource.Instance.AddRange(pluginAssemblies);
            var catalog = new AggregateCatalog(pluginAssemblies.Select(x =>  new AssemblyCatalog(x)).ToArray());

            //Add manually created objects
            var batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());

            //Compose the container with the catalog and add manually created exports
            _container = new CompositionContainer(catalog);
            _container.Compose(batch);

            AddActionMessageShortcuts();
		}

        private void AddActionMessageShortcuts()
        {
            MessageBinder.SpecialValues.Add("$originalsource", context =>
            {
                var args = context.EventArgs as RoutedEventArgs;
                return args == null ? null :  args.OriginalSource;
            });
        }

		protected override object GetInstance(Type serviceType, string key)
		{
			string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
			var exports = _container.GetExportedValues<object>(contract);

		    var result = exports.FirstOrDefault();
            if (result != null) return result;

			throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
		}

		protected override IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
		}

		protected override void BuildUp(object instance)
		{
			_container.SatisfyImportsOnce(instance);
		}
	}
}

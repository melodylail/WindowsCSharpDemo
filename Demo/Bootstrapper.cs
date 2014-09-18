// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

namespace Demo
{
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Windows;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.MefExtensions;
    using Microsoft.Practices.Prism.Modularity;
    using Demo.Views;
    using Demo.Infrastructure;

    /// <summary>
    /// Initializes Prism to start this quickstart Prism application to use Managed Extensibility Framework (MEF).
    /// </summary>
    public class Bootstrapper : MefBootstrapper
    {
        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>The shell of the application.</returns>
        /// <remarks>
        /// If the returned instance is a <see cref="DependencyObject"/>, the
        /// <see cref="MefBootstrapper"/> will attach the default <seealso cref="Microsoft.Practices.Composite.Regions.IRegionManager"/> of
        /// the application in its <see cref="Microsoft.Practices.Composite.Presentation.Regions.RegionManager.RegionManagerProperty"/> attached property
        /// in order to be able to add regions by using the <seealso cref=""Microsoft.Practices.Composite.Presentation.Regions.RegionManager.RegionNameProperty"/>
        /// attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<Shell>();
        }
        protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();

            factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

            return factory;
        }

        /// <summary>
        /// Initializes the shell.
        /// </summary>
        /// <remarks>
        /// The base implemention ensures the shell is composed in the container.
        /// </remarks>
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Shell)this.Shell;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Configures the <see cref="AggregateCatalog"/> used by MEF.
        /// </summary>
        /// <remarks>
        /// The base implementation does nothing.
        /// </remarks>
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            // Add this assembly to export ModuleTracker
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
        }

        /// <summary>
        /// Configures the <see cref="CompositionContainer"/>.
        /// May be overwritten in a derived class to add specific type mappings required by the application.
        /// </summary>
        /// <remarks>
        /// The base implementation registers all the types direct instantiated by the bootstrapper with the container.
        /// The base implementation also sets the ServiceLocator provider singleton.
        /// </remarks>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

        }

        /// <summary>
        /// Creates the <see cref="IModuleCatalog"/> used by Prism.
        /// </summary>
        /// <remarks>
        /// The base implementation returns a new ModuleCatalog.
        /// </remarks>
        /// <returns>
        /// A ConfigurationModuleCatalog.
        /// </returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            // When using MEF, the existing Prism ModuleCatalog is still the place to configure modules via configuration files.
            return new ConfigurationModuleCatalog();
        }
    }
}

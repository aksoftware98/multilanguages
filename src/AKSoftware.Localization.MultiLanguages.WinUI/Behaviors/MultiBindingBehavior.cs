using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Markup;
using Microsoft.Xaml.Interactivity;

namespace AKSoftware.Localization.MultiLanguages.WinUI.Behaviors
{
    [ContentProperty(Name = "Items")]
    [TypeConstraint(typeof(FrameworkElement))]
    public class MultiBindingBehavior : Behavior<FrameworkElement>
    {
        private sealed class BehaviorExtension : IExtension
        {
            public object Component { get; set; }
            public Action<object> Action { get; set; }
        }

        private IExtension _languageExtension;

        public MultiBindingItemCollection Items
        {
            get => (MultiBindingItemCollection)GetValue(ItemsProperty);
            private set => SetValue(ItemsProperty, value);
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(MultiBindingItemCollection),
                typeof(MultiBindingBehavior), null);

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(MultiBindingBehavior),
                new PropertyMetadata(null, OnPropertyChanged));

        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public static readonly DependencyProperty ConverterProperty =
            DependencyProperty.Register(nameof(Converter), typeof(IValueConverter), typeof(MultiBindingBehavior),
                new PropertyMetadata(null, OnPropertyChanged));

        public object ConverterParameter
        {
            get => GetValue(ConverterParameterProperty);
            set => SetValue(ConverterParameterProperty, value);
        }

        public static readonly DependencyProperty ConverterParameterProperty =
            DependencyProperty.Register(nameof(ConverterParameter), typeof(object), typeof(MultiBindingBehavior),
                new PropertyMetadata(null, OnPropertyChanged));

        public BindingMode Mode
        {
            get => (BindingMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(BindingMode), typeof(MultiBindingBehavior),
                new PropertyMetadata(BindingMode.OneWay, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var multiBindingBehavior = (MultiBindingBehavior)d;

            multiBindingBehavior.Update();
        }

        public MultiBindingBehavior()
        {
            Items = new MultiBindingItemCollection();
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            RegisterLanguageExtension();
            Update();
        }

        protected override void OnDetaching()
        {
            if (_languageExtension != null)
            {
                _languageExtension.Component = null;
                _languageExtension.Action = null;
                _languageExtension = null;
            }

            base.OnDetaching();
        }

        private void RegisterLanguageExtension()
        {
            if (_languageExtension != null)
            {
                return;
            }

            var localization = (Application.Current as IServiceProviderHost)?.ServiceProvider.GetService<ILanguageContainerService>();

            if (localization == null)
            {
                return;
            }

            _languageExtension = new BehaviorExtension
            {
                Component = this,
                Action = _ =>
                {
                    var dispatcherQueue = AssociatedObject?.DispatcherQueue;

                    if (dispatcherQueue == null)
                    {
                        return;
                    }

                    if (dispatcherQueue.HasThreadAccess)
                    {
                        Items?.Update();
                    }
                    else
                    {
                        dispatcherQueue.TryEnqueue(() => Items?.Update());
                    }
                }
            };

            localization.AddExtension(_languageExtension);
        }

        private void Update()
        {
            if (AssociatedObject == null || string.IsNullOrEmpty(PropertyName))
            {
                return;
            }

            var targetProperty = PropertyName;
            Type targetType;

            if (targetProperty.Contains("."))
            {
                var propertyNameParts = targetProperty.Split('.');

                targetType = Type.GetType($"Microsoft.UI.Xaml.Controls.{propertyNameParts[0]}, Microsoft.WinUI");

                targetProperty = propertyNameParts[1];
            }
            else
            {
                targetType = AssociatedObject.GetType();
            }

            PropertyInfo targetDependencyPropertyField = null;

            while (targetDependencyPropertyField == null && targetType != null)
            {
                var targetTypeInfo = targetType.GetTypeInfo();

                targetDependencyPropertyField = targetTypeInfo.GetDeclaredProperty(targetProperty + "Property");

                targetType = targetTypeInfo.BaseType;
            }

            if (targetDependencyPropertyField == null)
            {
                return;
            }

            var targetDependencyProperty = (DependencyProperty)targetDependencyPropertyField.GetValue(null);

            var binding = new Binding
            {
                Path = new PropertyPath("Value"),
                Source = Items,
                Converter = Converter,
                ConverterParameter = ConverterParameter,
                Mode = Mode
            };

            BindingOperations.SetBinding(AssociatedObject, targetDependencyProperty, binding);
        }
    }
}

using Microsoft.UI.Xaml;

namespace AKSoftware.Localization.MultiLanguages.WinUI.Behaviors
{
    public class MultiBindingItem : DependencyObject
    {
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(MultiBindingItem), new PropertyMetadata(null, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var multiBindingItem = (MultiBindingItem)d;
            multiBindingItem.Update();
        }

        internal MultiBindingItemCollection Parent { get; set; }

        private void Update()
        {
            var parent = Parent;

            if (parent != null)
            {
                parent.Update();
            }
        }
    }
}

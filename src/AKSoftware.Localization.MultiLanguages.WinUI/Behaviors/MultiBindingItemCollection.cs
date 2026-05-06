using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.UI.Xaml;

namespace AKSoftware.Localization.MultiLanguages.WinUI.Behaviors
{
    public class MultiBindingItemCollection : DependencyObjectCollection<MultiBindingItem>, INotifyPropertyChanged
    {
        private bool updating_;
        private object[] value_;

        public event PropertyChangedEventHandler PropertyChanged;

        public object[] Value
        {
            get => value_;
            set
            {
                if (!ReferenceEquals(value_, value))
                {
                    value_ = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                    OnValueChanged();
                }
            }
        }

        private void OnValueChanged()
        {
            UpdateSource();
        }

        public MultiBindingItemCollection()
        {
            CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (MultiBindingItem item in e.OldItems)
                {
                    item.Parent = null;
                }
            }

            if (e.NewItems != null)
            {
                foreach (MultiBindingItem item in e.NewItems)
                {
                    item.Parent = this;
                }
            }

            Update();
        }

        internal void Update()
        {
            if (updating_)
            {
                return;
            }

            try
            {
                updating_ = true;

                Value = this
                    .OfType<MultiBindingItem>()
                    .Select(x => x.Value)
                    .ToArray();
            }
            finally
            {
                updating_ = false;
            }
        }

        private void UpdateSource()
        {
            if (updating_)
            {
                return;
            }

            try
            {
                updating_ = true;

                var values = Value;

                for (var index = 0; index < this.Count; index++)
                {
                    if (this[index] is MultiBindingItem multiBindingItem)
                    {
                        var nextValue = values != null && index < values.Length
                            ? values[index]
                            : null;

                        if (!Equals(multiBindingItem.Value, nextValue))
                        {
                            multiBindingItem.Value = nextValue;
                        }
                    }
                }
            }
            finally
            {
                updating_ = false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;

namespace AKSoftware.Localization.MultiLanguages.WinUI.Behaviors
{
    public class DependencyObjectCollection<T> : DependencyObjectCollection, INotifyCollectionChanged
        where T : DependencyObject
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly List<T> _oldItems = new List<T>();

        public DependencyObjectCollection()
        {
            VectorChanged += DependencyObjectCollectionVectorChanged;
        }

        private void DependencyObjectCollectionVectorChanged(IObservableVector<DependencyObject> sender, IVectorChangedEventArgs e)
        {
            var index = (int)e.Index;

            switch (e.CollectionChange)
            {
                case CollectionChange.Reset:
                    foreach (var item in this)
                    {
                        VerifyType(item);
                    }

                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                    _oldItems.Clear();

                    break;

                case CollectionChange.ItemInserted:
                    VerifyType(this[index]);

                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this[index], index));

                    _oldItems.Insert(index, (T)this[index]);

                    break;

                case CollectionChange.ItemRemoved:
                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, _oldItems[index], index));

                    _oldItems.RemoveAt(index);

                    break;

                case CollectionChange.ItemChanged:
                    VerifyType(this[index]);

                    RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, this[index], _oldItems[index]));

                    _oldItems[index] = (T)this[index];

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void RaiseCollectionChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            var eventHandler = CollectionChanged;

            if (eventHandler != null)
            {
                eventHandler(this, eventArgs);
            }
        }

        private void VerifyType(DependencyObject item)
        {
            if (!(item is T))
            {
                throw new InvalidOperationException("Invalid item type added to collection");
            }
        }
    }
}

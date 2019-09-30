using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Utility.MVVM
{
	/// <summary>
	/// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DebuggerStepThrough]
	public class ObservableRangeCollection<T> : ObservableCollection<T>
	{
		/// <summary>
		/// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class.
		/// </summary>
		public ObservableRangeCollection()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the System.Collections.ObjectModel.ObservableCollection(Of T) class that contains elements copied from the specified collection.
		/// </summary>
		/// <param name="collection">collection: The collection from which the elements are copied.</param>
		/// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception>
		public ObservableRangeCollection(IEnumerable<T> collection)
			: base(collection)
		{
		}

		public void InsertOItem(int index, T item)
		{
			base.InsertItem(index, item);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// protected override void ClearItems()
		// {
		//    foreach (var item in this)
		//    {
		//        item.PropertyChanged -= item_PropertyChanged;
		//    }

		//    base.ClearItems();
		// }

		// protected override void SetItem(int index, T item)
		// {
		//    var oldItem = this[index];
		//    oldItem.PropertyChanged -= item_PropertyChanged;
		//    base.SetItem(index, item);
		//    item.PropertyChanged += item_PropertyChanged;
		// }

		// private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		// {
		//    OnItemPropertyChanged((T)sender, e.PropertyName);
		// }

		// public void OnItemPropertyChanged(T item, string propertyName)
		// {
		//    var handler = this.ItemPropertyChanged;

		//    if (handler != null)
		//    {
		//        handler(this, new ItemPropertyChangedEventArgs<T>(item, propertyName));
		//    }
		// }

		/// <summary>
		/// Adds the elements of the specified collection to the end of the ObservableCollection(Of T).
		/// </summary>
		public void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			CheckReentrancy();

			if (notificationMode == NotifyCollectionChangedAction.Reset)
			{
				foreach (var i in collection)
					Items.Add(i);

				OnPropertyChanged(new PropertyChangedEventArgs("Count"));
				OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

				return;
			}

			int startIndex = Count;
			var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);
			foreach (var i in changedItems)
				Items.Add(i);

			OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
		}

		/// <summary>
		/// Removes the first occurrence of each item in the specified collection from ObservableCollection(Of T).
		/// </summary>
		public void RemoveRange(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			foreach (var i in collection)
				Items.Remove(i);
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Clears the current collection and replaces it with the specified item.
		/// </summary>
		public void Replace(T item) => ReplaceRange(new T[] { item });

		/// <summary>
		/// Clears the current collection and replaces it with the specified collection.
		/// </summary>
		public void ReplaceRange(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");

			Items.Clear();
			AddRange(collection, NotifyCollectionChangedAction.Reset);
		}
	}

	[DebuggerStepThrough]
	public sealed class ItemPropertyChangedEventArgs<T> : EventArgs
	{
		private readonly T _item;
		private readonly string _propertyName;

		public ItemPropertyChangedEventArgs(T item, string propertyName)
		{
			_item = item;
			_propertyName = propertyName;
		}

		public T Item => _item;

		public string PropertyName => _propertyName;
	}
}
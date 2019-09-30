using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utility.MVVM
{
	/// <summary>
	/// Observable object with INotifyPropertyChanged implemented
	/// </summary>
	public class ObservableObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Id for item
		/// </summary>
		public Guid GuId { get; set; }

		#region ContextMenuItems

		private ObservableCollection<BaseDataObject> _ContextMenuItems;

		public ObservableCollection<BaseDataObject> ContextMenuItems
		{
			get
			{
				if (_ContextMenuItems == null) _ContextMenuItems = new ObservableCollection<BaseDataObject>();
				return _ContextMenuItems;
			}
			set { SetProperty(ref _ContextMenuItems, value); }
		}

		#endregion ContextMenuItems

		#region Items

		private ObservableCollection<BaseDataObject> _Items;

		public ObservableCollection<BaseDataObject> Items
		{
			get
			{
				if (_Items == null) _Items = new ObservableCollection<BaseDataObject>();
				return _Items;
			}
			set { SetProperty(ref _Items, value); }
		}

		#endregion Items

		/// <summary>
		/// Sets the property.
		/// </summary>
		/// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
		/// <param name="backingStore">Backing store.</param>
		/// <param name="value">Value.</param>
		/// <param name="propertyName">Property name.</param>
		/// <param name="onChanged">On changed.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		protected bool SetProperty<T>(
			ref T backingStore, T value,
			[CallerMemberName]string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			ModifiedDate = DateTime.Now;

			OnPropertyChanged(propertyName);
			return true;
		}

		public DateTime ModifiedDate { get; set; } = DateTime.Now;

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
		{
			var changed = PropertyChanged;
			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void Notify([CallerMemberName]string propertyName = "")
		{
			OnPropertyChanged(propertyName);
		}
	}

	// Watches a task and raises property-changed notifications when the task completes.
}
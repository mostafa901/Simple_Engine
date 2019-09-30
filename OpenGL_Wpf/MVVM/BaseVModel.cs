

#if !Android && !Console

using System.Windows;

#endif

namespace Utility.MVVM
{
	public class BaseVModel : ObservableObject
	{
		/// <summary>
		/// Private backing field to hold the title
		/// </summary>
		private string title = string.Empty;

		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		private bool isBusy = false;

		public bool IsBusy
		{
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}

		#region Busy

#if !Android && !Console
		private Visibility _Busy = Visibility.Collapsed;

		public Visibility Busy
		{
			get
			{
				return _Busy;
			}
			set { SetProperty(ref _Busy, value); }
		}

#endif

		#endregion Busy

		#region Refresh_CMD

		private cus_CMD refresh_CMD;

		public cus_CMD CMD_Refresh
		{
			get
			{
				if (refresh_CMD == null) refresh_CMD = new cus_CMD();
				return refresh_CMD;
			}
			set { SetProperty(ref refresh_CMD, value); }
		}

		#endregion Refresh_CMD
	}
}
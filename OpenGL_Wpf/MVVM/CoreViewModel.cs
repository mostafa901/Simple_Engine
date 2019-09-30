#if !Android && !Console

using System.Windows.Controls;
using System.Windows.Threading;

#endif
#if Android
using Xamarin.Forms;
#endif

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;


namespace Utility.MVVM
{
	public class CoreViewModel : BaseVModel
	{
		public CancellationTokenSource token { get; set; } = new CancellationTokenSource();

		public CoreViewModel()
		{

		}

#if !Android && !Console
		public UserControl uc;

#endif
#if Android

		#region uc

        private ContentPage _uc;

        public ContentPage uc
        {
            get
            {
                return _uc;
            }
            set
            {
                SetProperty(ref _uc, value);

                uc.Disappearing += delegate
                 {
                     token?.Cancel();
                 };
            }
        }

		#endregion uc

#endif

		#region FreeToGo

		private bool _FreeToGo;

		public bool FreeToGo
		{
			get
			{
				return _FreeToGo;
			}
			set
			{
#if Android
                if (value) NavigationPage.SetHasBackButton(uc, true);
                else NavigationPage.SetHasBackButton(uc, false);

#endif
				SetProperty(ref _FreeToGo, value);
			}
		}

		#endregion FreeToGo

		//this is saved signal  for groupKeySearch

		#region GroupKey

		private string _GroupKey = "Category";

		public string GroupKey
		{
			get
			{
				return _GroupKey;
			}
			set { SetProperty(ref _GroupKey, value); }
		}

		#endregion GroupKey

		#region Header

		private string _Header;

		public string Header
		{
			get
			{
				return _Header;
			}
			set { SetProperty(ref _Header, value); }
		}

		async internal Task AddItems(IEnumerable<BaseDataObject> models, CancellationTokenSource tknsource)
		{
			IsBusy = true;
			if (models.Any() == false)
			{
				IsBusy = false;
			}
			var count = models.Count();
			foreach (var mod in models)
			{
				if (tknsource != null)
					if (tknsource.Token.IsCancellationRequested) return;

#if Android
                await AddItemAsync(mod);

#endif
#if Windows
				await uc.Dispatcher.BeginInvoke(new Action(async () => await AddItemAsync(mod, tknsource)), DispatcherPriority.Background);

#endif
				if (count > 0)
					if (models.ElementAt(count - 1) == mod)
					{
						IsBusy = false;
#if !Android && !Console
						Busy = System.Windows.Visibility.Collapsed;
#endif
					}
			}
		}

		#endregion Header

		#region ModelGroup

		//this collection is used to Group all the elements in the View Model
		public class ModelGroup : ObservableRangeCollection<BaseDataObject>
		{
			public string Title { get; set; }
			public string ShortName { get; set; } //will be used for jump lists
			public string Subtitle { get; set; }

			public ModelGroup(string title, string shortName)
			{
				Title = title;
				ShortName = shortName;
			}

			public cus_CMD CMD { get; set; } = new cus_CMD();
		}

		#endregion ModelGroup

		#region Groups

		private ObservableRangeCollection<ModelGroup> _Groups;

#if Android
        public CoreViewModel(ContentPage _uc)
#endif
#if Windows

		public CoreViewModel(UserControl _uc)

		{
			this.uc = _uc;
		}
#endif
		public ObservableRangeCollection<ModelGroup> Groups
		{
			get
			{
				if (_Groups == null) _Groups = new ObservableRangeCollection<ModelGroup>();
				return _Groups;
			}
			set { SetProperty(ref _Groups, value); }
		}




		#endregion Groups

		public async Task<bool> AddItemAsync(BaseDataObject item, CancellationTokenSource token = null)
		{
#if Windows || Android
#if Android
            Device.BeginInvokeOnMainThread((() =>
#endif
#if Windows

			await uc.Dispatcher.BeginInvoke(new Action(() =>
#endif
			{
				if (token != null)
					if (token.IsCancellationRequested) return;
			//if(!Items.Where(o=>o.GuId==item.GuId).Any())
					Items.Add(item);
			}));

#endif
			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(BaseDataObject item)
		{
			var index = Items.IndexOf(Items.Where((BaseDataObject arg) => arg.GuId == item.GuId).FirstOrDefault());

#if Windows || Android
#if Android
            Device.BeginInvokeOnMainThread(() =>{
             Groups.ForEach(o => o.Remove(o.Where(m => m.GuId == item.GuId).FirstOrDefault()));
#endif
#if Windows
			uc.Dispatcher.Invoke(() =>
			{
#endif

			Items.Remove(item);
			});
			await Task.Delay(200);

			Items.Insert(index, item);

#endif
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(Guid id)
		{
#if Windows || Android
#if Windows
			uc.Dispatcher.Invoke(() =>
#endif
#if Android
            Device.BeginInvokeOnMainThread(()=>
#endif
			{
				var oldItem = Items.Where((BaseDataObject arg) => arg.GuId == id).FirstOrDefault();
				Items.Remove(oldItem);
			});
#endif

			return await Task.FromResult(true);
		}

		public async Task<BaseDataObject> GetItemAsync(Guid id)
		{
			return await Task.FromResult(Items.FirstOrDefault(s => s.GuId == id));
		}

		public async Task<IEnumerable<BaseDataObject>> GetItemsAsync(bool forceRefresh = false)
		{
			return await Task.FromResult(Items);
		}
	}
}
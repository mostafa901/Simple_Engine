using Newtonsoft.Json;
using SQLite;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using Utility.IO;


#if !Android && !Console

using System.Windows;
using System.Windows.Media;

#endif
#if Android
using Xamarin.Forms;
#endif

namespace Utility.MVVM
{
	public class BaseDataObject : ObservableObject
	{
		public BaseDataObject()
		{
			GuId = Guid.NewGuid();
			_MyAddress = GuId.ToString();
#if OrgSitu
			LastServerSync = DateTime.Now;
#elif Android
			LastClientSync = DateTime.Now; 

#endif
		}

		//this just a store for any data in case needed in a converter<--See PhotoConverter
		//this just a store for textselection in case needed in a converter<--See UC_ParamEdit

		#region DataContext

		private object _DataContext;

		[Ignore]
		public object DataContext
		{
			get
			{
				return _DataContext;
			}
			set { SetProperty(ref _DataContext, value); }
		}

		#endregion DataContext

		private string _MyAddress;

		public virtual string MyAddress { get { return GuId.ToString(); } set { _MyAddress = value; } }

		#region LastServerSync

		private DateTime _LastServerSync;

		public DateTime LastServerSync
		{
			get
			{
				return _LastServerSync;
			}
			set { SetProperty(ref _LastServerSync, value); }
		}

		#endregion LastServerSync

		#region LastClientSync

		private DateTime _LastClientSync;

		public DateTime LastClientSync
		{
			get
			{
				return _LastClientSync;
			}
			set { SetProperty(ref _LastClientSync, value); }
		}

		#endregion LastClientSync



		[Ignore]
		[JsonIgnore]
		public bool? ActionResult { get; set; } = false;

		#region Name

		private string name = "";

		[Display(AutoGenerateField = true, Order = 2)]
		virtual public string Name
		{
			get
			{
				return name;
			}
			set { SetProperty(ref name, value); }
		}

		#endregion Name

		#region Code

		private string code;

		[Display(AutoGenerateField = true, Order = 1)]
		virtual public string Code
		{
			get
			{
				return code;
			}
			set { SetProperty(ref code, value); }
		}

		#endregion Code

		#region CMD

		private cus_CMD cMD;

		[XmlIgnore]
		[Ignore]
		[JsonIgnore]
		public cus_CMD CMD
		{
			get
			{
				if (cMD == null) cMD = new cus_CMD();
				return cMD;
			}
			set { SetProperty(ref cMD, value); }
		}

		#endregion CMD

		#region Typical Create

		public static BaseDataObject Create(string name, Action<object> command = null)
		{
			return new BaseDataObject()
			{
				Name = name,
				ToolTip = name,
				CMD = new cus_CMD()
				{
					Action = command == null ? (a) => { } : command
				}
			};
		}

		#endregion Typical Create

		#region Icon

		private object _Icon;

		[Ignore]
		[JsonIgnore]
		public object Icon
		{
			get
			{
				return _Icon;
			}
			set { SetProperty(ref _Icon, value); }
		}

		#endregion Icon


		#region Width

		private double _Width=25;
		[Ignore]
		public double Width
		{
			get
			{
				return _Width;
			}
			set { SetProperty(ref _Width, value); }

		}
		#endregion

		#region Height

		private double _Height=25;
		[Ignore]
		public double Height
		{
			get
			{
				return _Height;
			}
			set { SetProperty(ref _Height, value); }

		}
		#endregion


		#region Angle

		private double _Angle;
		[Ignore]
		public double Angle
		{
			get
			{
				return _Angle;
			}
			set { SetProperty(ref _Angle, value); }

		}
		#endregion



		#region ToolTip

		private string _ToolTip;

		[Ignore]
		[JsonIgnore]
		public string ToolTip
		{
			get
			{
				return _ToolTip;
			}
			set { SetProperty(ref _ToolTip, value); }
		}

		#endregion ToolTip

#if true

		#region DisBrush

#if Windows || Android
#if Windows
		private Brush _DisBrush;

		[Ignore]
		[JsonIgnore]
		public Brush DisBrush

#endif
#if Android
		private Color _DisBrush;
        [Ignore] [JsonIgnore]
        public Color DisBrush
#endif

		{
			get
			{
				return _DisBrush;
			}
			set
			{
				SetProperty(ref _DisBrush, value);
			}
		}

#endif
		#endregion DisBrush

#endif

		#region IsVisible

		#region IsVisible

		private bool _IsVisible = true;

		public bool IsVisible
		{
			get
			{
				return _IsVisible;
			}
			set { SetProperty(ref _IsVisible, value); }
		}

		#endregion IsVisible

#if !Android && !Console

		private Visibility _Visible;

		[Ignore]
		[JsonIgnore]
		virtual public Visibility Visible

		{
			get
			{
				return _Visible;
			}
			set { SetProperty(ref _Visible, value); }
		}

#endif

		#endregion IsVisible

		#region IsBusy

		#region IsBusy

		private bool _IsBusy = false;

		[Ignore]
		[JsonIgnore]
		public bool IsBusy
		{
			get
			{
				return _IsBusy;
			}
			set { SetProperty(ref _IsBusy, value); }
		}

		#endregion IsBusy

		#endregion IsBusy

#if !RAAPSII && !Android
		public override string ToString() => this.GetStrings();

#endif
	}

}
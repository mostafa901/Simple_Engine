using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Utility.MVVM
{

	public class cus_CMD : ICommand
	{
		public enum ButtonStat
		{
			CanExecute,
			DontExecute
		}

		public event EventHandler CanExecuteChanged;

		Action<object> action = (a) => { };

		public Action<object> Action { get => action; set => action= value; }


		public bool CanExecute(object parameter) => true;

		public ButtonStat CommandState { get; set; }

		public void Execute(object parameter)
		{
			if (CommandState == ButtonStat.CanExecute) Action(parameter);
		}
	}
}
using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
	public class NetworkDisplayViewModel : BindableBase
	{

		public static void UpdateList(Saobracaj e)
		{
			for (int i = 0; i < EntitetList.Count; i++)
				if (EntitetList[i].Id == e.Id)
				{
					EntitetList[i].Valued = e.Valued;
					return;
				}

			
		}


		public static ObservableCollection<Saobracaj> EntitetList { get; set; }



	}
}

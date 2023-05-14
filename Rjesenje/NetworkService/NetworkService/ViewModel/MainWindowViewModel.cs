using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
	public class MainWindowViewModel : BindableBase
	{
		public MyICommand<string> NavigationCommand { get; private set; }
		public MyICommand ChangeCommand { get; set; }
		private NetworkEntitiesViewModel NEVM = new NetworkEntitiesViewModel();
		private NetworkDisplayViewModel NDVM = new NetworkDisplayViewModel();
		private MeasurementGraphViewModel MGVM = new MeasurementGraphViewModel();


		string path = @"C:\Users\Korisnik\Documents\GitHub\HCI-PZ2\Rjesenje\NetworkServiceLog.txt";

	}
}

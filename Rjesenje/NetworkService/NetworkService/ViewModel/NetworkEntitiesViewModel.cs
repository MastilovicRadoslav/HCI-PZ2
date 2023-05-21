using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
	public class NetworkEntitiesViewModel : BindableBase
	{
		public static ObservableCollection<Saobracaj> Entiteti { get; set; }  //omogucava notifikacije kada se elementi dodaju, brisu ili se sama kolekcija izmijeni

		public static ICollectionView PrikazEntiteta { get; set; }
		public static List<Model.TypeSaobracaj> Tipovi { get; set; }
		public MyICommand AddCommand { get; set; }	//Dodavanje
		public MyICommand DeleteCommand { get; set; }	//Brisanje
		public MyICommand FilterCommand { get; set; }	 //Filtracija
		public MyICommand HelpCommand { get; set; }		  //Help

		bool toolTipsBool;

		public bool ToolTipsBool
		{
			get => toolTipsBool;
			set
			{
				toolTipsBool = value;
				MainWindowViewModel.UseToolTips = value;
				OnPropertyChanged("ToolTipsBool");
			}
		}

		Saobracaj noviEntitet = new Saobracaj();
		public Saobracaj NoviEntitet
		{
			get => noviEntitet;
			set
			{
				noviEntitet = value;
				OnPropertyChanged("NoviEntitet");
			}
		}

		Saobracaj izabran;
		public Saobracaj Izabran
		{
			get => izabran;
			set
			{
				izabran = value;
				DeleteCommand.RaiseCanExecuteChanged();
			}
		}

	}
}

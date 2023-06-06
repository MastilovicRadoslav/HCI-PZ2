using NetworkService.Helpers;
using NetworkService.Model;
using System.Collections.Generic;

namespace NetworkService.ViewModel
{
	public class MeasurementGraphViewModel : BindableBase
	{
		public static GraphUpdater ElementHeights { get; set; } = new GraphUpdater();   //Ovo je svojstvo koje predstavlja objekat GraphUpdater i koristi se za ažuriranje visina elemenata grafa. Podrazumevana vrednost je nova instanca GraphUpdater.
		private static int idForShow { get; set; } = -1;  //Ovo je privatno statičko svojstvo koje predstavlja identifikator za prikazivanje određenog elementa. Podrazumevana vrednost je -1, što označava da nijedan element nije odabran za prikaz.

		public int IDForShow
		{
			get { return idForShow; }
			set { if (idForShow != value) { idForShow = value; } }
		}

		public static double CalculateElementHeight(double value, int index)
		{
			// Metoda za izračunavanje visine elementa na grafu
			// Ako je ID elementa jednak idForShow ili idForShow je -1 (prikazuje se sve),
			// izračunava se visina elementa na osnovu zadate formule
			if (idForShow == index || idForShow == -1) { return 60 + (18500 - value) * 0.0108; }
			return 100;
		}

		// Odavde je za filter.
		private string idSaobracaj;
		public MyICommand ShowCommand { get; set; }
		public MyICommand HelpCommand { get; set; }                         //Help komanda

		public List<int> ComboBoxData { get; set; } = new List<int>();
		//private string path = Environment.CurrentDirectory + @"\LogFile.txt";

		public MeasurementGraphViewModel()
		{
			ToolTipsBool = MainWindowViewModel.UseToolTips;	   //ToolTip
			HelpCommand = new MyICommand(OnHelp);
			ShowCommand = new MyICommand(OnShow);
			foreach (Entitie e in NetworkEntitiesViewModel.Entiteti) { ComboBoxData.Add(e.Id); }   //sa prvog prozora ucitaj odje za prikaz
		}

		//Help
		string helpText;
		static string saveHelp = "";
		public string HelpText
		{
			get => helpText;
			set
			{
				helpText = value;
				saveHelp = value;
				OnPropertyChanged("HelpText");
				HelpCommand.RaiseCanExecuteChanged();
			}
		}
		//Komadna
		private void OnHelp()
		{
			if (HelpText == string.Empty)
			{
				HelpText = "U koliko vam je potrebna pomoć,prečice su sledeće:" + "\nCTRL+DH -> Help\nCTRL+S -> ComboBox prikaz " +
						   "\nMeasurementGraph prikazuje podatke na svom grafiku, podaci zavise od vrijednosti koje se dobiju iz " +
						   "MeteringSimulator T3";
			}
			else
			{
				HelpText = string.Empty;
			}
		}


		public string IdSaobracaj
		{
			get { return idSaobracaj; }
			set
			{
				idSaobracaj = value;
				OnPropertyChanged("IdSaobracaj");
				ShowCommand.RaiseCanExecuteChanged();
			}
		}

		private int selectedSaobracajType;    // Pamcenje tipa saobracaja koji smo izabrali. 
		public int SelectedSaobracajType
		{
			get { return selectedSaobracajType; }
			set
			{
				if (selectedSaobracajType != value)
				{
					selectedSaobracajType = value;
					OnPropertyChanged("SelectedSaobracajType");
				}
			}
		}

		private bool CanShow()
		{
			return idForShow != -1;
		}

		private void OnShow()
		{
			idForShow = selectedSaobracajType;	 //id tipa saobracaja za prikaz
		}

		bool toolTipsBool;                                                  //bool promjenljiva za ToolTip

		public bool ToolTipsBool                                            //Property za ToolTip
		{
			get => toolTipsBool;
			set
			{
				toolTipsBool = value;
				MainWindowViewModel.UseToolTips = value;
				OnPropertyChanged("ToolTipsBool");
			}
		}
	}
}

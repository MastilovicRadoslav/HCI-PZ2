using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class MeasurementGraphViewModel : BindableBase
    {
		public static GraphUpdater ElementHeights { get; set; } = new GraphUpdater();
		private static int idForShow { get; set; } = -1;

		public int IDForShow
		{
			get { return idForShow; }
			set { if (idForShow != value) { idForShow = value; } }
		}

		public static double CalculateElementHeight(double value, int index)
		{
			if (idForShow == index || idForShow == -1) { return 60 + (10 - value) * 20; }
			return 260;
		}

		// Odavde je za filter.
		private string idReactor;
		public MyICommand ShowCommand { get; set; }
		public List<int> ComboBoxData { get; set; } = new List<int>();
		//private string path = Environment.CurrentDirectory + @"\LogFile.txt";

		public MeasurementGraphViewModel()
		{
			ShowCommand = new MyICommand(OnShow);
			foreach (Entitie e in NetworkEntitiesViewModel.Entiteti) { ComboBoxData.Add(e.Id); }
		}


		public string IdReactor
		{
			get { return idReactor; }
			set
			{
				idReactor = value;
				OnPropertyChanged("IdReactor");
				ShowCommand.RaiseCanExecuteChanged();
			}
		}

		private int selectedGeneratorType;    // Pamcenje tipa generatora koji smo izabrali. 
		public int SelectedGeneratorType
		{
			get { return selectedGeneratorType; }
			set
			{
				if (selectedGeneratorType != value)
				{
					selectedGeneratorType = value;
					OnPropertyChanged("SelectedGeneratorType");
				}
			}
		}

		private bool CanShow()
		{
			return idForShow != -1;
		}

		private void OnShow()
		{
			idForShow = selectedGeneratorType;
		}



	}
}

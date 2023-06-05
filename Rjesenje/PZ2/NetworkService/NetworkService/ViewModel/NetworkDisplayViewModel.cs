using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NetworkService.Views;
using System.Windows.Input;
using NetworkService.Helpers;

namespace NetworkService.ViewModel
{
	public class NetworkDisplayViewModel : BindableBase
	{
		// Metoda za uklanjanje entiteta iz liste
		public static void RemoveFromList(Entitie e)      //Izbrisan na prvom prozoru, izbrisi i ovdje
		{
			foreach (Entitie entitet in EntitetList)
				if (entitet.Id == e.Id)
				{
					EntitetList.Remove(entitet);
					return;
				}

			for (int i = 0; i < 12; i++)                  //Izbrisi i liniju za taj entitet
				if (Canvases[i].Entitet.Id == e.Id)
				{
					foreach (int id in Canvases[i].Lines)
						RemoveFromList(id);
					Canvases[i] = new CanvasInfo(i);
					return;
				}
		}

		// Metoda za ažuriranje liste
		public static void UpdateList(Entitie e)           //Azuriraj listu
		{
			for (int i = 0; i < EntitetList.Count; i++)
				if (EntitetList[i].Id == e.Id)
				{
					EntitetList[i].Valued = e.Valued;
					return;
				}

			for (int i = 0; i < 12; i++)                   //Kao i liniiju
				if (Canvases[i].Entitet.Id == e.Id)
				{
					Canvases[i].Entitet = e;
					return;
				}
		}

		// Lista entiteta
		public static ObservableCollection<Entitie> EntitetList { get; set; }     //Lista entiteta u koju dodajem one koje su u tabeli na prvom prozoru

		// Lista kanvasa
		public static ObservableCollection<CanvasInfo> Canvases { get; set; }     //Kanvas na slici da bude ono ao greska i 

		// Lista linija
		public static ObservableCollection<Line> Lines { get; set; }

		// Komanda koja se izvršava kada se promeni selekcija u ListView kontroli
		public MyICommand<ListView> SelectionChangedCommand { get; set; }

		// Komanda koja se izvršava kada se otpusti levi taster miša
		public MyICommand MouseLeftButtonUpCommand { get; set; }

		// Komanda koja se izvršava kada se pritisne dugme na Canvas kontroli
		public MyICommand<Canvas> ButtonCommand { get; set; }

		// Komanda koja se izvršava kada se pređe mišem preko Canvas kontrole
		public MyICommand<Canvas> DragOverCommand { get; set; }

		// Komanda koja se izvršava kada se nešto prevuče na Canvas kontrolu
		public MyICommand<Canvas> DropCommand { get; set; }

		// Komanda koja se izvršava kada se pritisne levi taster miša na Canvas kontroli
		public MyICommand<Canvas> MouseLeftButtonDownCommand { get; set; }

		// Komanda za automatsko pozicioniranje entiteta
		public MyICommand AutoPlaceCommand { get; set; }

		// Komanda za prikazivanje pomoći
		public MyICommand HelpCommand { get; set; }

		bool toolTipsBool;
		// Vrednost za prikazivanje ili skrivanje ToolTips-a
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

		string helpText;
		static string saveHelp = "";
		// Tekst koji se prikazuje u Help sekciji
		public string HelpText
		{
			get => helpText;
			set
			{
				helpText = value;
				saveHelp = value;
				OnPropertyChanged("HelpText");
			}
		}

		bool dragging = false;
		Entitie selectedEntitet;
		// Selektovani entitet
		public Entitie SelectedEntitet
		{
			get => selectedEntitet;
			set
			{
				selectedEntitet = value;
				OnPropertyChanged("SelectedEntitet");
			}
		}

		CanvasInfo currentCanvas;
		// Trenutno odabrani Canvas
		public CanvasInfo CurrentCanvas
		{
			get => currentCanvas;
			set
			{
				currentCanvas = value;
				OnPropertyChanged("CurrentCanvas");
			}
		}

		bool Cmp(CanvasInfo c)
		{
			return CurrentCanvas.Entitet == c.Entitet && CurrentCanvas.Taken == c.Taken && CurrentCanvas.Text == c.Text;
		}

		// Metoda za automatsko pozicioniranje entiteta
		private void OnAutoPlace()
		{
			List<Entitie> temp = new List<Entitie>();
			foreach (Entitie e in EntitetList)
			{
				for (int i = 0; i < 12; i++)
				{
					if (!Canvases[i].Taken)
					{
						Canvases[i] = new CanvasInfo(e, true, i);
						temp.Add(e);
						break;
					}
				}
			}

			foreach (Entitie e in temp)
				EntitetList.Remove(e);
		}

		// Konstruktor klase
		public NetworkDisplayViewModel()
		{
			if (EntitetList == null)
				EntitetList = new ObservableCollection<Entitie>();
			if (Canvases == null)
			{
				Canvases = new ObservableCollection<CanvasInfo>();
				for (int i = 0; i < 12; i++)
					Canvases.Add(new CanvasInfo(i));
			}
			if (Lines == null)
				Lines = new ObservableCollection<Line>();

			DragOverCommand = new MyICommand<Canvas>(DragOver);
			DropCommand = new MyICommand<Canvas>(Drop);
			ButtonCommand = new MyICommand<Canvas>(ButtonCommandFreeing);
			SelectionChangedCommand = new MyICommand<ListView>(SelectionChanged);
			MouseLeftButtonUpCommand = new MyICommand(MouseLeftButtonUp);
			MouseLeftButtonDownCommand = new MyICommand<Canvas>(MouseLeftButtonDown);
			AutoPlaceCommand = new MyICommand(OnAutoPlace);
			HelpCommand = new MyICommand(OnHelp);
			helpText = saveHelp;
			ToolTipsBool = MainWindowViewModel.UseToolTips;
		}

		// Metoda za prikazivanje ili skrivanje ToolTips-a
		private void OnHelp()
		{
			if (ToolTipsBool)
			{
				ToolTipsBool = false;
				HelpText = "";
			}
			else
			{
				ToolTipsBool = true;
				HelpText = saveHelp;
			}
		}

		// Metoda koja se izvršava kada se otpusti levi taster miša
		private void MouseLeftButtonUp()
		{
			dragging = false;
		}

		// Metoda koja se izvršava kada se pritisne levi taster miša na Canvas kontroli
		private void MouseLeftButtonDown(Canvas canvas)
		{
			dragging = true;
			if (canvas.DataContext is CanvasInfo canvasInfo)
			{
				CurrentCanvas = canvasInfo;
				SelectedEntitet = CurrentCanvas.Entitet;
			}
		}

		// Metoda koja se izvršava kada se promeni selekcija u ListView kontroli
		private void SelectionChanged(ListView listView)
		{
			if (listView.SelectedItem is Entitie entitet)
			{
				SelectedEntitet = entitet;
				CurrentCanvas = Canvases.FirstOrDefault(c => c.Entitet == entitet);
			}
		}

		// Metoda koja se izvršava kada se nešto prevuče na Canvas kontrolu
		private void Drop(Canvas canvas)
		{
			if (canvas.DataContext is CanvasInfo canvasInfo && SelectedEntitet != null)
			{
				if (canvasInfo.Taken && CurrentCanvas != canvasInfo)
					RemoveFromList(canvasInfo.Entitet);

				if (!canvasInfo.Taken)
				{
					canvasInfo.Entitet = SelectedEntitet;
					canvasInfo.Taken = true;
					RemoveFromList(SelectedEntitet);
					CurrentCanvas = canvasInfo;
				}
			}
		}

		// Metoda koja se izvršava kada se pređe mišem preko Canvas kontrole
		private void DragOver(Canvas canvas)
		{
			if (canvas.DataContext is CanvasInfo canvasInfo && SelectedEntitet != null)
			{
				if (!canvasInfo.Taken)
				{
					canvasInfo.Entitet = SelectedEntitet;
					canvasInfo.Taken = true;
					RemoveFromList(SelectedEntitet);
					CurrentCanvas = canvasInfo;
				}
			}
		}

		// Metoda koja se izvršava kada se pritisne dugme na Canvas kontroli
		private void ButtonCommandFreeing(Canvas canvas)
		{
			if (canvas.DataContext is CanvasInfo canvasInfo)
			{
				RemoveFromList(canvasInfo.Entitet);
			}
		}
	}
}

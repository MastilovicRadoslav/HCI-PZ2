using NetworkService.Helpers;
using NetworkService.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace NetworkService.ViewModel
{
	public class NetworkDisplayViewModel : BindableBase
	{
		public static void RemoveFromList(Entitie e)
		{
			// Provera svakog entiteta u listi EntitetList
			foreach (Entitie entitet in EntitetList)	 //ovo je u ListView
			{
				// Ako se identifikatori podudaraju
				if (entitet.Id == e.Id)
				{
					// Ukloni entitet iz liste
					EntitetList.Remove(entitet);
					return;
				}
			}

			// Provera za svaki Canvas u Canvases
			for (int i = 0; i < 12; i++)				//ovo je u Canvas
			{
				// Ako se identifikatori entiteta na Canvas-u podudaraju
				if (Canvases[i].Entitet.Id == e.Id)
				{
					// Ukloni sve linije povezane sa tim entitetom
					foreach (int id in Canvases[i].Lines)
					{
						RemoveLine(id);
					}

					// Resetuj Canvas informacije
					Canvases[i] = new CanvasInfo(i);
					return;
				}
			}
		}

		public static void UpdateList(Entitie e)	  //ako se posalju nove vrijednosti iz MeteringSimulator, azuriram na prvom prozoru tabelu a na ovome ListView
		{
			// Provera svakog entiteta u listi EntitetList
			for (int i = 0; i < EntitetList.Count; i++)
			{
				// Ako se identifikatori podudaraju
				if (EntitetList[i].Id == e.Id)
				{
					// Ažuriraj vrednost entiteta
					EntitetList[i].Valued = e.Valued;
					return;
				}
			}

			// Provera za svaki Canvas u Canvases
			for (int i = 0; i < 12; i++)					//Isto i za Canvas
			{
				// Ako se identifikatori entiteta na Canvas-u podudaraju
				if (Canvases[i].Entitet.Id == e.Id)
				{
					// Ažuriraj entitet na Canvas-u
					Canvases[i].Entitet = e;
					return;
				}
			}
		}

		public static ObservableCollection<Entitie> EntitetList { get; set; }     //Lista entiteta u koju dodajem one koje su u tabeli na prvom prozoru
		public static ObservableCollection<CanvasInfo> Canvases { get; set; }     //Kanvas na slici za iscrtavanje linija, 12 canvas kontrola, za background - text - foreground
		public static ObservableCollection<Line> Lines { get; set; }			  //Linije kao klasa, koje se prikazuju izmedju 12 Canvas kontrola
		public MyICommand<ListView> SelectionChangedCommand { get; set; }		  //Ono sto se prevlaci
		public MyICommand MouseLeftButtonUpCommand { get; set; }         //Kada se levi taster miša otpusti na platnu	
		public MyICommand<Canvas> ButtonCommand { get; set; }			 //Button comanda za oslobadjanje neke od 12 Canvas kontrola
		public MyICommand<Canvas> DragOverCommand { get; set; }     //Kada se prevlači preko platna
		public MyICommand<Canvas> DropCommand { get; set; }      //Kada se element prevuče i ispusti na platno
		public MyICommand<Canvas> MouseLeftButtonDownCommand { get; set; }         //događaja levog klika miša i odgovarajuće komande
		public MyICommand AutoPlaceCommand { get; set; }						  //automatska raspodjela na canvas kontrole
		public MyICommand HelpCommand { get; set; }								//isto

		bool toolTipsBool;
		public bool ToolTipsBool  //ToolTip kao i na prvom prozoru
		{
			get => toolTipsBool;
			set
			{
				toolTipsBool = value;
				MainWindowViewModel.UseToolTips = value;
				OnPropertyChanged("ToolTipsBool");
			}
		}

		string helpText;					 //isto
		static string saveHelp = "";
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
		public Entitie SelectedEntitet	 //Koji je entitet selektovan u treeView - u
		{
			get => selectedEntitet;
			set
			{
				selectedEntitet = value;
				OnPropertyChanged("SelectedEntitet");
			}

		}

		CanvasInfo currentCanvas;                 //trenutni canvas, entitet je prevučen između Canvase-ova),
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
			// Proverava da li su odgovarajući atributi CurrentCanvas objekta jednaki atributima objekta c
			return CurrentCanvas.Entitet == c.Entitet && CurrentCanvas.Taken == c.Taken && CurrentCanvas.Text == c.Text;
		}


		private void OnAutoPlace() // Automatsko stavljanje entiteta
		{
			List<Entitie> temp = new List<Entitie>();

			// Provera svakog entiteta u listi EntitetList
			foreach (Entitie e in EntitetList)
			{
				// Provera za svaki Canvas u Canvases
				for (int i = 0; i < 12; i++)
				{
					// Ako je Canvas slobodan (nije zauzet)
					if (!Canvases[i].Taken)
					{
						// Postavi entitet na taj Canvas i označi ga kao zauzetog
						Canvases[i] = new CanvasInfo(e, true, i);
						temp.Add(e); // Dodaj entitet u privremenu listu
						break;
					}
				}
			}

			// Ukloni entitete koji su postavljeni na Canvase iz EntitetList
			foreach (Entitie e in temp)
			{
				EntitetList.Remove(e);
			}
		}


		public NetworkDisplayViewModel()
		{
			if (EntitetList == null)
				EntitetList = new ObservableCollection<Entitie>();	   //inicijalizujem listu u treeView
			if (Canvases == null)  //Inicijalizacija CanvasInfo za 12 canvas - a za prevlacenje
			{
				Canvases = new ObservableCollection<CanvasInfo>();
				for (int i = 0; i < 12; i++)   //Inicijalizacija novih 12 canvas kontrola
					Canvases.Add(new CanvasInfo(i));
			}
			if (Lines == null)			  //Inicijalizacija linija
				Lines = new ObservableCollection<Line>();

			DragOverCommand = new MyICommand<Canvas>(DragOver);         //Kada se prevlači preko platna
			DropCommand = new MyICommand<Canvas>(Drop);                 //Kada se element prevuče i ispusti na platno
			ButtonCommand = new MyICommand<Canvas>(ButtonCommandFreeing);  //inicijalizacija ButtomCommand kontrole
			SelectionChangedCommand = new MyICommand<ListView>(SelectionChanged);	   //inicijalizacija onog sto se prevlaci
			MouseLeftButtonUpCommand = new MyICommand(MouseLeftButtonUp);
			MouseLeftButtonDownCommand = new MyICommand<Canvas>(MouseLeftButtonDown);
			AutoPlaceCommand = new MyICommand(OnAutoPlace);
			HelpCommand = new MyICommand(OnHelp);
			helpText = saveHelp;
			ToolTipsBool = MainWindowViewModel.UseToolTips;
		}

		private void OnHelp()	 //Isto kao prvi prozor
		{
			if (HelpText == "")
			{
				HelpText = "Prečice su sledeće:\nCTRL+D -> Automatsko stavljanje entiteta na mesta\nCtrl+H -> Help\nCtrl+Tab pomjeranje izmedju prozora" +
						   "Prevlačenjem entiteta iz liste u odabrano polje će rezultirati prebacivanjem entiteta iz liste" +
						   " u to polje za prikaz trenutnog stanja tog entiteta.Prevlačenjem entiteta iz polja" +
						   " u polje ce rezultirati prebacivanjem entiteta iz polja u polje.\nPovlačenje linije" +
						   " izmedju 2 entiteta se radi povlačenjem prvog zauzetog polja na drugo polje.";
			}
			else
			{
				HelpText = "";
			}
		}

		void ChangeLine(int id, int x, int y, int nx, int ny)
		{
			for (int i = 0; i < Lines.Count; i++)
			{
				if (Lines[i].Id == id)
				{
					// Proverava da li trenutne koordinate (x, y) odgovaraju tački početka linije
					if (Lines[i].X1 == x && Lines[i].Y1 == y)
					{
						// Ako odgovaraju, ažuriraju se koordinate tačke početka linije
						Lines[i].X1 = nx;
						Lines[i].Y1 = ny;
					}
					else
					{
						// Ako trenutne koordinate (x, y) odgovaraju tački kraja linije,
						// ažuriraju se koordinate tačke kraja linije
						Lines[i].X2 = nx;
						Lines[i].Y2 = ny;
					}
					return;
				}
			}
		}


		private void Drop(Canvas obj)
		{
			// Metoda omogućava postavljanje entiteta na Canvas, zamenu Canvas-a i povezivanje linija između Canvas-a, 
			// u zavisnosti od odabranih entiteta i stanja Canvas-a.

			if (SelectedEntitet != null)
			{
				// Ako je selektovani entitet postavljen na Canvas
				int id = int.Parse(obj.Name.Substring(1)); // Izdvajanje identifikatora Canvas-a
				if (!Canvases[id].Taken)
				{
					// Postavi selektovani entitet na odabrani Canvas i označi ga kao zauzetog
					Canvases[id] = new CanvasInfo(SelectedEntitet, true, id);
					EntitetList.Remove(SelectedEntitet); // Ukloni entitet iz liste EntitetList
				}
			}
			else if (CurrentCanvas != null)
			{
				// Ako postoji trenutni Canvas (entitet je prevučen između Canvas-a)
				int id = int.Parse(obj.Name.Substring(1)); // Izdvajanje identifikatora Canvas-a
				if (!Canvases[id].Taken)
				{
					// Pronađi slobodan Canvas za zamenu sa trenutnim Canvas-om
					for (int i = 0; i < 12; i++)
					{
						if (Cmp(Canvases[i]))
						{
							// Zamena trenutnog Canvas-a sa slobodnim Canvas-om
							Canvases[i] = new CanvasInfo(i);
							break;
						}
					}

					// Postavi entitet sa trenutnog Canvas-a na odabrani Canvas i označi ga kao zauzetog
					Canvases[id] = new CanvasInfo(CurrentCanvas.Entitet, true, id);

					// Promena linija koje povezuju Canvas-e
					foreach (int i in CurrentCanvas.Lines)
					{
						// Ažuriranje linija koje su povezane sa trenutnim Canvas-om
						ChangeLine(i, CurrentCanvas.X, CurrentCanvas.Y, Canvases[id].X, Canvases[id].Y);
						Canvases[id].Lines.Add(i);
					}
				}
				else
				{
					// Ako je odabrani Canvas zauzet
					for (int i = 0; i < 12; i++)
					{
						if (Cmp(Canvases[i]))
						{
							// Kreiranje nove linije između slobodnog i zauzetog Canvas-a
							Line line = new Line(Canvases[i].X, Canvases[id].X, Canvases[i].Y, Canvases[id].Y);
							Lines.Add(line);
							Canvases[i].Lines.Add(line.Id);
							Canvases[id].Lines.Add(line.Id);
							break;
						}
					}
				}
			}
			MouseLeftButtonUp(); // Završetak akcije prevlačenja miša
		}



		private void DragOver(Canvas obj)
		{
			int id = int.Parse(obj.Name.Substring(1));	  //Uzimanje Id

			// Provera da li Canvases[id] ima zauzetu poziciju
			if (!Canvases[id].Taken)
				obj.AllowDrop = true;   // Dozvoljeno spuštanje entiteta na Canvas
			else
				obj.AllowDrop = false;  // Zabranjeno spuštanje entiteta na Canvas
		}


		private void MouseLeftButtonUp()
		{
			SelectedEntitet = null;   // Poništava selektovani entitet (postavlja na null)
			CurrentCanvas = null;     // Poništava trenutni Canvas (postavlja na null)
			dragging = false;         // Postavlja indikator prevlačenja (dragging) na false
		}



		private void MouseLeftButtonDown(Canvas c) //klik lijevi misa na Canvas kontrolu
		{
			int id = int.Parse(c.Name.Substring(1));

			// Proverava da li Canvases[id] ima zauzetu poziciju
			if (Canvases[id].Taken)
			{
				CurrentCanvas = Canvases[id];  // Postavlja trenutni Canvas na Canvases[id]

				// Proverava da li je prethodno prevlačenje u toku
				if (!dragging)
				{
					dragging = true;  // Postavlja indikator prevlačenja (dragging) na true

					// Pokreće operaciju prevlačenja entiteta pomoću metode DoDragDrop
					// Prilikom prevlačenja, prenosi se referenca na trenutni Canvas (CurrentCanvas)
					DragDrop.DoDragDrop(c, CurrentCanvas, DragDropEffects.Copy | DragDropEffects.Move);
				}
			}
		}


		static void RemoveLine(int id)    //Metoda RemoveLine je statička metoda koja se koristi za uklanjanje linije sa određenim identifikatorom iz liste linija (Lines)
		{
			for (int i = 0; i < Lines.Count; i++)
			{
				// Provera da li identifikator linije odgovara traženom identifikatoru
				if (Lines[i].Id == id)     //Ukoliko se identifikatori poklapaju, to znači da je pronađena linija koju treba ukloniti.
				{
					// Uklanjanje linije iz liste
					Lines.RemoveAt(i);
					return;
				}
			}
		}

		private void ButtonCommandFreeing(Canvas obj)     //Ova metoda ima za cilj da oslobodi Canvas kontrolu, uklanjajući povezane linije i dodajući entitet u listu, kako bi se mesto na Canvas-u ponovo koristilo
		{
			// Izvlačenje identifikatora iz naziva Canvas kontrola
			int id = int.Parse(obj.Name.Substring(1));	 //tamo sam davo imena npr. c0 sto znaci da je id = 0

			// Provera da li je Canvas kontrola zauzeta
			if (Canvases[id].Taken)
			{
				// Iteriranje kroz listu linija povezanih sa Canvas kontrolom
				foreach (int i in Canvases[id].Lines)
					RemoveLine(i);		 //I njih obrisi

				// Dodavanje entiteta iz oslobađene Canvas kontrole u EntitetListu
				EntitetList.Add(Canvases[id].Entitet);	 //Vracanje nazad u ListView

				// Kreiranje nove instance CanvasInfo za oslobađenu Canvas kontrolu
				Canvases[id] = new CanvasInfo(id);		//postavljam je u pocetno stanje
			}
		}


		private void SelectionChanged(ListView obj)
		{
			// Proverava da li je prethodno prevlačenje u toku
			if (!dragging)
			{
				dragging = true;  // Postavlja indikator prevlačenja (dragging) na true

				// Pokreće operaciju prevlačenja entiteta pomoću metode DoDragDrop
				// Prenosi se referenca na izabrani entitet (SelectedEntitet) kao podaci koji se prevlače
				DragDrop.DoDragDrop(obj, SelectedEntitet, DragDropEffects.Copy | DragDropEffects.Move);
			}
		}

	}
}

using NetworkService.Helpers; // Uključivanje potrebnog namespace-a
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.Model
{
	public class CanvasInfo : BindableBase
	{
		Entitie entitet; // Privatno polje za entitet
		bool taken; // Privatno polje koje označava da li je polje zauzeto
		int x, y; // Privatna polja za X i Y koordinate
		ObservableCollection<int> lines; // Privatna kolekcija za linije

		public CanvasInfo(int ind)
		{
			Taken = false; // Inicijalizacija Taken na false
			Entitet = new Entitie(); // Kreiranje nove instance klase Entitie
			X = 10 + (ind % 4 + 1) * 100 + (ind % 4) * 160; // Izračunavanje vrednosti X koordinate
			Y = 85 + (ind / 4) * 200; // Izračunavanje vrednosti Y koordinate
			lines = new ObservableCollection<int>(); // Kreiranje nove instance observable kolekcije
		}

		public CanvasInfo(Entitie entitet, bool taken, int ind)
		{
			this.Entitet = entitet; // Postavljanje vrednosti Entitet polja na prosleđeni entitet
			this.Taken = taken; // Postavljanje vrednosti Taken polja na prosleđeni boolean
			X = 10 + (ind % 4 + 1) * 100 + (ind % 4) * 160; // Izračunavanje vrednosti X koordinate
			Y = 85 + (ind / 4) * 200; // Izračunavanje vrednosti Y koordinate
			lines = new ObservableCollection<int>(); // Kreiranje nove instance observable kolekcije
		}

		public Brush Background
		{
			get
			{
				if (Entitet.Type != null) // Provera da li entitet ima tip
				{
					BitmapImage slika = new BitmapImage(); // Kreiranje nove instance BitmapImage
					slika.BeginInit();
					slika.UriSource = new Uri(Environment.CurrentDirectory + "../../../" + Entitet.Type.Img_src); // Postavljanje izvora slike na osnovu putanje
					slika.EndInit();
					return new ImageBrush(slika); // Vraćanje nove instance ImageBrush sa slikom
				}
				else
					return Brushes.GhostWhite; // Ako entitet nema tip, vraća se GhostWhite boja
			}
		}

		public string Text { get => Entitet.Name != null ? "Id: " + Entitet.Id + " Name: " + Entitet.Name + "Type: " + Entitet.Type.Name : ""; } // Vraćanje teksta koji prikazuje informacije o entitetu

		public string Foreground { get => Uslov() ? "Blue" : "Red"; } // Vraćanje boje teksta na osnovu određenog uslova

		public bool Uslov()
		{
			if (Entitet.Type != null) // Provera da li entitet ima tip
			{
				if ((Entitet.Type.Name.Contains("iA") && Entitet.Valued > 15000) || (Entitet.Type.Name.Contains("iB") && Entitet.Valued > 7000)) // Provera određenog uslova
				{
					return false; // Vraćanje false ako uslov nije ispunjen
				}
			}

			return true; // Vraćanje true ako je uslov ispunjen
		}

		public bool Taken
		{
			get => taken; // Getter za polje taken
			set
			{
				if (taken != value) // Provera da li vrednost polja taken treba da se promeni
				{
					taken = value; // Postavljanje nove vrednosti polja taken
					OnPropertyChanged("Taken"); // Obaveštavanje o promeni vrednosti polja taken
				}
			}
		}

		public Entitie Entitet
		{
			get => entitet; // Getter za polje entitet
			set
			{
				entitet = value; // Postavljanje nove vrednosti polja entitet
				OnPropertyChanged("Entitet"); // Obaveštavanje o promeni vrednosti polja entitet
				OnPropertyChanged("Foreground"); // Obaveštavanje o promeni vrednosti Foreground boje
				OnPropertyChanged("Text"); // Obaveštavanje o promeni vrednosti teksta
			}
		}

		public int X
		{
			get => x; // Getter za polje x
			set
			{
				x = value; // Postavljanje nove vrednosti polja x
				OnPropertyChanged("X"); // Obaveštavanje o promeni vrednosti polja x
			}
		}

		public int Y
		{
			get => y; // Getter za polje y
			set
			{
				y = value; // Postavljanje nove vrednosti polja y
				OnPropertyChanged("Y"); // Obaveštavanje o promeni vrednosti polja y
			}
		}

		public ObservableCollection<int> Lines
		{
			get => lines; // Getter za kolekciju lines
			set { Lines = value; OnPropertyChanged("Lines"); } // Postavljanje nove vrednosti kolekcije lines i obaveštavanje o promeni
		}
	}
}

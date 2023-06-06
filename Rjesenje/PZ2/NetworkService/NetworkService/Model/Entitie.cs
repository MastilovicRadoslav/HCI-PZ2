using NetworkService.Helpers; // Uključivanje potrebnog namespace-a
using NetworkService.ViewModel; // Uključivanje potrebnog namespace-a

namespace NetworkService.Model
{
	public class Entitie : ValidationBase
	{
		int id; // Privatno polje za ID entiteta
		string name; // Privatno polje za ime entiteta
		double valued; // Privatno polje za vrednost entiteta
		Type type; // Privatno polje za tip entiteta

		public Entitie()
		{
			// Konstruktor bez parametara
		}

		public Entitie(int id, string name, double valued, Type type)
		{
			this.Id = id; // Postavljanje vrednosti ID entiteta na prosleđenu vrednost
			this.Name = name; // Postavljanje vrednosti imena entiteta na prosleđenu vrednost
			this.Valued = valued; // Postavljanje vrednosti vrednosti entiteta na prosleđenu vrednost
			this.Type = type; // Postavljanje vrednosti tipa entiteta na prosleđenu vrednost
		}

		public Entitie(Entitie en)
		{
			this.Id = en.Id; // Kopiranje vrednosti ID entiteta iz drugog entiteta
			this.Name = en.Name; // Kopiranje vrednosti imena entiteta iz drugog entiteta
			this.Valued = en.Valued; // Kopiranje vrednosti vrednosti entiteta iz drugog entiteta
			this.Type = en.Type; // Kopiranje vrednosti tipa entiteta iz drugog entiteta
		}

		public int Id
		{
			get => id; // Getter za polje ID
			set
			{
				id = value; // Postavljanje nove vrednosti polja ID
				OnPropertyChanged("Id"); // Obaveštavanje o promeni vrednosti polja ID
			}
		}

		public string Name
		{
			get => name; // Getter za polje name
			set
			{
				name = value; // Postavljanje nove vrednosti polja name
				OnPropertyChanged("Name"); // Obaveštavanje o promeni vrednosti polja name
			}
		}

		public double Valued
		{
			get => valued; // Getter za polje valued
			set
			{
				if (valued != value) // Provera da li vrednost polja valued treba da se promeni
				{
					valued = value; // Postavljanje nove vrednosti polja valued
					OnPropertyChanged("Valued"); // Obaveštavanje o promeni vrednosti polja valued
					MeasurementGraphViewModel.ElementHeights.FirstBindingPoint = MeasurementGraphViewModel.CalculateElementHeight(value, Id); // Postavljanje visine prvog poveznog elementa u ViewModelu MeasurementGraphViewModel
				}
			}
		}

		public Type Type
		{
			get => type; // Getter za polje type
			set
			{
				type = value; // Postavljanje nove vrednosti polja type
				OnPropertyChanged("Type"); // Obaveštavanje o promeni vrednosti polja type
			}
		}

		protected override void ValidateSelf()
		{
			if (this.Id <= 0) // Provera da li ID entiteta ima ispravnu vrednost
			{
				this.ValidationErrors["Id"] = "ID must be more than 0 and must be a number"; // Postavljanje greške u validaciji za polje ID
			}
			else
			{
				foreach (Entitie entitet in ViewModel.NetworkEntitiesViewModel.Entiteti) // Prolazak kroz sve entitete u ViewModelu NetworkEntitiesViewModel
				{
					if (entitet.Id == this.Id)
						this.ValidationErrors["Id"] = "Can't have 2 same IDs"; // Postavljanje greške u validaciji za polje ID ako postoje dva entiteta sa istim ID-om
				}
			}

			if (string.IsNullOrWhiteSpace(this.Name)) // Provera da li ime entiteta ima ispravnu vrednost
			{
				this.ValidationErrors["Name"] = "Name is required"; // Postavljanje greške u validaciji za polje Name
			}

			if (type == null) // Provera da li tip entiteta ima ispravnu vrednost
			{
				this.ValidationErrors["Type"] = "Type is required"; // Postavljanje greške u validaciji za polje Type
			}
		}

		public override string ToString()
		{
			return Id + " " + Name + " " + Type.Name; // Prikazivanje ID-ja, imena i imena tipa entiteta kao string
		}
	}
}

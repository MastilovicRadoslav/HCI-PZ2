using NetworkService.Helpers; // Uključivanje potrebnog namespace-a

namespace NetworkService.Model
{
	public class GraphUpdater : BindableBase
	{
		private double firstBindingPoint; // Privatno polje za prvu tačku povezivanja
		private double secondBindingPoint; // Privatno polje za drugu tačku povezivanja
		private double thirdBindingPoint; // Privatno polje za treću tačku povezivanja
		private double fourthBindingPoint; // Privatno polje za četvrtu tačku povezivanja
		private double fifthBindingPoint; // Privatno polje za petu tačku povezivanja

		public double FirstBindingPoint
		{
			get { return firstBindingPoint; } // Getter za prvu tačku povezivanja
			set
			{
				SecondBindingPoint = firstBindingPoint; // Postavljanje vrednosti druge tačke povezivanja na prethodnu vrednost prve tačke povezivanja
				firstBindingPoint = value; // Postavljanje nove vrednosti prve tačke povezivanja
				OnPropertyChanged("FirstBindingPoint"); // Obaveštavanje o promeni vrednosti polja FirstBindingPoint
			}
		}

		public double SecondBindingPoint
		{
			get { return secondBindingPoint; } // Getter za drugu tačku povezivanja
			set
			{
				ThirdBindingPoint = secondBindingPoint; // Postavljanje vrednosti treće tačke povezivanja na prethodnu vrednost druge tačke povezivanja
				secondBindingPoint = value; // Postavljanje nove vrednosti druge tačke povezivanja
				OnPropertyChanged("SecondBindingPoint"); // Obaveštavanje o promeni vrednosti polja SecondBindingPoint
			}
		}

		public double ThirdBindingPoint
		{
			get { return thirdBindingPoint; } // Getter za treću tačku povezivanja
			set
			{
				FourthBindingPoint = thirdBindingPoint; // Postavljanje vrednosti četvrte tačke povezivanja na prethodnu vrednost treće tačke povezivanja
				thirdBindingPoint = value; // Postavljanje nove vrednosti treće tačke povezivanja
				OnPropertyChanged("ThirdBindingPoint"); // Obaveštavanje o promeni vrednosti polja ThirdBindingPoint
			}
		}

		public double FourthBindingPoint
		{
			get { return fourthBindingPoint; } // Getter za četvrtu tačku povezivanja
			set
			{
				FifthBindingPoint = fourthBindingPoint; // Postavljanje vrednosti pete tačke povezivanja na prethodnu vrednost četvrte tačke povezivanja
				fourthBindingPoint = value; // Postavljanje nove vrednosti četvrte tačke povezivanja
				OnPropertyChanged("FourthBindingPoint"); // Obaveštavanje o promeni vrednosti polja FourthBindingPoint
			}
		}

		public double FifthBindingPoint
		{
			get { return fifthBindingPoint; } // Getter za petu tačku povezivanja
			set
			{
				fifthBindingPoint = value; // Postavljanje nove vrednosti pete tačke povezivanja
				OnPropertyChanged("FifthBindingPoint"); // Obaveštavanje o promeni vrednosti polja FifthBindingPoint
			}
		}
	}
}

using NetworkService.Helpers; // Uključivanje potrebnog namespace-a

namespace NetworkService.Model
{
	public class Line : BindableBase
	{
		int x1, x2, y1, y2, id; // Privatna polja za koordinate linije i ID
		static int i; // Statičko polje koje čuva brojač ID-a

		public Line(int x1, int x2, int y1, int y2)
		{
			this.X1 = x1; // Postavljanje vrednosti X1
			this.X2 = x2; // Postavljanje vrednosti X2
			this.Y1 = y1; // Postavljanje vrednosti Y1
			this.Y2 = y2; // Postavljanje vrednosti Y2
			this.Id = i++; // Postavljanje vrednosti ID-a na vrednost brojača i inkrementiranje brojača
		}

		public int X1 { get => x1; set { x1 = value; OnPropertyChanged("X1"); } } // Svojstvo za X1 koje automatski obaveštava o promeni vrednosti
		public int X2 { get => x2; set { x2 = value; OnPropertyChanged("X2"); } } // Svojstvo za X2 koje automatski obaveštava o promeni vrednosti
		public int Y1 { get => y1; set { y1 = value; OnPropertyChanged("Y1"); } } // Svojstvo za Y1 koje automatski obaveštava o promeni vrednosti
		public int Y2 { get => y2; set { y2 = value; OnPropertyChanged("Y2"); } } // Svojstvo za Y2 koje automatski obaveštava o promeni vrednosti
		public int Id { get => id; set { id = value; OnPropertyChanged("Id"); } } // Svojstvo za ID koje automatski obaveštava o promeni vrednosti
	}
}

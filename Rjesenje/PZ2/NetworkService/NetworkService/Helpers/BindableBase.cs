using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NetworkService.Helpers
{
	public class BindableBase : INotifyPropertyChanged
	{
		// Metoda za postavljanje vrednosti svojstva
		protected virtual void SetProperty<T>(ref T member, T val,
		   [CallerMemberName] string propertyName = null)
		{
			// Provera da li nova vrednost odgovara trenutnoj vrednosti
			if (object.Equals(member, val)) return;

			// Postavljanje nove vrednosti
			member = val;

			// Pozivanje događaja PropertyChanged kako bi se obavestilo da je svojstvo promenjeno
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Metoda koja obaveštava da je svojstvo promenjeno
		protected virtual void OnPropertyChanged(string propertyName)
		{
			// Pozivanje događaja PropertyChanged kako bi se obavestilo da je svojstvo promenjeno
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Događaj koji se pokreće kada se svojstvo promeni
		public event PropertyChangedEventHandler PropertyChanged = delegate { };
	}
}

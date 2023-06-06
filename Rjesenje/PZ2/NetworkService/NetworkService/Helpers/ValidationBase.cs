namespace NetworkService.Helpers
{
	// Apstraktna klasa koja pruža osnovnu funkcionalnost za validaciju
	public abstract class ValidationBase : BindableBase
	{
		// Lista grešaka validacije
		public ValidationErrors ValidationErrors { get; set; }

		// Indikator da li je objekat validan
		public bool IsValid { get; private set; }

		// Konstruktor koji inicijalizuje listu grešaka validacije
		protected ValidationBase()
		{
			this.ValidationErrors = new ValidationErrors();
		}

		// Apstraktna metoda koja se mora implementirati u nasleđenim klasama
		protected abstract void ValidateSelf();

		// Metoda za pokretanje validacije
		public void Validate()
		{
			this.ValidationErrors.Clear();
			this.ValidateSelf();
			this.IsValid = this.ValidationErrors.IsValid;

			// Obaveštava promene svojstava IsValid i ValidationErrors
			this.OnPropertyChanged("IsValid");
			this.OnPropertyChanged("ValidationErrors");
		}
	}
}

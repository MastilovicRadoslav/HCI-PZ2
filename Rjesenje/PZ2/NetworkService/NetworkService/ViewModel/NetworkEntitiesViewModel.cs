using NetworkService.Helpers;
using NetworkService.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace NetworkService.ViewModel
{
	public class NetworkEntitiesViewModel : BindableBase
	{
		public static ObservableCollection<Entitie> Entiteti { get; set; }  //Lista entiteta
		public static ObservableCollection<Entitie> temp { get; set; }      //Pomocna lista koja ce mi trebat za Pretragu, da ispisem u tabeli sta je nadjeno a sta nije
		public static ICollectionView PrikazEntiteta { get; set; }          //Kolekcija za prikaz entiteta  u tabeli
		public static List<Model.Type> Tipovi { get; set; }                 //Tip entiteta	koji smo izabrali za dodavanje
		public MyICommand AddCommand { get; set; }                          //Komanda za dodavanje svega ovoga sto smo unijeli u NoviEntitet u tabelu
		public MyICommand DeleteCommand { get; set; }                       //Komanda za brisanje onoga kojeg smo selektovali u tabeli sa SelectedItem izabran

		public string Filter = "ip";                                        //Potrebno da bih razdvojio po cemu da pretrazujem, default je po ip

		private string search = "";                                         //Sta sam ukucao u pretrazi, tj ono sto pretrazujem

		public MyICommand SearchCommand { get; set; }                       //Komanda za pretragu, koja pretrazuje po onome sto je uneseno i po onome sto je izabrano

		public MyICommand<string> FilterCommand { get; set; }               //Komanda po cemu se pretrazuje, Type ili Name

		public MyICommand HelpCommand { get; set; }                         //Help komanda	 za prikaz pomoci


		bool toolTipsBool;                                                  //bool promjenljiva za ToolTip

		public bool ToolTipsBool                                            //Property za ToolTip, tj za ove skracenice
		{
			get => toolTipsBool;
			set
			{
				toolTipsBool = value;
				MainWindowViewModel.UseToolTips = value;					//prosledjujemo glavnom prozoru za izvrsavanje ToolTip - ova
				OnPropertyChanged("ToolTipsBool");
			}
		}

		Entitie noviEntitet = new Entitie();                                //Entitet za tabelu kad dodavam novi entitet u tabelu
		public Entitie NoviEntitet                                          //Property za Entitet
		{
			get => noviEntitet;
			set
			{
				noviEntitet = value;
				OnPropertyChanged("NoviEntitet");
			}
		}

		Entitie izabran;                                                   //Entitet za brisanje, onaj koji smo selektovali	da bi obrisali
		public Entitie Izabran                                             //Property
		{
			get => izabran;
			set
			{
				izabran = value;
				DeleteCommand.RaiseCanExecuteChanged();
			}
		}

		public string Search                                              //Property za pretragu  koji ce nam reci sta je korisnik unio za pretragu
		{
			get { return search; }
			set
			{
				search = value;
				OnPropertyChanged("Search");
				SearchCommand.RaiseCanExecuteChanged();
			}
		}

		public NetworkEntitiesViewModel()                                //Inicijalizacija  listi i komandi
		{
			//potrebna inicijalizacija
			if (Entiteti == null)
			{
				Entiteti = new ObservableCollection<Entitie>();
			}
			PrikazEntiteta = CollectionViewSource.GetDefaultView(Entiteti);	  //inizijalizacija ono sto ce se pojaviti u tabeli
			temp = new ObservableCollection<Entitie>();						 //treba mi za pretragu da bih ispisao u tabeli
			Tipovi = new List<Model.Type> { new Model.Type("iA"), new Model.Type("iB") };	 //inicijalizacija u ComboBox da se prikaze kad zelimo da dodajemo novi entitet pa da izaberemo
			//komande
			AddCommand = new MyICommand(OnAdd);						//inicijalizacija komande Add
			DeleteCommand = new MyICommand(OnDelete, CanDelete);	//inicijalizacije komande za brisanje
			SearchCommand = new MyICommand(OnSearch);				//Komanda za search
			FilterCommand = new MyICommand<string>(OnFilter);		//Komanda po cemu se filtrira
			HelpCommand = new MyICommand(OnHelp);				    //inicijalizacija Help comande
			//Dodjelivanje odmah na pocektu potrebnih vrijednosti
			NoviEntitet.Id = 1;		   //id inicijalizujem
			HelpText = saveHelp;
			ToolTipsBool = MainWindowViewModel.UseToolTips;
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
			if (HelpText == string.Empty)		 //ako je prazno dodaj na bijeli prostor u canvasu koje si rezervisao
			{
				HelpText = "U koliko vam je potrebna pomoć,prečice su sledeće:" + "\nCTRL+D -> Dodavanje entiteta u listu\nCTRL+Tab -> pomeranje " +
						   "između prozora\nCTRL+F -> Pretraga sa zadatim parametrima\nCTRL+H -> Help \n" +
						   "Za dodavanje novog entiteta potrebno je uneti jedinstveni id, naziv i izabrati " +
						   "kog je tipa entitet. Nakon toga pritisak na dugme \"Add\" ili gestikulacijom CTRL+D " +
						   "se doda u listu entiteta.\nOznačavanjem entiteta u listi i pritiskom na \"Delete\" se ukloni " +
						   "entitet iz liste.\n Nakon izabranih parametara pretragu je moguće pokrenuti na dugme \"Search\" ili " +
						   "gestikulacijom CTRL+F";
			}
			else		   //ako nije ocisti
			{
				HelpText = string.Empty;
			}
		}

		//Dodavanje
		private void OnAdd()
		{
			NoviEntitet.Validate();       //Validacija novog eniteta kojeg smo formirali na osnovu onih parametara za dodavanje 
			if (NoviEntitet.IsValid)      //Validacija	tog novog entiteta
			{
				if (ExistsID(NoviEntitet.Id))    //Da li vec postoji u tabeli entitet sa tim ID
				{
					NoviEntitet.ValidationErrors["Id"] = "ID exists in list";  //postoji vec sa istim Id u tabeli, pa validiram gresku koju sam u XML povezao kao ValidationErrors[id]
					return;
				}
				Entiteti.Add(new Entitie(NoviEntitet));    //Dodaj u glavnu listu koja ce se prikazati u tabeli
				temp.Add(new Entitie(NoviEntitet));        //Dodaj u pomocnu listu koju cu koristiti kad pretrazujem
				NetworkDisplayViewModel.EntitetList.Add(new Entitie(NoviEntitet));   //Prebacivanju u TreeView liste entiteta u tabeli, kako bih imao sta da prevucem

				NoviEntitet.Id++;     //Ako korisnik pritisne za dodavanje novog eniteta pomjerit cemo ID da ne ispisivalo stalno gresku, to jest ako je na istom prozoru id ce se sam povecavati kad dodavamo redom
			}
		}
		//Brisanje
		private void OnDelete()
		{
			NetworkDisplayViewModel.RemoveFromList(Izabran);   //Brisemo ga i sa drugog prozora jer ga nema vise u tabeli
			Entiteti.Remove(Izabran);                          //Brisemo i iz glavne liste za prikaz u tabeli
			temp.Remove(Izabran);                              //Brisemo ga i iz pomocne liste za pretragu
		}
		//Dozvola za brisanje
		private bool CanDelete()
		{
			return Izabran != null;
		}
		//Pretraga
		private void OnSearch()
		{
			if (Search.Trim() != "")  //Da li je uopste nesto ispisano
			{
				Entiteti.Clear();     //Skloni listu svih iz tabele
				foreach (Entitie e in temp)     //Prodji kroz pomocnu
				{
					switch (Filter)             //Po cemu korisnik trazi
					{
						case "ip":              //Po Type
							if (e.Type.Name.Contains(Search)) Entiteti.Add(e);    //iz pomocne ubaci u glavnu listu i ono ce se prikazati, podniz u nizu
							break;
						case "name":            //Po Name                  
							if (e.Name.Contains(Search)) Entiteti.Add(e);        //isto
							break;
					}
				}
			}
			else    //ako nista nije korisnik unio
			{
				Entiteti.Clear();        //ocisti listu
				foreach (Entitie e in temp) Entiteti.Add(e);    //dodaj one prije prikazane za pregled
			}
		}
		//Dozvola
		public void OnFilter(string t)     //povezao sam FilterCommand sa time sta je izabrano pa ce on ovo da postavi na izabranu vrijednost sto ce se dalje proslijediti po cemu se pretrazuje, string je to sto je izabrano po cemu se filtrira
		{
			Filter = t;	  //postavljam izabrano po cemu se filtrira
		}

		bool ExistsID(int id)     //Provjera za postojanje ID, tj da li u tabeli vec postoji entitet sa istim Id
		{
			foreach (Entitie e in Entiteti)
			{
				if (e.Id == id)
				{
					return true;
				}
			}
			return false;
		}
	}
}

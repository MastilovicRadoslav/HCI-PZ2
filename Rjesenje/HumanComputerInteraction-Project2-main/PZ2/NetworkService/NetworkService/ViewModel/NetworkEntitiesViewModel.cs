using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NetworkService.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        public static ObservableCollection<Entitie> Entiteti { get; set; }  //Lista entiteta
		public static ObservableCollection<Entitie> temp { get; set; }      //Pomocna lista koja ce mi trebat za Pretragu
		public static ICollectionView PrikazEntiteta { get; set; }          //Kolekcija za prikaz entiteta  u tabeli
        public static List<Model.Type> Tipovi { get; set; }                 //Tip entiteta
        public MyICommand AddCommand { get; set; }                          //Komanda za dodavanje
        public MyICommand DeleteCommand { get; set; }                       //Komanda za brisanje

		public string Filter = "ip";                                        //Potrebno da bih razdvojio po cemu da pretrazujem

		private string search = "";                                         //Sta sam ukucao u pretrazi

		public MyICommand SearchCommand { get; set; }                       //Komanda za pretragu

		public MyICommand<string> FilterCommand { get; set; }               //Komanda po cemu se pretrazuje

		public MyICommand HelpCommand { get; set; }                         //Help komanda


        bool toolTipsBool;                                                  //bool promjenljiva za ToolTip

        public bool ToolTipsBool                                            //Property za ToolTip
        {
            get => toolTipsBool;
            set
            {
                toolTipsBool = value;
                MainWindowViewModel.UseToolTips = value;
                OnPropertyChanged("ToolTipsBool");
            }
        }

        Entitie noviEntitet = new Entitie();                                //Entitet za tabelu kad dodavam s
        public Entitie NoviEntitet                                          //Property za Entitet
        {
            get => noviEntitet;
            set
            {
                noviEntitet = value;
                OnPropertyChanged("NoviEntitet");
            }
        }

        Entitie izabran;                                                   //Entitet za brisanje, onaj koji smo selektovali
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
            PrikazEntiteta = CollectionViewSource.GetDefaultView(Entiteti);
			temp = new ObservableCollection<Entitie>();
			Tipovi = new List<Model.Type> { new Model.Type("iA"), new Model.Type("iB") };
            //komande
            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
			SearchCommand = new MyICommand(OnSearch);
			FilterCommand = new MyICommand<string>(OnFilter);
			HelpCommand = new MyICommand(OnHelp);
            //Dodjelivanje odmah na pocektu potrebnih vrijednosti
            NoviEntitet.Id = 1;
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
           if (HelpText == string.Empty)
            {
                HelpText = "U koliko vam je potrebna pomoć,prečice su sledeće:" + "\nCTRL+D -> Dodavanje entiteta u listu\nCTRL+Tab -> pomeranje " +
                           "između prozora\nCTRL+F -> Pretraga sa zadatim parametrima\nCTRL+H -> Help \n" +
                           "Za dodavanje novog entiteta potrebno je uneti jedinstveni id, naziv i izabrati " +
                           "kog je tipa entitet. Nakon toga pritisak na dugme \"Add\" ili gestikulacijom CTRL+D " +
                           "se doda u listu entiteta.\nOznačavanjem entiteta u listi i pritiskom na \"Delete\" se ukloni " +
                           "entitet iz liste.\n Nakon izabranih parametara pretragu je moguće pokrenuti na dugme \"Search\" ili " +
                           "gestikulacijom CTRL+F";
            }
            else
            {
               HelpText = string.Empty;
            }
        }

        //Dodavanje
        private void OnAdd()
        {
            NoviEntitet.Validate();       //Validacija
            if (NoviEntitet.IsValid)      //Validacija
            {
                if (ExistsID(NoviEntitet.Id))    //Da li vec postoji u tabeli entitet sa tim ID
                {
                    NoviEntitet.ValidationErrors["Id"] = "ID exists in list";
                    return;
                }
                Entiteti.Add(new Entitie(NoviEntitet));    //Dodaj u tabelu
                temp.Add(new Entitie(NoviEntitet));        //Za pretragu
                NetworkDisplayViewModel.EntitetList.Add(new Entitie(NoviEntitet));   //Prebacivanju u TreeView liste entiteta u tabeli
                
                NoviEntitet.Id++;     //Ako korisnik pritisne za dodavanje novog eniteta pomjerit cemo IDda ne ispisivalo stalno gresku
            }
        }
        //Brisanje
		private void OnDelete()
		{
			NetworkDisplayViewModel.RemoveFromList(Izabran);   //Brisemo ga i sa drugog prozora jer ga nema vise u tabeli
			Entiteti.Remove(Izabran);                          //Brisemo i iz tabele
            temp.Remove(Izabran);                              //Brisemo ga i u listi za pretrazivanje
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
		public void OnFilter(string t)     //povezao sam FilterCommand sa time sta je izabrano pa ce on ovo da postavi na izabranu vrijednost sto ce se dalje proslijediti po cemu se pretrazuje
		{
			Filter = t;
		}

		bool ExistsID(int id)     //Provjera za postojanje ID
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

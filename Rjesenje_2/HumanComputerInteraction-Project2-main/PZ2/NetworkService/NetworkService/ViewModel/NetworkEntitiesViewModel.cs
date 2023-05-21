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
        public static ObservableCollection<Entitie> Entiteti { get; set; }

		public static ObservableCollection<Entitie> temp { get; set; }

		public static ObservableCollection<Entitie> temp2 = new ObservableCollection<Entitie>();

		public static ICollectionView PrikazEntiteta { get; set; }
        public static List<Model.Type> Tipovi { get; set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }

		public string Filter = "ip";

		private string search = "";

		public MyICommand SearchCommand { get; set; }

		public MyICommand<string> FilterCommand { get; set; }

		public MyICommand HelpCommand { get; set; }


        bool toolTipsBool;

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

        Entitie noviEntitet = new Entitie();
        public Entitie NoviEntitet
        {
            get => noviEntitet;
            set
            {
                noviEntitet = value;
                OnPropertyChanged("NoviEntitet");
            }
        }

        Entitie izabran;
        public Entitie Izabran
        {
            get => izabran;
            set
            {
                izabran = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

		public string Search
		{
			get { return search; }
			set
			{
				search = value;
				OnPropertyChanged("Search");
			}
		}
		
        public NetworkEntitiesViewModel()
        {
            if (Entiteti == null)
                Entiteti = new ObservableCollection<Entitie>();
            PrikazEntiteta = CollectionViewSource.GetDefaultView(Entiteti);
			temp = new ObservableCollection<Entitie>();
			Tipovi = new List<Model.Type> { new Model.Type("iA"), new Model.Type("iB") };

            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
			SearchCommand = new MyICommand(OnSearch);
			FilterCommand = new MyICommand<string>(OnFilter);
			HelpCommand = new MyICommand(OnHelp);
            NoviEntitet.Id = 1;
            HelpText = saveHelp;
            ToolTipsBool = MainWindowViewModel.UseToolTips;
        }

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

        private void OnHelp()
        {
           if (HelpText == string.Empty)
            {
                HelpText = "U koliko vam je potrebna pomoć,prečice su sledeće:" + "\nCTRL+D -> Dodavanje entiteta u listu\nCTRL+TAB -> pomeranje" +
                   " između prozora\nCTRL+F -> Pretraga sa zadatim parametrima\nCTRL+H -> Help \n" +
                    "Za dodavanje novog entiteta potrebno je uneti jedinstveni id, naziv i izabrati " +
                  "kog je tipa entitet. Nakon toga pritisak na dugme \"Add\" ili gestikulacijom CTRL+D " +
                    "se doda u listu entiteta.\nOznačavanjem entiteta u listi i pritiskom na \"Delete\" se ukloni " +
                   "entitet iz liste.\nFiltracija je moguća na 3 načina: samo po tipu, samo po id-u i po tipu i id-u.\n" +
                   "Nakon izabranih parametara filtraciju je moguće pokrenuti na dugme \"Filter\" ili " +
                   "gestikulacijom CTRL+F";
            }
            else
            {
               HelpText = string.Empty;
            }
        }

        private void OnAdd()
        {
            NoviEntitet.Validate();
            if (NoviEntitet.IsValid)
            {
                if (ExistsID(NoviEntitet.Id))
                {
                    NoviEntitet.ValidationErrors["Id"] = "ID exists in list";
                    return;
                }
                Entiteti.Add(new Entitie(NoviEntitet));
                temp.Add(new Entitie(NoviEntitet));
                NetworkDisplayViewModel.EntitetList.Add(new Entitie(NoviEntitet));
                
                NoviEntitet.Id++;
            }
        }

		private void OnDelete()
		{
			NetworkDisplayViewModel.RemoveFromList(Izabran);
			Entiteti.Remove(Izabran);
            temp.Remove(Izabran);
		}

		private bool CanDelete()
		{
			return Izabran != null;
		}

		private void OnSearch()
		{
			if (Search.Trim() != "")
			{
				Entiteti.Clear();
				foreach (Entitie e in temp)
				{
					switch (Filter)
					{
						case "ip":
							if (e.Type.Name == Search) Entiteti.Add(e);
							break;
						case "name":
							if (e.Name == Search) Entiteti.Add(e);
							break;
					}
				}
			}
			else
			{
				Entiteti.Clear();
				foreach (Entitie e in temp) Entiteti.Add(e);
			}
		}
		public void OnFilter(string t)
		{
			Filter = t;
		}

		bool ExistsID(int id)
        {
            foreach (Entitie e in Entiteti)
                if (e.Id == id)
                    return true;
            return false;
        }
    }
}

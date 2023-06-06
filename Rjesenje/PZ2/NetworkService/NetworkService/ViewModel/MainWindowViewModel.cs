using NetworkService.Helpers;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace NetworkService.ViewModel
{

	public class MainWindowViewModel : BindableBase
	{
		//Prozori i manipulacija
		public MyICommand<string> NavCommand { get; private set; }
		public MyICommand ChangeCommand { get; set; }
		public MyICommand<Window> CloseWindowCommand { get; private set; }

		NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();
		MeasurementGraphViewModel measurementGraphViewModel = new MeasurementGraphViewModel();
		NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();


		BindableBase currentViewModel;

		public BindableBase CurrentViewModel
		{
			get => currentViewModel;
			set
			{
				SetProperty(ref currentViewModel, value);

			}
		}

		public static bool UseToolTips { get; set; } = true;
		public MainWindowViewModel()
		{
			createListener(); //Povezivanje sa serverskom aplikacijom
			NavCommand = new MyICommand<string>(OnNav);
			ChangeCommand = new MyICommand(Change);
			CloseWindowCommand = new MyICommand<Window>(CloseWindow);
			CurrentViewModel = networkEntitiesViewModel;
		}
		private void Change()
		{
			if (CurrentViewModel == networkDisplayViewModel)
				CurrentViewModel = measurementGraphViewModel;
			else if (CurrentViewModel == measurementGraphViewModel)
				CurrentViewModel = networkEntitiesViewModel;
			else if (CurrentViewModel == networkEntitiesViewModel)
				CurrentViewModel = networkDisplayViewModel;
		}

		private void OnNav(string dest)
		{
			switch (dest)
			{
				case "NetEnt":
					CurrentViewModel = networkEntitiesViewModel;
					break;
				case "NetDis":
					CurrentViewModel = networkDisplayViewModel;
					break;
				case "MesGraph":
					CurrentViewModel = measurementGraphViewModel;
					break;
			}
		}

		private void createListener()
		{
			var tcp = new TcpListener(IPAddress.Any, 25565); //kreira se objekat TCP koji slusa na odredjenom portu
			tcp.Start();     //pokrece se proces slusanja

			var listeningThread = new Thread(() =>   //glavna petlja slusanja
			{
				while (true)
				{
					var tcpClient = tcp.AcceptTcpClient(); //blokira izvršavanje programa dok ne bude primljen novi TCP klijentski zahtev
					ThreadPool.QueueUserWorkItem(param =>  // za postavljanje zadatka za obradu dolazne poruke u pozadinsku nit iz niti bazena.
					{
						//Prijem poruke
						NetworkStream stream = tcpClient.GetStream();  //U okviru te niti, prvo se prima poruka od klijenta putem
						string incomming;  //cita u niz bajtova
						byte[] bytes = new byte[1024];    //pretvara u ASCII 
						int i = stream.Read(bytes, 0, bytes.Length);
						//Primljena poruka je sacuvana u incomming stringu
						incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

						//Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
						if (incomming.Equals("Need object count"))
						{
							//Response
							/* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
							Byte[] data = System.Text.Encoding.ASCII.GetBytes(NetworkEntitiesViewModel.Entiteti.Count.ToString());
							stream.Write(data, 0, data.Length);  //Broj objekata se pretvara u niz bajtova (data) i šalje se klijentu putem stream.Write
						}
						else
						{
							//U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
							Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

							//################ IMPLEMENTACIJA ####################
							// Obraditi poruku kako bi se dobile informacije o izmeni
							// Azuriranje potrebnih stvari u aplikaciji
							//Nakon toga sledi deo koda koji implementira logiku za ažuriranje podataka u aplikaciji na osnovu primljene poruke
							if (NetworkEntitiesViewModel.Entiteti.Count > 0)    //Ako u NetworkEntitiesViewModel.Entiteti postoji barem jedan entitet, poruka se parsira kako bi se dobile informacije o izmeni.
							{
								var splited = incomming.Split(':'); //Primljena poruka se deli na dva dela koristeći separator :. Očekuje se da poruka ima format "Entitet_X:Vrednost", gde je X identifikator entiteta, a Vrednost nova vrednost koju treba ažurirati.
								DateTime dt = DateTime.Now; //Dobija se trenutno vreme kako bi se zabeležilo vreme primanja poruke.
								using (StreamWriter sw = File.AppendText("Log.txt")) // Vreme primanja poruke se zapisuje u Log.txt fajl.
								{   //Kreira se StreamWriter objekat koji će se koristiti za pisanje u fajl "Log.txt". Opcija File.AppendText otvara fajl za pisanje i pozicionira se na kraj fajla kako bi dodao nove linije bez brisanja postojećeg sadržaja.
									sw.WriteLine(dt + ": " + splited[0] + ", " + splited[1]);//sw.WriteLine(dt + ": " + splited[0] + ", " + splited[1]);: Upisuje se linija u fajl "Log.txt" koja sadrži trenutno vreme, identifikator entiteta (splited[0]) i novu vrednost (splited[1]). Ova linija predstavlja zapis o primljenoj poruci i njenom vremenu prijema.
									sw.Close();
								}

								int id = Int32.Parse(splited[0].Split('_')[1]);   //Id objekta se dobija iz poruke
								NetworkEntitiesViewModel.Entiteti[id].Valued = Double.Parse(splited[1]);     //ovdje nam stizu nove vrijednosti koje azuriramo u tabeli
								NetworkDisplayViewModel.UpdateList(NetworkEntitiesViewModel.Entiteti[id]);   //metoda za ažuriranje prikaza entiteta u aplikaciji.
							}

						}
					}, null);
				}
			});

			listeningThread.IsBackground = true;
			listeningThread.Start();
		}

		private void CloseWindow(Window MainWindow)
		{
			MainWindow.Close();
		}
	}
}

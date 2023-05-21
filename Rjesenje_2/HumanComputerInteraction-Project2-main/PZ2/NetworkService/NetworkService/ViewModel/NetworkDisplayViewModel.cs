using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NetworkService.Views;
using System.Windows.Input;
using NetworkService.Helpers;

namespace NetworkService.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        public static void RemoveFromList(Entitie e)
        {
            foreach (Entitie entitet in EntitetList)
                if (entitet.Id == e.Id)
                {
                    EntitetList.Remove(entitet);
                    return;
                }

            for (int i = 0; i < 12; i++)
                if (Canvases[i].Entitet.Id == e.Id)
                {
                    foreach (int id in Canvases[i].Lines)
                        RemoveLine(id);
                    Canvases[i] = new CanvasInfo(i);
                    return;
                }
        }

        public static void UpdateList(Entitie e)
        {
            for (int i = 0; i < EntitetList.Count; i++)
                if (EntitetList[i].Id == e.Id)
                {
                    EntitetList[i].Valued = e.Valued;
                    return;
                }

            for (int i = 0; i < 12; i++)
                if (Canvases[i].Entitet.Id == e.Id)
                {
                    Canvases[i].Entitet = e;
                    return;
                }
        }
        public static ObservableCollection<Entitie> EntitetList { get; set; }
        public static ObservableCollection<CanvasInfo> Canvases { get; set; }
        public static ObservableCollection<Line> Lines { get; set; }
        public MyICommand<ListView> SelectionChangedCommand { get; set; }
        public MyICommand MouseLeftButtonUpCommand { get; set; }
        public MyICommand<Canvas> ButtonCommand { get; set; }
        public MyICommand<Canvas> DragOverCommand { get; set; }
        public MyICommand<Canvas> DropCommand { get; set; }
        public MyICommand<Canvas> MouseLeftButtonDownCommand { get; set; }
        public MyICommand AutoPlaceCommand { get; set; }
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
            }
        }
        bool dragging = false;
        Entitie selectedEntitet;
        public Entitie SelectedEntitet
        {
            get => selectedEntitet;
            set
            {
                selectedEntitet = value;
                OnPropertyChanged("SelectedEntitet");
            }

        }

        CanvasInfo currentCanvas;
        public CanvasInfo CurrentCanvas
        {
            get => currentCanvas;
            set
            {
                currentCanvas = value;
                OnPropertyChanged("CurrentCanvas");
            }
        }



        bool Cmp(CanvasInfo c)
        {
            return CurrentCanvas.Entitet == c.Entitet && CurrentCanvas.Taken == c.Taken && CurrentCanvas.Text == c.Text;
        }

        private void OnAutoPlace()
        {
            List<Entitie> temp = new List<Entitie>();
            foreach (Entitie e in EntitetList)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (!Canvases[i].Taken)
                    {
                        Canvases[i] = new CanvasInfo(e, true, i);
                        temp.Add(e);
                        break;
                    }
                }
            }

            foreach (Entitie e in temp)
                EntitetList.Remove(e);
        }

        public NetworkDisplayViewModel()
        {
            if (EntitetList == null)
                EntitetList = new ObservableCollection<Entitie>();
            if (Canvases == null)
            {
                Canvases = new ObservableCollection<CanvasInfo>();
                for (int i = 0; i < 12; i++)
                    Canvases.Add(new CanvasInfo(i));
            }
            if (Lines == null)
                Lines = new ObservableCollection<Line>();

            DragOverCommand = new MyICommand<Canvas>(DragOver);
            DropCommand = new MyICommand<Canvas>(Drop);
            ButtonCommand = new MyICommand<Canvas>(ButtonCommandFreeing);
            SelectionChangedCommand = new MyICommand<ListView>(SelectionChanged);
            MouseLeftButtonUpCommand = new MyICommand(MouseLeftButtonUp);
            MouseLeftButtonDownCommand = new MyICommand<Canvas>(MouseLeftButtonDown);
            AutoPlaceCommand = new MyICommand(OnAutoPlace);
            HelpCommand = new MyICommand(OnHelp);
            helpText = saveHelp;
            ToolTipsBool = MainWindowViewModel.UseToolTips;
        }

        private void OnHelp()
        {
            if (HelpText == "")
            {
                HelpText = "Prečice su sledeće:\nCTRL+D -> Automatsko stavljanje entiteta na mesta\nCtrl+H -> Help\n" +
                    "Prevlačenjem entiteta iz liste u odabrano polje će rezultirati prebacivanjem entiteta iz liste" +
                    " u to polje za prikaz trenutnog stanja tog entiteta.Prevlačenjem entiteta iz polja" +
                    " u polje ce rezultirati prebacivanjem entiteta iz polja u polje.\nPovlačenje linije" +
                    " izmedju 2 entiteta se radi povlačenjem prvog zauzetog polja na drugo polje.";
            }
            else
            {
                HelpText = "";
            }
        }

        void ChangeLine(int id, int x, int y, int nx, int ny)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].Id == id)
                {
                    if (Lines[i].X1 == x && Lines[i].Y1 == y)
                    {
                        Lines[i].X1 = nx;
                        Lines[i].Y1 = ny;
                    }
                    else
                    {
                        Lines[i].X2 = nx;
                        Lines[i].Y2 = ny;
                    }
                    return;
                }
            }
        }

        private void Drop(Canvas obj)
        {
            if (SelectedEntitet != null)
            {
                int id = int.Parse(obj.Name.Substring(1));
                if (!Canvases[id].Taken)
                {
                    Canvases[id] = new CanvasInfo(SelectedEntitet, true, id);
                    EntitetList.Remove(SelectedEntitet);
                }
            }
            else if (CurrentCanvas != null)
            {
                int id = int.Parse(obj.Name.Substring(1));
                if (!Canvases[id].Taken)
                {
                    for (int i = 0; i < 12; i++)
                        if (Cmp(Canvases[i]))
                        {
                            Canvases[i] = new CanvasInfo(i);
                            break;
                        }
                    Canvases[id] = new CanvasInfo(CurrentCanvas.Entitet, true, id);
                    foreach (int i in CurrentCanvas.Lines)
                    {
                        ChangeLine(i, CurrentCanvas.X, CurrentCanvas.Y, Canvases[id].X, Canvases[id].Y);
                        Canvases[id].Lines.Add(i);
                    }
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                        if (Cmp(Canvases[i]))
                        {
                            Line line = new Line(Canvases[i].X, Canvases[id].X, Canvases[i].Y, Canvases[id].Y);
                            Lines.Add(line);
                            Canvases[i].Lines.Add(line.Id);
                            Canvases[id].Lines.Add(line.Id);
                            break;
                        }
                }
            }
            MouseLeftButtonUp();
        }

        private void DragOver(Canvas obj)
        {
            int id = int.Parse(obj.Name.Substring(1));
            if (!Canvases[id].Taken)
                obj.AllowDrop = true;
            else
                obj.AllowDrop = false;
        }

        private void MouseLeftButtonUp()
        {
            SelectedEntitet = null;
            CurrentCanvas = null;
            dragging = false;
        }


        private void MouseLeftButtonDown(Canvas c)
        {
            int id = int.Parse(c.Name.Substring(1));
            if (Canvases[id].Taken)
            {
                CurrentCanvas = Canvases[id];
                if (!dragging)
                {
                    dragging = true;
                    DragDrop.DoDragDrop(c, CurrentCanvas, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        static void RemoveLine(int id)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].Id == id)
                {
                    Lines.RemoveAt(i);
                    return;
                }
            }
        }
        private void ButtonCommandFreeing(Canvas obj)
        {
            int id = int.Parse(obj.Name.Substring(1));
            if (Canvases[id].Taken)
            {
                foreach (int i in Canvases[id].Lines)
                    RemoveLine(i);
                EntitetList.Add(Canvases[id].Entitet);
                Canvases[id] = new CanvasInfo(id);
            }
        }

        private void SelectionChanged(ListView obj)
        {
            if (!dragging)
            {
                dragging = true;
                DragDrop.DoDragDrop(obj, SelectedEntitet, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }
    }
}

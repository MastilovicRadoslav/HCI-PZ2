using NetworkService.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NetworkService.Model
{
    public class CanvasInfo : BindableBase
    {
        Entitie entitet;
        bool taken;
        int x, y;
        ObservableCollection<int> lines;
        public CanvasInfo(int ind)
        {
            Taken = false;
            Entitet = new Entitie();
            X = 10 + (ind % 4 + 1) * 100 + (ind % 4) * 160;
            Y = 85 + (ind / 4) * 200;
            lines = new ObservableCollection<int>();
        }

        public CanvasInfo(Entitie entitet, bool taken, int ind)
        {
            this.Entitet = entitet;
            this.Taken = taken;
            X = 10 + (ind % 4 + 1) * 100 + (ind % 4) * 160;
            Y = 85 + (ind / 4) * 200;
            lines = new ObservableCollection<int>();
        }

        public Brush Background
        {
            get
            {
                if (Entitet.Type != null)
                {
                    BitmapImage slika = new BitmapImage();
                    slika.BeginInit();
                    slika.UriSource = new Uri(Environment.CurrentDirectory + "../../../" + Entitet.Type.Img_src);
                    slika.EndInit();
                    return new ImageBrush(slika);
                }
                else
                    return Brushes.GhostWhite;
            }
        }
        public string Text { get => Entitet.Name != null ? "ID: " + Entitet.Id + " Name: " + Entitet.Name : ""; }
        public string Foreground { get => Uslov() ? "Blue" : "Red"; }
        public bool Uslov()
        {
            //if ((Entitet.Type.Name.Equals("iA") && Entitet.Valued > 15000) || (Entitet.Type.Name.Equals("iB") && Entitet.Valued > 7000))
            //    return false;
          
        
           return true;
        
        }
        public bool Taken
        {
            get => taken;
            set
            {
                if (taken != value)
                {
                    taken = value;
                    OnPropertyChanged("Taken");
                }
            } 
        }
        public Entitie Entitet
        {
            get => entitet;
            set
            {
                entitet = value;
                OnPropertyChanged("Entitet");
                OnPropertyChanged("Foreground");
                OnPropertyChanged("Text");
            }
        }

        public int X
        {
            get => x;
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public int Y
        {
            get => y;
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }

        public ObservableCollection<int> Lines
        {
            get => lines;
            set { Lines = value; OnPropertyChanged("Lines"); }
        }
    }
}

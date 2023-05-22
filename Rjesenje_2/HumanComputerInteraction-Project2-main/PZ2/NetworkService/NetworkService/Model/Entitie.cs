using NetworkService.Helpers;
using NetworkService.ViewModel;
using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Entitie : ValidationBase
    {
        int id;
        string name;
        double valued;
        Type type;

        public Entitie()
        {

        }
        public Entitie(int id, string name, double valued, Type type)
        {
            this.Id = id;
            this.Name = name;
            this.Valued = valued;
            this.Type = type;
        }

        public Entitie(Entitie en)
        {
            this.Id = en.Id;
            this.Name = en.Name;
            this.Valued = en.Valued;
            this.Type = en.Type;
        }

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public double Valued
        {
            get => valued;
            set
            {
                if (valued != value)
                {
                    valued = value;
                    OnPropertyChanged("Valued");
					MeasurementGraphViewModel.ElementHeights.FirstBindingPoint = MeasurementGraphViewModel.CalculateElementHeight(value, Id);

				}
			}
        }
        public Type Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        protected override void ValidateSelf()
        {
            if (this.Id <= 0)
            {
                this.ValidationErrors["Id"] = "ID must be more then 0 and must be a number";
            }
            else
            {
                foreach (Entitie entitet in ViewModel.NetworkEntitiesViewModel.Entiteti)
                {
                    if (entitet.Id == this.Id)
                        this.ValidationErrors["Id"] = "Can't have 2 same ID's";
                }
            }

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ValidationErrors["Name"] = "Name is required";
            }

            if (type == null)
            {
                this.ValidationErrors["Type"] = "Type is required";
            }
        }

        public override string ToString()
        {
            return Id + " " + Name + " " + Type.Name;
        }

       
    }
}

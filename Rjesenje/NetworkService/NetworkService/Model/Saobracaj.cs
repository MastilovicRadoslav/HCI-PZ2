using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
	public class Saobracaj : ValidationBase
	{
		int id;
		string name;
		double valued;
		TypeSaobracaj type;



		public Saobracaj()
		{
		}

		public Saobracaj(int id, string name, double valued, TypeSaobracaj type)
		{
			this.Id = id;
			this.Name = name;
			this.Valued = valued;
			this.Type = type;
		}

		public Saobracaj(Saobracaj s)
		{
			this.Id = s.Id;
			this.Name = s.Name;
			this.Valued = s.Valued;
			this.Type = s.Type;
		}

		public int Id { get => id;
			set
			{ 
				id = value;
				OnPropertyChanged("Id");
			}
		}

		public string Name { get => name;
			set
			{
				name = value;
				OnPropertyChanged("Name");
			}
		}
		public double Valued { get => valued; 
			set
			{
				valued = value;
				OnPropertyChanged("Valued");
			}
		
		}
		public TypeSaobracaj Type { get => type;
			set
			{
				type = value;
				OnPropertyChanged("Type");
			} 
		
		}


		protected override void ValidateSelf()
		{
			if(this.Id <= 0)
			{
				this.ValidationErrors["Id"] = "ID must be more then 0 and must be a number";
			}
			else
			{
				//foreach(Saobracaj saobracaj in ViewModel.NetworkEntitiesViewModel.Saobracaj)
				//{
				//	if(saobracaj.Id == this.Id)
				//	{
				//		this.ValidationErrors["Id"] = "Can't have 2 same ID's";
				//	}
				//}
			}

			if (string.IsNullOrWhiteSpace(this.Name))
			{
				this.ValidationErrors["Name"] = "Name is required";
			}

			if(type == null)
			{
				this.ValidationErrors["Type"] = "Ty[e is required";
			}
		}

		public override string ToString()
		{
			return Id + " " + Name + " " + Type.Name;
		}
	}
}

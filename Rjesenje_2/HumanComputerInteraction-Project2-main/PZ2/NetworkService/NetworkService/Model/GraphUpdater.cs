using NetworkService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
	public class GraphUpdater : BindableBase
	{
		private double firstBindingPoint;
		private double secondBindingPoint;
		private double thirdBindingPoint;
		private double fourthBindingPoint;
		private double fifthBindingPoint;

		public double FirstBindingPoint
		{
			get { return firstBindingPoint; }
			set
			{
				SecondBindingPoint = firstBindingPoint;
				firstBindingPoint = value;
				OnPropertyChanged("FirstBindingPoint");
			}
		}

		public double SecondBindingPoint
		{
			get { return secondBindingPoint; }
			set
			{
				ThirdBindingPoint = secondBindingPoint;
				secondBindingPoint = value;
				OnPropertyChanged("SecondBindingPoint");
			}
		}

		public double ThirdBindingPoint
		{
			get { return thirdBindingPoint; }
			set
			{
				FourthBindingPoint = thirdBindingPoint;
				thirdBindingPoint = value;
				OnPropertyChanged("ThirdBindingPoint");
			}
		}

		public double FourthBindingPoint
		{
			get { return fourthBindingPoint; }
			set
			{
				FifthBindingPoint = fourthBindingPoint;
				fourthBindingPoint = value;
				OnPropertyChanged("FourthBindingPoint");
			}
		}

		public double FifthBindingPoint
		{
			get { return fifthBindingPoint; }
			set
			{
				fifthBindingPoint = value;
				OnPropertyChanged("FifthBindingPoint");
			}
		}

		// Sluzbenici
		private double firstBindingPointSluzbenici;
		private double secondBindingPointSluzbenici;

		public double FirstBindingPointSluzbenici
		{
			get { return firstBindingPointSluzbenici; }
			set
			{
				firstBindingPointSluzbenici = value;
				OnPropertyChanged("FirstBindingPointSluzbenici");
			}
		}

		public double SecondBindingPointSluzbenici
		{
			get { return secondBindingPointSluzbenici; }
			set
			{
				secondBindingPointSluzbenici = value;
				OnPropertyChanged("SecondBindingPointSluzbenici");
			}
		}
	}
}

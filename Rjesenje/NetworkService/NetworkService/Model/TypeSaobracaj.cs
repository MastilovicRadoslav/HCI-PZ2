using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
	public class TypeSaobracaj
	{
		string name;
		string img_src;

		public TypeSaobracaj(string type)
		{
			name = type;
			img_src = (type == "iA") ? "Resources/Images/iA.jpg" :
				"Resources/Images/iB.jpg";
		}

		public string Name { get => name; set => name = value; }
		public string Img_src { get => img_src; set => img_src = value; }
	}
}

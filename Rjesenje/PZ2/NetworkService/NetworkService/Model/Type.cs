namespace NetworkService.Model
{
	public class Type
	{
		string name; // Privatno polje za ime tipa
		string img_src; // Privatno polje za putanju do slike

		public string Name { get => name; set => name = value; } // Svojstvo za ime tipa koje omogućava pristup i postavljanje vrednosti
		public string Img_src { get => img_src; set => img_src = value; } // Svojstvo za putanju do slike koje omogućava pristup i postavljanje vrednosti

		public Type(string type)
		{
			Name = type; // Postavljanje vrednosti imena tipa na vrednost prosleđenu konstruktoru
			Img_src = (type == "iA") ? "/Resources/Images/iA.jpg" : "/Resources/Images/iB.jpg"; // Postavljanje vrednosti putanje do slike na osnovu vrednosti tipa
		}

		public override bool Equals(object obj)
		{
			string s = obj as string; // Pretvaranje objekta u string
			return s == Name; // Provera jednakosti stringova
		}
		public override int GetHashCode()
		{
			return base.GetHashCode(); // Povrat osnovnog hash koda objekta
		}
	}
}

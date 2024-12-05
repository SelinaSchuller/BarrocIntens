using BarrocIntens.Data;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Onderhoud
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OnderhoudMainPage : Page
	{
		public List<OnderhoudItem> LaatsteOnderhoudLijst { get; set; }
		//public List<ServiceRequests> StoringenLijst { get; set; }

		public OnderhoudMainPage()
		{
			this.InitializeComponent();

			//CreateHardcodeLists();
			//this.DataContext = this;
		}

		public void CreateHardcodeLists()
		{
			// Hardcoded data voor "Laatste onderhoud"
			LaatsteOnderhoudLijst = new List<OnderhoudItem>
			{
				new OnderhoudItem { KlantNaam = "Jan van Dijk", Date = new DateTime(2024, 6, 1) },
				new OnderhoudItem { KlantNaam = "Pieter de Jong", Date = new DateTime(2024, 3, 25) },
				new OnderhoudItem { KlantNaam = "Klaas Bakker", Date = new DateTime(2024, 3, 12) },
				new OnderhoudItem { KlantNaam = "Maria Jansen", Date = new DateTime(2024, 1, 1) }
			};

			// Hardcoded data voor "Storingen"
			//StoringenLijst = new List<StoringItem>
			//{
			//	new StoringItem { KlantNaam = "Jan van Dijk", Status = 0, Date = new DateTime(2024, 10, 8) },
			//	new StoringItem { KlantNaam = "Pieter de Jong", Status = 0, Date = DateTime.Today.AddDays(-1) },
			//	new StoringItem { KlantNaam = "Klaas Bakker", Status = 0, Date = DateTime.Today }
			//};
		}

		//Deze classes zijn temp tot dat de database klaar is:
		public class OnderhoudItem
		{
			public string KlantNaam { get; set; }
			public DateTime Date { get; set; }

			public string DateFormatted => Date.ToString("dd/MM/yyyy");
		}

		public class StoringItem
		{
			public string KlantNaam { get; set; }
			public int Status { get; set; }
			public DateTime Date { get; set; }

			public string DateFormatted => Date.ToString("dd/MM/yyyy");
		}


	}
}

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
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

namespace BarrocIntens.Financiën
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class FinanciënMainPage : Page
	{
		public List<Contract> ContractLijst { get; set; }

		public FinanciënMainPage()
		{
			this.InitializeComponent();

			// Temporary list for contracts
			ContractLijst = new List<Contract>
			{
				new Contract { KlantNaam = "Jan van Dijk", StartDatum = new DateTime(2024, 6, 1), EindDatum = new DateTime(2025, 6, 1) },
				new Contract { KlantNaam = "Pieter de Jong", StartDatum = new DateTime(2024, 3, 25), EindDatum = new DateTime(2025, 3, 25) },
				new Contract { KlantNaam = "Klaas Bakker", StartDatum = new DateTime(2024, 3, 12), EindDatum = new DateTime(2025, 3, 12) },
				new Contract { KlantNaam = "Maria Jansen", StartDatum = new DateTime(2024, 1, 1), EindDatum = new DateTime(2025, 1, 1) }
			};

			// Set DataContext for data binding
			this.DataContext = this;
		}

		//Deze class is temp tot dat de database klaar is:
		public class Contract
		{
			public string KlantNaam { get; set; }
			public DateTime StartDatum { get; set; }
			public DateTime EindDatum { get; set; }

			public string StartDatumFormatted => StartDatum.ToString("dd/MM/yyyy");
			public string EindDatumFormatted => EindDatum.ToString("dd/MM/yyyy");
		}

	}

}
using Microsoft.UI;
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
using Windows.Graphics;
using Microsoft.UI.Windowing;
using static BarrocIntens.Onderhoud.OnderhoudMainPage;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BarrocIntens.Onderhoud
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OnderhoudIngekomenStoringen : Window
	{
		public List<OnderhoudMainPage.StoringItem> StoringenLijst { get; set; }

		public OnderhoudIngekomenStoringen()
		{
			this.InitializeComponent();

			this.Title = $"Storingen";

			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
			var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
			var appWindow = AppWindow.GetFromWindowId(windowId);

			appWindow.Resize(new SizeInt32(500, 800));

			CreateHardcodeList();
		}

		public void CreateHardcodeList()
		{

			// Hardcoded data voor "Storingen"
			StoringenLijst = new List<StoringItem>
			{
				new StoringItem { KlantNaam = "Jan van Dijk", Status = 0, Date = new DateTime(2024, 10, 8) },
				new StoringItem { KlantNaam = "Pieter de Jong", Status = 0, Date = DateTime.Today.AddDays(-1) },
				new StoringItem { KlantNaam = "Klaas Bakker", Status = 0, Date = DateTime.Today }
			};
		}
	}
}

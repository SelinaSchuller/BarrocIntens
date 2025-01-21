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
using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;

namespace BarrocIntens.Onderhoud
{
	public sealed partial class OnderhoudIngekomenStoringen : Window
	{
		private List<ServiceRequest> _storingenLijst { get; set; }

		public OnderhoudIngekomenStoringen()
		{
			this.InitializeComponent();

			this.Title = $"Storingen";

			var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
			var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
			var appWindow = AppWindow.GetFromWindowId(windowId);

			appWindow.Resize(new SizeInt32(500, 800));

			using(var db = new AppDbContext())
			{
				//explicit loading:
				_storingenLijst = db.ServiceRequests
					.Where(s => s.Status == 1)
					.Include(s => s.Customer)
					.OrderBy(s => s.Date_Reported)
					.ToList();

				storingenListView.ItemsSource = _storingenLijst;

			}
		}
	}
}

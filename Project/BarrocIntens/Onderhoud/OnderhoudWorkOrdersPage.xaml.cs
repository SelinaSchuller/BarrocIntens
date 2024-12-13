using BarrocIntens.Data;
using Microsoft.EntityFrameworkCore;
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

namespace BarrocIntens.Onderhoud
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OnderhoudWorkOrdersPage : Page
	{
		private List<WorkOrder> _workOrders { get; set; }
		public OnderhoudWorkOrdersPage()
		{
			this.InitializeComponent();
			LoadData();
		}

		private void LoadData()
		{
			using(var db = new AppDbContext())
			{
				_workOrders = db.WorkOrders
					.OrderByDescending(w => w.Date_Created)
					.Include(w => w.Appointment)
					.ThenInclude(a => a.Customer)
					.Include (w => w.Request)
					.ThenInclude(r => r.Product)
					.Include(w => w.User)
					.Include(w => w.WorkOrderProducts)
					.ToList();
				workOrdersListView.ItemsSource = _workOrders;

			}
		}
	}
}

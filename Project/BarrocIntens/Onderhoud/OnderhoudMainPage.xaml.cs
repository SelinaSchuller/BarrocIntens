using BarrocIntens.Data;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        List<Appointment> appointments2 = new List<Appointment>();
        public OnderhoudMainPage()
		{
			this.InitializeComponent();
            
			using (var db = new AppDbContext())
            {
                List<CalenderInfo> calenderInfos1 = new List<CalenderInfo>();
                List<CalenderInfo> calenderInfos = new List<CalenderInfo>();
                DateTime date = DateTime.Now;
                var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                var appointments = db.Appointments.OrderBy(a => a.Date).ToArray();
				List<List<Appointment>> appointments1 = new List<List<Appointment>>();
                for (int i = 0; i <= 6; i++)
                {
                    appointments1.Add(new List<Appointment>());
                }
				
                bool AreFallingInSameWeek(DateTime date1, DateTime date2, DayOfWeek weekStartsOn)
                {
                    return date1.AddDays(-GetOffsetedDayofWeek(date1.DayOfWeek, (int)weekStartsOn)) == date2.AddDays(-GetOffsetedDayofWeek(date2.DayOfWeek, (int)weekStartsOn));
                }

                int GetOffsetedDayofWeek(DayOfWeek dayOfWeek, int offsetBy)
                {
                    return (((int)dayOfWeek - offsetBy + 7) % 7);
                }
                foreach (var appointment in appointments)
				{
					if (AreFallingInSameWeek(date.Date, appointment.Date.Date, DayOfWeek.Monday))
					{
                         appointments2.Add(appointment);
                    }
				}
                    foreach (var item in appointments2)
                    {
                        CalenderInfo calenderInfo = new CalenderInfo();
                        if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek;
                            calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 1;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; 
                            calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 2;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; 
                            calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 3;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; 
                            calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 4;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek;
                            calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 5;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 6;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 7;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 8;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 9;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 10;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 11;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 12;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 13;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 14;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 15;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 16;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 17;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 18;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 19;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 20;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 21;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 22;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 23;
                        }
                        else if (TimeOnly.FromDateTime(item.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(item.Date) <= TimeOnly.ParseExact("00:00:00", "HH:mm:ss"))
                        {
                            calenderInfo.id = item.Id;
                            calenderInfo.Day = item.Date.DayOfWeek; calenderInfo.Appointment = item;
                            calenderInfo.Date = item.Date;
                            calenderInfo.Row = 24;
                        }
                        calenderInfos.Add(calenderInfo);
                    }
                monday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                monday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Monday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);

                tuesday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                tuesday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Tuesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);

                wednesday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                wednesday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Wednesday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);

                thursday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                thursday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Thursday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);

                friday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                friday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Friday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);

                saturday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                saturday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Saturday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);

                sunday1.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("00:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("01:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday2.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("01:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("02:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday3.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("02:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("03:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday4.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("03:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("04:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday5.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("04:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("05:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday6.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("05:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("06:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday7.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("06:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("07:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday8.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("07:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("08:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday9.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("08:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("09:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday10.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("09:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("10:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday11.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("10:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("11:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday12.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("11:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("12:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday13.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("12:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("13:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday14.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("13:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("14:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday15.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("14:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("15:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday16.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("15:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("16:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday17.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("16:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("17:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday18.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("17:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("18:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday19.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("18:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("19:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday20.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("19:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("20:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday21.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("20:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("21:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday22.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("21:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("22:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday23.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("22:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:00:00", "HH:mm:ss")).Select(ca => ca.Description);
                sunday24.ItemsSource = appointments2.Where(c => c.Date.DayOfWeek == DayOfWeek.Sunday && TimeOnly.FromDateTime(c.Date) >= TimeOnly.ParseExact("23:00:00", "HH:mm:ss") && TimeOnly.FromDateTime(c.Date) <= TimeOnly.ParseExact("23:59:59", "HH:mm:ss")).Select(ca => ca.Description);
            }
        }

        private void ButtonMonday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Monday).OrderBy(a => a.Date);
            }
        }

        private void ButtonTuesday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Tuesday).OrderBy(a => a.Date);
            }
        }

        private void ButtonWednesday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Wednesday).OrderBy(a => a.Date);
            }
        }

        private void ButtonThursday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Thursday).OrderBy(a => a.Date);
            }
        }

        private void ButtonFriday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Friday).OrderBy(a => a.Date);
            }
        }

        private void ButtonSaturday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Saturday).OrderBy(a => a.Date);
            }
        }

        private void ButtonSunday_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Collapsed;
            dayCalander.Visibility = Visibility.Visible;
            List<Appointment> appointments = new List<Appointment>();
            using (var db = new AppDbContext())
            {
                DayData.ItemsSource = db.Appointments.Include(a => a.Customer).Include(a => a.User).Where(a => appointments2.Contains(a)).Where(a => a.Date.DayOfWeek == DayOfWeek.Sunday).OrderBy(a => a.Date);
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            weekCalander.Visibility = Visibility.Visible;
            dayCalander.Visibility = Visibility.Collapsed;
        }
    }
}

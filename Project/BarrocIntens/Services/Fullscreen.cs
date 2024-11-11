using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;

namespace BarrocIntens.Services
{
	public class Fullscreen : IFullscreen
	{
		private AppWindow _appWindow;

		public void SetFullscreen(Window window)
		{
			_appWindow = GetAppWindowForCurrentWindow(window);
			_appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
		}

		public AppWindow GetAppWindowForCurrentWindow(Window window)
		{
			IntPtr hWnd = WindowNative.GetWindowHandle(window);
			WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
			return AppWindow.GetFromWindowId(myWndId);
		}
	}

	public interface IFullscreen
	{
		void SetFullscreen(Window window);
	}
}

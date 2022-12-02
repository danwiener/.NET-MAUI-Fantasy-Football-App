

namespace FantasyFootballMAUI;

public partial class App : Application
{
	public App(LoginPage lp)
	{
		InitializeComponent();
		var width = 1920;
		var height = 1200;
		Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
		{
#if WINDOWS
			var nativeWindow = handler.PlatformView;
			nativeWindow.Activate();
			IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
			Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
			Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
			//TODO: this is hardcoded stuff for fullhd
			appWindow.MoveAndResize(new Windows.Graphics.RectInt32((1920 / 2) - width / 2, (1200 / 2) - height / 2, width, height));
#endif
		});
			Routing.RegisterRoute("OpeningPage", typeof(OpeningPage));
			Routing.RegisterRoute("LoginPage", typeof(LoginPage));
			Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));

			//var navPage = new NavigationPage(new RegisterPage());
			//navPage.BarBackgroundColor = Color.FromArgb("eeeeee");
			//navPage.BarTextColor = Color.FromArgb("233b66");
			//navPage.WidthRequest = 600;
			//navPage.HeightRequest = 500;

			
			//appshell.WidthRequest = 600;
			//appshell.HeightRequest = 500;
			MainPage = new AppShell();
	} // End constructor
}

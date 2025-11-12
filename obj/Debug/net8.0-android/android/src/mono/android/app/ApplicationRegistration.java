package mono.android.app;

public class ApplicationRegistration {

	public static void registerApplications ()
	{
				// Application and Instrumentation ACWs must be registered first.
		mono.android.Runtime.register ("Experiencias_Significativas_App.MAUI.MainApplication, Experiencias_Significativas_App.MAUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", crc646e0a87eff3bfc58f.MainApplication.class, crc646e0a87eff3bfc58f.MainApplication.__md_methods);
		mono.android.Runtime.register ("Microsoft.Maui.MauiApplication, Microsoft.Maui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", crc6488302ad6e9e4df1a.MauiApplication.class, crc6488302ad6e9e4df1a.MauiApplication.__md_methods);
		
	}
}

namespace AtomicLibrary.WinFormsDemo;

/// <summary>The main entry point for the AtomicLibrary WinForms demo application, which initializes the application configuration and runs the main form.</summary>
internal static class Program
{
	/// <summary>The main entry point for the application.</summary>
	[STAThread]
	private static void Main()
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		using Form1 mainForm = new();
		Application.Run(mainForm: mainForm);
	}
}
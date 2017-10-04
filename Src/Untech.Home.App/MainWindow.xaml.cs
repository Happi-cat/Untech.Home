using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Untech.Home.App
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
			this.AttachDevTools();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			var expensesGrid = this.FindControl<Grid>("Expense");
			var savingGrid = this.FindControl<Grid>("Saving");
			var incomeGrid = this.FindControl<Grid>("Income");
		}
	}
}

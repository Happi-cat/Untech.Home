using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Untech.Home.App.Controls
{
    public class AnnualFinancialReportControl : UserControl
    {
        public AnnualFinancialReportControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

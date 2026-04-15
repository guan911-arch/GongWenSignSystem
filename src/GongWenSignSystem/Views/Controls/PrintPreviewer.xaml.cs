using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GongWenSignSystem.ViewModels;

namespace GongWenSignSystem.Views.Controls
{
    public partial class PrintPreviewer : UserControl
    {
        public PrintPreviewer()
        {
            InitializeComponent();
            this.DataContextChanged += PrintPreviewer_DataContextChanged;
        }

        private void PrintPreviewer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateVisual();
        }

        public void UpdateVisual()
        {
            if (this.DataContext is MainViewModel vm)
            {
                var visual = vm.PreviewVisual as DrawingVisual;
                if (visual != null)
                {
                    var host = new PrintVisualHost(visual);
                    VisualHost.Content = host;
                }
            }
        }

        private class PrintVisualHost : FrameworkElement
        {
            private readonly DrawingVisual _visual;
            public PrintVisualHost(DrawingVisual visual) => _visual = visual;

            protected override void OnRender(DrawingContext drawingContext)
            {
                drawingContext.DrawDrawing(_visual.Drawing);
            }
        }
    }
}

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
                    // In a real WPF implementation, we wrap the DrawingVisual in a
                    // DrawingVisualHost or use a custom FrameworkElement to render it.
                    // Here we simulate the hosting logic.
                    var host = new VisualHost(visual);
                    VisualHost.Content = host;
                }
            }
        }

        private class VisualHost : FrameworkElement
        {
            private readonly DrawingVisual _visual;
            public VisualHost(DrawingVisual visual) => _visual = visual;

            protected override void OnRender(DrawingContext drawingContext)
            {
                // Render the PrintEngine's output directly to the screen
                drawingContext.DrawDrawing(_visual.Drawing);
            }
        }
    }
}

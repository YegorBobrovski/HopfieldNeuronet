using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NeuroNet
{
    public partial class MainWindow
    {
        private static IEnumerable<ListBoxItem> SourceImages =>
            PixelImage.Images.Select(
                                     pi =>
                                         new ListBoxItem
                                         {
                                             Tag = pi,
                                             Content = pi.Grid(100),
                                             Margin = new Thickness(0, 3, 0, 3),
                                             HorizontalContentAlignment = HorizontalAlignment.Center
                                         });

        private PixelImage CurrentImage { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            PixelImage.LoadImages();
            HopfieldNet.Learn();
            SourceBox.ItemsSource = SourceImages;
        }


        private void SourceImageChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems[0] as ListBoxItem;
            var img = item?.Tag as PixelImage;
            if (img == null) return;

            CurrentImage = img;
            SelectedItemContainer.Children.Clear();
            SelectedItemContainer.Children.Add(CurrentImage.Grid(150));
            TestNeuronet(null, null);
        }

        private void TestNeuronet(object sender, MouseButtonEventArgs e)
        {
            ResultBox.Items.Clear();
            foreach (double noiseRate in Utils.Range(0, 0.1, 1))
            {
                PixelImage source = CurrentImage.AddNoise(noiseRate);
                int stepsDone;
                PixelImage result = HopfieldNet.Recognize(source, out stepsDone);

                const double gridSide = 100;

                ResultBox.Items.Add(new ListBoxItem
                                    {
                                        Content =
                                            new WrapPanel
                                            {
                                                Children =
                                                {
                                                    source.Grid(gridSide),
                                                    new Label
                                                    {
                                                        Content = $"{noiseRate*100:F0}%\n===>\n{stepsDone:D3}",
                                                        VerticalAlignment = VerticalAlignment.Center
                                                    },
                                                    result.Grid(gridSide)
                                                }
                                            },
                                        Margin = new Thickness(0, 2, 0, 3)
                                    });
            }
        }
    }
}

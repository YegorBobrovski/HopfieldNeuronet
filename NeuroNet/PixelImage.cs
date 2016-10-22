using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NeuroNet
{
    internal class PixelImage
    {
        public int[] NeuronStates { get; set; }
        private string Tag { get; set; }

        public UniformGrid Grid(double side = 0)
        {
            var grid = new UniformGrid
            {
                Rows = 10,
                Columns = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                ToolTip = Tag
            };
            if (side > 0)
            {
                grid.Width = side;
                grid.Height = side;
            }

            foreach (int ns in NeuronStates)
            {
                grid.Children.Add(new Viewbox
                                  {
                                      Stretch = Stretch.Uniform,
                                      Child = new Rectangle
                                      {
                                          Width = 100,
                                          Height = 100,
                                          Fill =
                                              new SolidColorBrush
                                              {
                                                  Color =
                                                      ns == NeuronState.Upper ? Colors.White : Colors.Black
                                              }
                                      }
                                  });
            }

            return grid;
        }

        private PixelImage(int[] mask, string tag)
        {
            NeuronStates = mask;
            Tag = tag;
        }

        private PixelImage(PixelImage src)
        {
            NeuronStates = (int[])src.NeuronStates.Clone();
            Tag = src.Tag;
        }

        public PixelImage AddNoise(double noiseRate)
        {
            noiseRate = noiseRate < 0 ? 0 : (noiseRate > 1 ? 1 : noiseRate);

            var copy = new PixelImage(this);

            if (noiseRate <= 0) return copy;

            IEnumerable<int> indieces =
                Enumerable.Range(0, NeuronStates.Length)
                          .ToList()
                          .Shuffle()
                          .Take((int)Math.Round(noiseRate*NeuronStates.Length));
            foreach (int index in indieces)
                copy.NeuronStates[index] = copy.NeuronStates[index] == NeuronState.Upper
                                               ? NeuronState.Lower
                                               : NeuronState.Upper;

            copy.Tag = Tag + $" {noiseRate*100 :F0}%";
            return copy;
        }

        public PixelImage Clone()
        {
            return new PixelImage(this);
        }

        #region static

        public static readonly List<PixelImage> Images = new List<PixelImage>();

        public static void LoadImages()
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines("srcimgs.txt");
            }
            catch (Exception)
            {
                lines = new string[0];
            }

            int ii = 0;
            int[] mask = null;
            string tag = string.Empty;
            foreach (string line in lines)
            {
                if (line.StartsWith("#")) continue;

                if (line.Length == 0 || line.StartsWith("//"))
                {
                    ii = 0;
                    if (mask != null)
                    {
                        Images.Add(new PixelImage(mask, tag));
                        mask = null;
                    }
                    tag = line.TrimStart('/', ' ');
                    continue;
                }

                if (mask == null)
                    mask = new int[HopfieldNet.NeuronsCount];

                for (int jj = 0; jj < HopfieldNet.Dim && jj < line.Length; jj++)
                {
                    mask[ii*HopfieldNet.Dim + jj] = line[jj].Equals('|') ? NeuronState.Upper : NeuronState.Lower;
                }

                if (++ii >= HopfieldNet.NeuronsCount) ii = HopfieldNet.NeuronsCount - 1;
            }
        }

        #endregion
    }
}

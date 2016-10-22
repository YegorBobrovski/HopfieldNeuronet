using System.Linq;

namespace NeuroNet
{
    internal static class NeuronState
    {
        public const int Upper = 1;
        public const int Lower = -1;
    }

    internal static class HopfieldNet
    {
        public const int Dim = 10;
        public const int NeuronsCount = Dim*Dim;

        private static readonly double[,] NetCoeffs = new double[NeuronsCount, NeuronsCount];

        public static void Learn()
        {
            for (int ii = 0; ii < NeuronsCount; ii++)
                for (int jj = 0; jj < ii; jj++)
                {
                    double val = PixelImage.Images.Sum(img => img.NeuronStates[ii]*img.NeuronStates[jj]);
                    NetCoeffs[ii, jj] = val;
                    NetCoeffs[jj, ii] = val;
                }
        }

        public static PixelImage Recognize(PixelImage source, out int stepsDone)
        {
            stepsDone = 0;
            PixelImage result = source.Clone();

            do
            {
                stepsDone++;
            } while (stepsDone < 100 && Step(result));

            return result;
        }

        private static bool Step(PixelImage image)
        {
            bool valueChanged = false;
            int[] oldNetState = image.NeuronStates;
            var newNetState = (int[])oldNetState.Clone();

            for (int ii = 0; ii < NeuronsCount; ii++)
            {
                double val = 0;
                for (int jj = 0; jj < NeuronsCount; jj++)
                    val += oldNetState[jj]*NetCoeffs[ii, jj];

                newNetState[ii] = val > 0 ? NeuronState.Upper : NeuronState.Lower;
                valueChanged |= newNetState[ii] != oldNetState[ii];
            }

            image.NeuronStates = newNetState;
            return valueChanged;
        }
    }
}

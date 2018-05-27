namespace Kaliko.ImageLibrary.BitFilters
{
    public class SubtractFilter : IBitFilter
    {
        private BitMatrix m_SubtractionMatrix;

        public SubtractFilter(KalikoImage subtractImage)
        {
            m_SubtractionMatrix = new BitMatrix(subtractImage);
        }

        public void Run(BitMatrix matrix)
        {
            PerformSubtraction(matrix);
        }

        private void PerformSubtraction(BitMatrix matrix)
        {
            for (int j = 0; j < matrix.Height; ++j)
            {
                for (int i = 0; i < matrix.Width; ++i)
                {
                    if (i < m_SubtractionMatrix.Width && j < m_SubtractionMatrix.Height)
                    {
                        if (m_SubtractionMatrix[i, j])
                        {
                            matrix[i, j] = false;
                        }
                    }
                }
            }
        }
    }
}

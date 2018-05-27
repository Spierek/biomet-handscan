namespace Kaliko.ImageLibrary.BitFilters
{
    public class AutoCropFilter : IBitFilter
    {
        public void Run(BitMatrix matrix)
        {
            PerformAutoCrop(matrix);
        }

        private void PerformAutoCrop(BitMatrix matrix)
        {
            int left, right, top, bottom;
            GetLastEmptyLeftRow(matrix, out left);
            GetLastEmptyRightRow(matrix, out right);
            GetLastEmptyTopRow(matrix, out top);
            GetLastEmptyBottomRow(matrix, out bottom);

            matrix.Crop(
                left,
                top,
                matrix.Width - left - (matrix.Width - right),
                matrix.Height - top - (matrix.Height - bottom)
                );
        }

        private void GetLastEmptyLeftRow(BitMatrix matrix, out int left)
        {
            left = 0;
            int leftLimit = matrix.Width / 2;

            while (left < leftLimit)
            {
                if (!CheckColumnBlack(matrix, left))
                {
                    return;
                }
                left++;
            }
        }

        private void GetLastEmptyRightRow(BitMatrix matrix, out int right)
        {
            right = matrix.Width;
            int rightLimit = matrix.Width / 2 + 1;

            while (right > rightLimit)
            {
                if (!CheckColumnBlack(matrix, right - 1))
                {
                    return;
                }
                right--;
            }
        }

        private void GetLastEmptyTopRow(BitMatrix matrix, out int top)
        {
            top = 0;
            int topLimit = matrix.Height / 2;

            while (top < topLimit)
            {
                if (!CheckRowBlack(matrix, top))
                {
                    return;
                }
                top++;
            }
        }

        private void GetLastEmptyBottomRow(BitMatrix matrix, out int bottom)
        {
            bottom = matrix.Height;
            int bottomLimit = matrix.Height / 2 + 1;

            while (bottom > bottomLimit)
            {
                if (!CheckRowBlack(matrix, bottom - 1))
                {
                    return;
                }
                bottom--;
            }
        }

        // check if column contains only black pixels
        private bool CheckColumnBlack(BitMatrix matrix, int x)
        {
            for (int y = 0; y < matrix.Height; ++y)
            {
                if (matrix[x, y])
                {
                    return false;
                }
            }

            return true;
        }

        // check if row contains only black pixels
        private bool CheckRowBlack(BitMatrix matrix, int y)
        {
            for (int x = 0; x < matrix.Width; ++x)
            {
                if (matrix[x, y])
                {
                    return false;
                }
            }

            return true;
        }
    }
}

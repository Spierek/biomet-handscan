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
                matrix.RowCount - left - (matrix.RowCount - right),
                matrix.ColumnCount - top - (matrix.ColumnCount - bottom)
                );
        }

        private void GetLastEmptyLeftRow(BitMatrix matrix, out int left)
        {
            left = 0;
            int leftLimit = matrix.RowCount / 2;

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
            right = matrix.RowCount;
            int rightLimit = matrix.RowCount / 2 + 1;

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
            int topLimit = matrix.RowCount / 2;

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
            bottom = matrix.ColumnCount;
            int bottomLimit = matrix.ColumnCount / 2 + 1;

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
            for (int y = 0; y < matrix.ColumnCount; ++y)
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
            for (int x = 0; x < matrix.RowCount; ++x)
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

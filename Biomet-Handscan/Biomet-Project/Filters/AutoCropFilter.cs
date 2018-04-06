namespace Kaliko.ImageLibrary.Filters
{
    public class AutoCropFilter : IFilter
    {
        public void Run(KalikoImage image)
        {
            PerformAutoCrop(image);
        }

        private void PerformAutoCrop(KalikoImage image)
        {
            int left, right, top, bottom;
            GetLastEmptyLeftRow(image, out left);
            GetLastEmptyRightRow(image, out right);
            GetLastEmptyTopRow(image, out top);
            GetLastEmptyBottomRow(image, out bottom);

            image.Crop(left, top, image.Width - left - (image.Width - right), image.Height - top - (image.Height - bottom));
        }

        private void GetLastEmptyLeftRow(KalikoImage image, out int left)
        {
            byte[] byteArray = image.ByteArray;

            left = 0;
            int leftLimit = image.Width / 2;

            while (left < leftLimit)
            {
                if (!CheckColumnBlack(byteArray, left, image.Height))
                {
                    return;
                }
                left++;
            }
        }

        private void GetLastEmptyRightRow(KalikoImage image, out int right)
        {
            byte[] byteArray = image.ByteArray;

            right = image.Width;
            int rightLimit = image.Width / 2 + 1;

            while (right > rightLimit)
            {
                if (!CheckColumnBlack(byteArray, right - 1, image.Height))
                {
                    return;
                }
                right--;
            }
        }

        private void GetLastEmptyTopRow(KalikoImage image, out int top)
        {
            byte[] byteArray = image.ByteArray;

            top = 0;
            int topLimit = image.Height / 2;

            while (top < topLimit)
            {
                if (!CheckRowBlack(byteArray, top, image.Width))
                {
                    return;
                }
                top++;
            }
        }

        private void GetLastEmptyBottomRow(KalikoImage image, out int bottom)
        {
            byte[] byteArray = image.ByteArray;

            bottom = image.Height;
            int bottomLimit = image.Height / 2 + 1;

            while (bottom > bottomLimit)
            {
                if (!CheckRowBlack(byteArray, bottom - 1, image.Width))
                {
                    return;
                }
                bottom--;
            }
        }

        // check if column contains only black pixels
        private bool CheckColumnBlack(byte[] byteArray, int x, int height)
        {
            int heightMod = height * 4;
            for (int i = 0, l = heightMod; i < l; i += 4)
            {
                int offset = i + x * heightMod;
                byte grayscale = ImageHelper.GetGrayscalePixel(byteArray[offset + 2], byteArray[offset + 1], byteArray[offset]);
                if (grayscale > byte.MinValue)
                {
                    return false;
                }
            }

            return true;
        }

        // check if row contains only black pixels
        private bool CheckRowBlack(byte[] byteArray, int y, int width)
        {
            int widthMod = width * 4;
            int offset = y * widthMod;
            for (int i = 0, l = widthMod; i < l; i += 4)
            {
                int currOffset = i + offset;
                byte grayscale = ImageHelper.GetGrayscalePixel(byteArray[currOffset + 2], byteArray[currOffset + 1], byteArray[currOffset]);
                if (grayscale > byte.MinValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

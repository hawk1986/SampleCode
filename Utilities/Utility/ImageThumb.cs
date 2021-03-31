using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Utilities.Utility
{
    public static class ImageThumb
    {
        /// <summary>
        /// 取得縮圖後的影像的大小
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="maxPx">最大的影像像素</param>
        /// <returns></returns>
        public static int[] GetThumbPicWidthAndHeight(System.Drawing.Image image, int maxPx)
        {
            //要回傳的結果
            int fixWidth = 0;
            int fixHeight = 0;

            //如果圖片的寬大於最大值或高大於最大值就往下執行
            if (image.Width > maxPx || image.Height > maxPx)
            {
                //圖片的寬大於圖片的高 
                if (image.Width >= image.Height)
                {
                    fixHeight = Convert.ToInt32((Convert.ToDouble(maxPx) / Convert.ToDouble(image.Width)) * Convert.ToDouble(image.Height));
                    //設定修改後的圖高 
                    fixWidth = maxPx;
                }
                else
                {
                    fixWidth = Convert.ToInt32((Convert.ToDouble(maxPx) / Convert.ToDouble(image.Height)) * Convert.ToDouble(image.Width));
                    //設定修改後的圖寬 
                    fixHeight = maxPx;
                }
            }
            else
            {
                //圖片沒有超過設定值，不執行縮圖 
                fixHeight = image.Height;
                fixWidth = image.Width;
            }

            return new int[] { fixWidth, fixHeight };
        }

        /// <summary>
        /// 依影像的寬度取得縮圖後的影像的大小
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="maxWidth">縮圖的寬度</param>
        /// <returns></returns>
        public static int[] GetThumbPicWidth(System.Drawing.Image image, int maxWidth)
        {
            //要回傳的結果
            int fixWidth = 0;
            int fixHeight = 0;

            //如果圖片的寬大於最大值
            if (image.Width > maxWidth)
            {
                //等比例的圖高
                fixHeight = Convert.ToInt32((Convert.ToDouble(maxWidth) / Convert.ToDouble(image.Width)) * Convert.ToDouble(image.Height));
                //設定修改後的圖寬 
                fixWidth = maxWidth;
            }
            else
            {
                //圖片寬沒有超過設定值，不執行縮圖 
                fixHeight = image.Height;
                fixWidth = image.Width;
            }

            return new int[] { fixWidth, fixHeight };
        }

        /// <summary>
        /// 依影像的高度取得縮圖後的影像的大小
        /// </summary>
        /// <param name="image">影像</param>
        /// <param name="maxHeight">縮圖高度</param>
        /// <returns></returns>
        public static int[] GetThumbPicHeight(System.Drawing.Image image, int maxHeight)
        {
            //要回傳的值
            int fixWidth = 0;
            int fixHeight = 0;

            //如果圖片的高大於最大值
            if (image.Height > maxHeight)
            {
                //等比例的寬
                fixWidth = Convert.ToInt32((Convert.ToDouble(maxHeight) / Convert.ToDouble(image.Height)) * Convert.ToDouble(image.Width));
                //圖高固定 
                fixHeight = maxHeight;
            }
            else
            {
                //圖片的高沒有超過設定值
                fixHeight = image.Height;
                fixWidth = image.Width;
            }

            return new int[] { fixWidth, fixHeight };
        }

        /// <summary>
        /// 依比率產生縮圖
        /// </summary>
        /// <param name="stream">影像流</param>
        /// <param name="maxPix">最大的橡素</param>
        /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
        public static void SaveThumbPic(Stream stream, int maxPix, string saveThumbFilePath)
        {
            //取得原始圖片
            using (Bitmap bitmap = new System.Drawing.Bitmap(stream))
            {
                //圖片寬高
                int ImgWidth = bitmap.Width;
                int ImgHeight = bitmap.Height;
                //計算維持比例的縮圖大小
                int[] thumbnailScaleWidth = GetThumbPicWidthAndHeight(bitmap, maxPix);
                int AfterImgWidth = thumbnailScaleWidth[0];
                int AfterImgHeight = thumbnailScaleWidth[1];

                //產生縮圖
                using (Bitmap thumb = new Bitmap(AfterImgWidth, AfterImgHeight))
                {
                    using (var gr = Graphics.FromImage(thumb))
                    {
                        gr.CompositingQuality = CompositingQuality.HighSpeed;
                        gr.SmoothingMode = SmoothingMode.HighSpeed;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.DrawImage(bitmap, new Rectangle(0, 0, AfterImgWidth, AfterImgHeight), 0, 0, ImgWidth, ImgHeight, GraphicsUnit.Pixel);
                        thumb.Save(saveThumbFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// 依寬度比率產生縮圖
        /// </summary>
        /// <param name="stream">影像流</param>
        /// <param name="widthMaxPix">縮圖的寬度</param>
        /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
        public static void SaveThumbPicWidth(Stream stream, int widthMaxPix, string saveThumbFilePath)
        {
            //取得原始圖片
            using (Bitmap bitmap = new System.Drawing.Bitmap(stream))
            {
                //圖片寬高
                int ImgWidth = bitmap.Width;
                int ImgHeight = bitmap.Height;
                // 計算維持比例的縮圖大小
                int[] thumbnailScaleWidth = GetThumbPicWidth(bitmap, widthMaxPix);
                int AfterImgWidth = thumbnailScaleWidth[0];
                int AfterImgHeight = thumbnailScaleWidth[1];

                // 產生縮圖
                using (Bitmap thumb = new Bitmap(AfterImgWidth, AfterImgHeight))
                {
                    using (var gr = Graphics.FromImage(thumb))
                    {
                        gr.CompositingQuality = CompositingQuality.HighSpeed;
                        gr.SmoothingMode = SmoothingMode.HighSpeed;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.DrawImage(bitmap, new Rectangle(0, 0, AfterImgWidth, AfterImgHeight), 0, 0, ImgWidth, ImgHeight, GraphicsUnit.Pixel);
                        thumb.Save(saveThumbFilePath);
                    }
                }
            }
        }

        /// <summary>
        /// 依高度比率產生縮圖
        /// </summary>
        /// <param name="stream">影像流</param>
        /// <param name="heightMaxPix">縮圖的高度</param>
        /// <param name="saveThumbFilePath">縮圖的儲存檔案路徑</param>
        public static void SaveThumbPicHeight(Stream stream, int heightMaxPix, string saveThumbFilePath)
        {
            //取得原始圖片
            using (Bitmap bitmap = new System.Drawing.Bitmap(stream))
            {
                //圖片寬高
                int ImgWidth = bitmap.Width;
                int ImgHeight = bitmap.Height;
                // 計算維持比例的縮圖大小
                int[] thumbnailScaleWidth = GetThumbPicHeight(bitmap, heightMaxPix);
                int AfterImgWidth = thumbnailScaleWidth[0];
                int AfterImgHeight = thumbnailScaleWidth[1];

                //產生縮圖
                using (Bitmap thumb = new Bitmap(AfterImgWidth, AfterImgHeight))
                {
                    using (var gr = Graphics.FromImage(thumb))
                    {
                        gr.CompositingQuality = CompositingQuality.HighSpeed;
                        gr.SmoothingMode = SmoothingMode.HighSpeed;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.DrawImage(bitmap, new Rectangle(0, 0, AfterImgWidth, AfterImgHeight), 0, 0, ImgWidth, ImgHeight, GraphicsUnit.Pixel);
                        thumb.Save(saveThumbFilePath);
                    }
                }
            }
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Utilities.Utility
{
    public class CaptchaImage
    {
        /// <summary>
        /// 列舉扭曲字型的程度
        /// </summary>
        public enum FontTwistLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme
        }

        /// <summary>
        /// 列舉背景雜點的程度
        /// </summary>
        public enum BackgroundNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme,
        }

        /// <summary>
        /// 列舉線條雜訊的程度
        /// </summary>
        public enum LineNoiseLevel
        {
            None,
            Low,
            Medium,
            High,
            Extreme,
        }

        /// <summary>
        /// 列舉 Windows XP/Server 2003 繁體中文版所內建的字型清單（使用分號分隔這些字型）
        /// </summary>
        private readonly string _fontList = "Arial;Calibri;SimHei;SimSum;Tahoma";

        /// <summary>
        /// 驗證碼圖片的高度（建議不要小於 30 個像素）
        /// </summary>
        private readonly int _height = 60;

        /// <summary>
        /// 驗證碼圖片的寬度（建議一個字以 40 個像素為計算的基準）
        /// </summary>
        private readonly int _width = 200;

        /// <summary>
        /// 亂數產生器
        /// </summary>
        private static Random _rand = new Random();

        /// <summary>
        /// 驗證碼的文字長度
        /// </summary>
        private static int _randomTextLength = 5;

        /// <summary>
        /// 扭曲字型的程度
        /// </summary>
        private readonly FontTwistLevel _fontTwist = FontTwistLevel.High;

        /// <summary>
        /// 背景雜點的程度
        /// </summary>
        private readonly BackgroundNoiseLevel _backgroundNoise = BackgroundNoiseLevel.Extreme;

        /// <summary>
        /// 線條雜訊的程度
        /// </summary>
        private readonly LineNoiseLevel _lineNoise = LineNoiseLevel.Medium;

        /// <summary>
        /// 字形顏色
        /// </summary>
        private readonly Color[] _fontColors = new Color[] { Color.DarkGreen, Color.DarkOrange, Color.DarkGray, Color.LightBlue, Color.LightPink };

        /// <summary>
        /// 隨機取得字型清單中的某個字型
        /// </summary>
        /// <returns></returns>
        private string GetRandomFontFamily()
        {
            string[] fontFamily = _fontList.Split(';');
            return fontFamily[_rand.Next(0, fontFamily.Length)];
        }

        /// <summary>
        /// 建立兩個名稱為 RandomPoint 的多載函式，會回傳 PointF 資料型別
        /// 在所指定的範圍內，隨機產生 X 軸與 Y 軸的值
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        private PointF RandomPoint(int xMin, int xMax, int yMin, int yMax)
        {
            return new PointF(_rand.Next(xMin, xMax), _rand.Next(yMin, yMax));
        }

        /// <summary>
        /// 在所指定的矩型範圍內，隨機產生 X 軸與 Y 軸的值
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// 建立內含所指定之文字字串與字型的 GraphicsPath 類別
        /// </summary>
        /// <param name="str"></param>
        /// <param name="fnt"></param>
        /// <param name="rect"></param>
        /// <returns>用來傳回一系列連接的直線和曲線</returns>
        private GraphicsPath TextPath(string str, Font fnt, Rectangle rect)
        {
            StringFormat sf = new StringFormat();
            // 指定文字字串對齊方式
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Near;

            GraphicsPath gp = new GraphicsPath();
            gp.AddString(str, fnt.FontFamily, Convert.ToInt32(fnt.Style), fnt.Size, rect, sf);
            return gp;
        }

        /// <summary>
        /// 指定文字字串之字型、大小與樣式
        /// </summary>
        /// <returns></returns>
        private Font GetFontStyle()
        {
            float fontSize;
            string fontName = GetRandomFontFamily();

            // 根據扭曲字型的程度來指定字型大小
            switch (_fontTwist)
            {
                case FontTwistLevel.None:
                    fontSize = Convert.ToInt32(_height * 0.7);
                    break;
                case FontTwistLevel.Low:
                    fontSize = Convert.ToInt32(_height * 0.75);
                    break;
                case FontTwistLevel.Medium:
                    fontSize = Convert.ToInt32(_height * 0.8);
                    break;
                case FontTwistLevel.High:
                    fontSize = Convert.ToInt32(_height * 0.85);
                    break;
                case FontTwistLevel.Extreme:
                    fontSize = Convert.ToInt32(_height * 0.9);
                    break;
                default:
                    fontSize = 0;
                    break;
            }

            FontStyle fontStyle;
            // 隨機指定字型的樣式
            switch (_rand.Next(5))
            {
                case 0:
                    fontStyle = FontStyle.Bold;
                    break;
                case 1:
                    fontStyle = FontStyle.Italic;
                    break;
                case 2:
                    fontStyle = FontStyle.Regular;
                    break;
                case 3:
                    fontStyle = FontStyle.Strikeout;
                    break;
                case 4:
                    fontStyle = FontStyle.Underline;
                    break;
                default:
                    fontStyle = FontStyle.Underline;
                    break;
            }

            return new Font(fontName, fontSize, fontStyle);
        }

        /// <summary>
        /// 扭曲文字
        /// </summary>
        /// <param name="textPath"></param>
        /// <param name="rect"></param>
        private void TwistText(GraphicsPath textPath, Rectangle rect)
        {
            // 扭曲文字的大小差異數
            float twistDivisor;
            // 扭曲文字的範圍變異數
            float rangeModifier;

            switch (_fontTwist)
            {
                case FontTwistLevel.None:
                    return;
                case FontTwistLevel.Low:
                    twistDivisor = 6;
                    rangeModifier = 1.1F;
                    break;
                case FontTwistLevel.Medium:
                    twistDivisor = 5.5F;
                    rangeModifier = 1.2F;
                    break;
                case FontTwistLevel.High:
                    twistDivisor = 5;
                    rangeModifier = 1.3F;
                    break;
                case FontTwistLevel.Extreme:
                    twistDivisor = 4.5F;
                    rangeModifier = 1.4F;
                    break;
                default:
                    twistDivisor = 0;
                    rangeModifier = 0;
                    break;
            }

            // 使用四個為一組的浮點數值 (Floating-Point Number)，用來表示矩形的位置和大小
            RectangleF rectF = new RectangleF(
                Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);

            // 計算扭曲文字所需的位置大小
            int hRange = Convert.ToInt32(rect.Height / twistDivisor);
            int wRange = Convert.ToInt32(rect.Width / twistDivisor);
            int left = rect.Left - Convert.ToInt32(wRange * rangeModifier);
            int top = rect.Top - Convert.ToInt32(hRange * rangeModifier);
            int width = rect.Left + rect.Width + Convert.ToInt32(wRange * rangeModifier);
            int height = rect.Top + rect.Height + Convert.ToInt32(hRange * rangeModifier);

            // 確保扭曲後的字不會偏離驗證碼所在的底圖
            if (left < 0) left = 0;
            if (top < 0) top = 0;
            if (width > _width) width = _width;
            if (height > _height) height = _height;

            // 隨機指定扭曲後的字所在的位置
            PointF leftTop = RandomPoint(left, left + wRange, top, top + hRange);
            PointF rightTop = RandomPoint(width - wRange, width, top, top + hRange);
            PointF leftBottom = RandomPoint(left, left + wRange, height - hRange, height);
            PointF rightBottom = RandomPoint(width - wRange, width, height - hRange, height);
            // 定義一個 PointF 結構的陣列
            PointF[] pF = new PointF[] { leftTop, rightTop, leftBottom, rightBottom };
            // 建立 3 x 3 仿射矩陣
            Matrix mtrx = new Matrix();
            // 以 0 來轉換這個 Matrix 的 X 與 Y 值
            mtrx.Translate(0.0F, 0.0F);
            // 使用平行四邊形和矩形所定義的透視點彎曲，來轉換這個 GraphicsPath，
            // 以便建立奇特的形狀，達到扭曲文字的目的
            textPath.Warp(pF, rectF, mtrx, WarpMode.Perspective, 0.5F);
        }

        /// <summary>
        /// 加入背景雜點
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="rect"></param>
        private void AddNoise(Graphics graph, Rectangle rect)
        {
            int density;
            int size;

            switch (_backgroundNoise)
            {
                case BackgroundNoiseLevel.None:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 100;
                    size = 60;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 75;
                    size = 50;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 50;
                    size = 45;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 30;
                    size = 45;
                    break;
                default:
                    density = 0;
                    size = 0;
                    break;
            }

            // 使用複合模式控制 Alpha 混色
            SolidBrush colorBrush = new SolidBrush(Color.Aqua);

            // 取得所傳入的矩型之寬度與高度最大者
            int max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);

            for (int i = 0; i < ((rect.Width * rect.Height) / density) + 1; i++)
            {
                // 自動指定筆刷顏色
                colorBrush.Color = Color.FromArgb(_rand.Next(255), _rand.Next(255), _rand.Next(255));
                // 填滿由座標對、寬度與高度指定的邊框所定義的橢圓形內部，來產生雜點
                graph.FillEllipse(colorBrush, _rand.Next(rect.Width), _rand.Next(rect.Height),
                    _rand.Next(max), _rand.Next(max));
            }
            colorBrush.Dispose();
        }

        /// <summary>
        /// 加入線條雜訊
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="rect"></param>
        private void AddLine(Graphics graph, Rectangle rect)
        {
            int length;
            int totalLines;
            float penWidth;

            switch (_lineNoise)
            {
                case LineNoiseLevel.None:
                    return;
                case LineNoiseLevel.Low:
                    length = 3;
                    totalLines = 1;
                    penWidth = (Convert.ToSingle(_height) / Convert.ToSingle(35));
                    break;
                case LineNoiseLevel.Medium:
                    length = 4;
                    totalLines = 2;
                    penWidth = (Convert.ToSingle(_height) / Convert.ToSingle(30));
                    break;
                case LineNoiseLevel.High:
                    length = 5;
                    totalLines = 3;
                    penWidth = (Convert.ToSingle(_height) / Convert.ToSingle(25));
                    break;
                case LineNoiseLevel.Extreme:
                    length = 6;
                    totalLines = 3;
                    penWidth = (Convert.ToSingle(_height) / Convert.ToSingle(20));
                    break;
                default:
                    length = 0;
                    totalLines = 0;
                    penWidth = 0;
                    break;
            }

            Color penColor;
            PointF[] pF = new PointF[length + 1];
            Pen randomColorPen = null;

            for (int i = 1; i < totalLines + 1; i++)
            {
                for (int intLoop = 0; intLoop < length + 1; intLoop++)
                {
                    pF[intLoop] = RandomPoint(rect);
                }
                // 自動指定筆的顏色
                penColor = Color.FromArgb(_rand.Next(255), _rand.Next(255), _rand.Next(255));
                randomColorPen = new Pen(penColor, penWidth);
                // 指定曲線的張力為 1.2F，來繪製曲線
                graph.DrawCurve(randomColorPen, pF, 1.2F);
                randomColorPen.Dispose();
            }
        }

        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="complex"></param>
        /// <returns></returns>
        private static string[] GetVerifyCode(bool complex)
        {
            string[] ary = new string[2];
            char[] RndSign = { '*', '@', '^' };
            string RndStr = null;
            string RndHelp = null;
            Random RndNum = new Random();

            if (complex)
            {
                for (int i = 0; i < 5; i++)
                {
                    switch (RndNum.Next(1, 5))
                    {
                        case 1:
                            //取得一個大寫的英文字母
                            RndStr = String.Concat(RndStr, Convert.ToChar(RndNum.Next(65, 90)));
                            RndHelp = String.Concat(RndHelp, "大寫｜");
                            break;
                        case 2:
                            //取得一個小寫的英文字母
                            RndStr = String.Concat(RndStr, Convert.ToChar(RndNum.Next(97, 122)));
                            RndHelp = String.Concat(RndHelp, "小寫｜");
                            break;
                        case 3:
                            //取得一個 0-9 的整數
                            RndStr = String.Concat(RndStr, Convert.ToChar(RndNum.Next(48, 57)));
                            RndHelp = String.Concat(RndHelp, "數字｜");
                            break;
                        default:
                            //取得特殊的符號
                            RndStr = String.Concat(RndStr, RndSign[RndNum.Next(0, RndSign.Length)].ToString());
                            RndHelp = String.Concat(RndHelp, "特殊｜");
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    //取得一個 0-9 的整數
                    RndStr = String.Concat(RndStr, Convert.ToChar(RndNum.Next(48, 57)));
                    RndHelp = String.Concat(RndHelp, "數字｜");
                }
            }

            ary[0] = RndStr;
            ary[1] = RndHelp;
            return ary;
        }

        /// <summary>
        /// 取得驗證碼影像
        /// </summary>
        /// <param name="byteImage"></param>
        /// <returns></returns>
        public string GetCaptchaImage(out byte[] byteImage)
        {
            MemoryStream stream = new MemoryStream();
            //取得驗證碼內容資訊
            string[] code = GetVerifyCode(false);
            string strDraw = code[0];

            // 建立 Bitmap
            using (Bitmap bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb))
            {
                // 建立畫布
                using (Graphics objGraphics = Graphics.FromImage(bitmap))
                {
                    // 指定使用平滑化處理（又稱為反鋸齒功能）
                    objGraphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // 定義一個矩型作為顯示驗證碼之用
                    Rectangle rect = new Rectangle(0, 0, _width, _height);

                    // 指定從頂點到底點往右斜的斜線花紋、
                    // 前景顏色為黃色、背景顏色為白色的筆刷
                    HatchBrush hBr = new HatchBrush(HatchStyle.WideDownwardDiagonal, Color.Yellow, Color.White);
                    // 建立一個矩型底圖
                    objGraphics.FillRectangle(hBr, rect);
                    hBr.Dispose();

                    // 文字字串偏移值
                    int charOffset = 0;
                    // 每個文字的寬度
                    double charWidth = Convert.ToDouble(_width) / Convert.ToDouble(_randomTextLength);
                    // 將每個文字視為一個矩型，便於扭曲該字
                    Rectangle rectChar;
                    // 所欲使用的字型樣式
                    Font fnt = null;

                    // 使用黑色筆刷來將文字繪製於矩型中，然後扭曲該矩型，
                    // 最後把該矩型填入原本的底圖之中
                    using (Brush br = new SolidBrush(_fontColors[_rand.Next(0, _fontColors.Length)]))
                    {
                        foreach (char ch in strDraw)
                        {
                            fnt = GetFontStyle();
                            rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), _height);

                            GraphicsPath gp = TextPath(ch.ToString(), fnt, rectChar);
                            TwistText(gp, rectChar);
                            objGraphics.FillPath(br, gp);
                            charOffset += 1;
                        }
                    }

                    // 加上背景雜點與線條雜訊
                    AddNoise(objGraphics, rect);
                    AddLine(objGraphics, rect);
                }

                //處理圖形品質
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;

                //找出 Encoder
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                        ici = codec;
                }

                //參數 - 高品質圖檔
                EncoderParameters ep = new EncoderParameters();
                int nQuality = 100;
                ep.Param[0] = new EncoderParameter(Encoder.Quality, nQuality);

                if (ici == null)
                {
                    //儲存 - 低品質
                    bitmap.Save(stream, ImageFormat.Jpeg);
                }
                else
                {
                    //儲存 - 高品質
                    bitmap.Save(stream, ici, ep);
                }

                byteImage = stream.ToArray();
                return strDraw;
            }
        }
    }
}

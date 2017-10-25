using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiInfoScraper
{
    class DataObfuscator
    {
        private SecureRandom _rand;
        private Graphics _g;
        private Bitmap _b;
        private int _width;
        private int _height;

        private Color[] _randcolor =
        {
            Color.Black,
            Color.Blue,
            Color.Red,
            Color.Orange,
            Color.Green,
            Color.Purple,
            Color.Yellow
        };

        public DataObfuscator()
        {
            this._rand = new SecureRandom();
            InitializeImage();
        }

        private void InitializeImage()
        {
            _width = 400;
            _height = 100;
            this._b = new Bitmap(_width, _height);
            this._g = Graphics.FromImage(_b);
            this._g.SmoothingMode = SmoothingMode.AntiAlias;
            _g.FillRectangle(new SolidBrush(Color.White), 0, 0,_width, _height);
        }
        public  Bitmap GetData(string data)
        {
            // Clear image each time
            InitializeImage();

            // Obscure image
            DrawPhantomText();


            // Draw main data
            DrawMainData(data);

            // Overlay obscuring
            DrawNoise();
            DrawLines();
            DrawBoxes();

            return _b;
        }
        private void DrawBoxes()
        {
            int count = _rand.Next(4, 12);
            for (int i = 0; i < count; i++)
            {
                Point one = new Point(_rand.Next(_width), _rand.Next(_height));
                _g.DrawRectangle(new Pen(RandColor()), new Rectangle(one, new Size(_rand.Next(5,_width/2), _rand.Next(5,_height/2))));
            }
        }
        private void DrawNoise()
        {
            int count = _rand.Next(25, 100);
            for (int i = 0; i < count; i++)
            {
                Point one = new Point(_rand.Next(_width), _rand.Next(_height));

                _g.FillRectangle(new SolidBrush(RandColor()), new Rectangle(one, new Size(2, 2)));
            }
        }

        private void DrawMainData(string text)
        {
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            Font f = RandomFont();

            CharacterRange[] ranges = new CharacterRange[text.Length];
            for (int i = 0; i < text.Length; i++)
                ranges[i] = new CharacterRange(i, 1);
            
            format.SetMeasurableCharacterRanges(ranges);
            
            Region[] regions =_g.MeasureCharacterRanges(text, f, new RectangleF(0, 0, _width,_height+ _height/2), format);

            // Draw the characters one at a time.
            for (int i = 0; i < text.Length; i++)
            {
                RectangleF rectf = regions[i].GetBounds(_g);
                using (Brush the_brush = new SolidBrush(Color.Black))
                {
                    DrawTextRotated((int)rectf.X, (int)rectf.Y, _rand.Next(-35, 35), text.Substring(i, 1), f, the_brush);
                }
            }
        }

        private Color RandColor()
        {
            return _randcolor[_rand.Next(_randcolor.Length)];
        }
        private void DrawLines()
        {
            int count = _rand.Next(5, 10);
            for (int i = 0; i < count; i++)
            {
                Point one = new Point(_rand.Next(_width), _rand.Next(_height));
                Point two = new Point(_rand.Next(_width), _rand.Next(_height));
                _g.DrawLine(new Pen(RandColor()), one, two);
            }
        }

        private void DrawPhantomText()
        {
            int count = _rand.Next(20, 100);
            for (int i = 0; i < count; i++)
            {
                DrawTextRotated(
                    _rand.Next(_width), _rand.Next(_height), _rand.Next(-180, 180),
                    _rand.String(1), new Font("Verdana", _rand.Next(12, 16)), new SolidBrush(Color.FromArgb(150, RandColor()))
                );
            }
        }

        private void DrawTextRotated(int x, int y, float angle, string text, Font font, Brush brush)
        {
            _g.TranslateTransform(x, y); // Set rotation point
            _g.RotateTransform(angle); // Rotate text
            _g.TranslateTransform(-x, -y); // Reset translate transform
            SizeF size = _g.MeasureString(text, font); // Get size of rotated text (bounding box)
            _g.DrawString(text, font, brush, new PointF(x - size.Width / 2.0f, y - size.Height / 2.0f)); // Draw string centered in x, y
            _g.ResetTransform(); // Only needed if you reuse the Graphics object for multiple calls to DrawString
        }

        private Font RandomFont()
        {
            string[] FontNames =
            {
                "Freestyle Script",
                "Chiller",
                "Broadway",
                "Colonna MT",
                "Jokerman",
                "Magneto"
            };
            return new Font(FontNames[_rand.Next(FontNames.Length)], _rand.Next(26, 30), FontStyle.Bold);
        }
    }
}

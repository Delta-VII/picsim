namespace Try.RJControls
{
    internal class RJToggleButton
    {
        public class RJTogglebutton : CheckBox
        {
            // Fields
            private Color onBackColor = Color.Cyan;
            private Color onToggleColor = Color.WhiteSmoke;
            private Color offbackcolor = Color.Gray;
            private Color offToggleColor = Color.Gainsboro;

            // Constructor
            public RJTogglebutton()
            {
                this.MinimumSize = new Size(45, 22);
            }

            // Methods
            //private System.Drawing.Drawing2DGraphicsPath FigurePath { get}

            private GraphicsPath GetFigurePath()
            {
                int arcSize = this.Height - 1;
                Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
                Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);

                GraphicsPath path = new GraphicsPath();
                path.StartFigure();
                path.AddArc(leftArc, 90, 180);
                path.AddArc(rightArc, 270, 180);
                path.CloseFigure();

                return path;
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                int toggleSize = this.Height - 5;
                pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                pevent.Graphics.Clear(this.Parent.BackColor);

                if (this.Checked) //ON
                {
                    //Draw the control surface
                    pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                    // Draw
                    pevent.Graphics.FillEllipse(new SolidBrush(onToggleColor),
                        new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
                }
                else // Off
                {
                    // Draw the Control Surface
                    pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                    // Draw
                    pevent.Graphics.FillEllipse(new SolidBrush(onToggleColor),
                        new Rectangle(2, 2, toggleSize, toggleSize));
                }
            }
        }
    }
}

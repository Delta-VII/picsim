using System.Drawing.Drawing2D;

namespace Try.RJControls
{
    internal class RjToggleButton
    {
        public class RjTogglebutton : CheckBox
        {
            // Fields
            private Color _onBackColor = Color.Cyan;
            private Color _onToggleColor = Color.WhiteSmoke;
            private Color _offbackcolor = Color.Gray;
            private Color _offToggleColor = Color.Gainsboro;

            // Constructor
            public RjTogglebutton()
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
                    pevent.Graphics.FillPath(new SolidBrush(_onBackColor), GetFigurePath());
                    // Draw
                    pevent.Graphics.FillEllipse(new SolidBrush(_onToggleColor),
                        new Rectangle(this.Width - this.Height + 1, 2, toggleSize, toggleSize));
                }
                else // Off
                {
                    // Draw the Control Surface
                    pevent.Graphics.FillPath(new SolidBrush(_onBackColor), GetFigurePath());
                    // Draw
                    pevent.Graphics.FillEllipse(new SolidBrush(_onToggleColor),
                        new Rectangle(2, 2, toggleSize, toggleSize));
                }
            }
        }
    }
}

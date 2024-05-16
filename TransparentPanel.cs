using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_Editor
{
    public class TransparentPanel : Panel
    {

        public Hint currentHint;

        public TransparentPanel()
        {
            var timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 100;
            timer.Enabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (currentHint != null)
            {
                Size targetSize = new Size(currentHint.Size.Width, currentHint.Size.Height);
                //e.Graphics.DrawImage(currentHint.targetImage, 0, 0 , Size.Width, Size.Height);
            }
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT

                return cp;
            }
        }
    }
}

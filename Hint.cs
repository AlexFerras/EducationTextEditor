using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Text_Editor
{

    public enum ArrowDir
    {
        TopLeft, TopMid, TopRight,
        BottomLeft, BottomMid, BottomRight
    }

    public partial class Hint : UserControl
    {

        private Image targetImage;



        public ArrowDir ArrowDirection
        {
            get { return _arrowDirection; }
            set { _arrowDirection = value; UpdateArrow(); }
        }

        private string _text;
        public string LabelText
        {
            get { return _text; }
            set { _text = value; label1.Text = _text; }
        }

        private ArrowDir _arrowDirection = ArrowDir.TopLeft;

        public Image character = Properties.Resources.fox1;
        public Image[] images = [Properties.Resources.fox1, Properties.Resources.fox2, Properties.Resources.fox3];
        public int currentCharacterIndex = 0;

        private Point[] offsets;

        public RichTextBox textBox;
        public Component targetControl;

        public Image mainImage = Properties.Resources.hint_cloud;
        public Image[] arrows;
        public Requirement[] requirements;
        private bool textNextLine = false;

        public Hint()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.White;

            this.targetControl = targetControl;

            offsets =
            [
                new Point(26, 34),
                new Point(93, 34),
                new Point(166, 34),
                new Point(26, 124),
                new Point(0, 124),
                new Point(166, 124)
            ];

            arrows =
            [
                Properties.Resources.arrow_topleft,
                Properties.Resources.arrow_topmid,
                Properties.Resources.arrow_topright,
                Properties.Resources.arrow_bottomleft,
                Properties.Resources.arrow_bottommid,
                Properties.Resources.arrow_bottomright,
            ];

            var timer = new Timer();
            timer.Interval = 300;
            timer.Tick += (s, e) => Refresh();
            timer.Enabled = true;
            ParentChanged += (s, e) => ParentUpdated();

            Enabled = true;
            UpdateArrow();

        }

        private void UpdateArrow()
        {
            var arrow = arrows[(int)_arrowDirection];
            targetImage = Utils.CombineAndResizeTwoImages(mainImage, arrow, 394, 229);
            targetImage = Utils.CombineAndResizeTwoImages(targetImage, character, 394, 229);
            if (this.Parent != null)
            {
                Parent.Invalidate(this.Bounds, true);
            }
        }

        public void ParentUpdated()
        {
            if (Parent != null)
            {
                textBox = (RichTextBox)Parent.Controls["richTextBox1"];

                textBox.TextChanged += TickAndRefresh;
            }
            else
                textBox.TextChanged -= TickAndRefresh;
        }
        public void TickAndRefresh(object s, EventArgs e)
        {
            Invalidate();
            if (textBox.Text.EndsWith("\n"))
            {
                textBox.Invalidate();
                textNextLine = true;
            }
            else if (textNextLine)
            {
                textBox.Invalidate();
                textNextLine = false;
            }


        }


        public void OffsetPosition()
        {
            var selectedOffset = offsets[(int)ArrowDirection];
            //Location.Offset(selectedOffset);

        }

        public bool drag = false;
        public bool enab = false;
        private int m_opacity = 100;

        private int alpha;

        public int Opacity
        {
            get
            {
                if (m_opacity > 100)
                {
                    m_opacity = 100;
                }
                else if (m_opacity < 1)
                {
                    m_opacity = 0;
                }
                return this.m_opacity;
            }
            set
            {
                this.m_opacity = value;
                if (this.Parent != null)
                {
                    Parent.Invalidate(this.Bounds, true);
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
#if false
            
            Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Color frmColor = this.Parent.BackColor;
            Brush bckColor = default(Brush);

            alpha = (m_opacity * 255) / 100;

            if (drag)
            {
                Color dragBckColor = default(Color);

                if (BackColor != Color.Transparent)
                {
                    int Rb = BackColor.R * alpha / 255 + frmColor.R * (255 - alpha) / 255;
                    int Gb = BackColor.G * alpha / 255 + frmColor.G * (255 - alpha) / 255;
                    int Bb = BackColor.B * alpha / 255 + frmColor.B * (255 - alpha) / 255;
                    dragBckColor = Color.FromArgb(Rb, Gb, Bb);
                }
                else
                {
                    dragBckColor = frmColor;
                }

                alpha = 255;
                bckColor = new SolidBrush(Color.FromArgb(alpha, dragBckColor));
            }
            else
            {
                bckColor = new SolidBrush(Color.FromArgb(alpha, this.BackColor));
            }

            if (this.BackColor != Color.Transparent | drag)
            {
                g.FillRectangle(bckColor, bounds);
            }
#endif
            Size targetSize = new Size(Size.Width, Size.Height);
            //label1.DrawToBitmap((Bitmap)targetImage, label1.Bounds);
            e.Graphics.DrawImage(targetImage, Size.Width / 2 - targetSize.Width / 2, Size.Height / 2 - targetSize.Height / 2, Size.Width, Size.Height);

            //bckColor.Dispose();
            g.Dispose();
            base.OnPaint(e);



        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (this.Parent != null)
            {
                Parent.Invalidate(this.Bounds, true);
            }
            base.OnBackColorChanged(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnParentBackColorChanged(e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }


    public class Requirement
    {
        public Component targetControl;

        public Func<object, EventArgs, bool> checkMethod;

        private bool _isMet;
        public bool IsMet
        {
            get { return _isMet; }
            set { _isMet = value; if (_isMet) onMet.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler onMet;

        public Requirement(Component targetControl, Func<object, EventArgs, bool> checkMethod = null)
        {
            this.targetControl = targetControl;
            this.checkMethod = checkMethod;
        }

        public void CheckSelf(object o, EventArgs args)
        {
            if (checkMethod(o, args))
                IsMet = true;

        }
    }


    public class HintRequirement : Requirement
    {
        public string text;
        public ArrowDir direction;

        public HintRequirement(Component targetControl, string text, ArrowDir direction, Func<object, EventArgs, bool> checkMethod = null) : base(targetControl, checkMethod)
        {
            this.text = text;
            this.direction = direction;
        }
    }

    public class HintStep
    {
        public Requirement[] requirements;
        public List<Hint> hints = new List<Hint>();

        public bool isRun = true;
        public bool isComplete = false;
        public Form form;
        public bool lastStep = false;


        public static HintStep CreateHintStep(Form topLevelForm, Point pos, Requirement[] requirements, bool lastStep = false, Image character = null)
        {
            List<Component> reqControls = new List<Component>();


            var step = new HintStep(requirements);
            step.lastStep = lastStep;
            step.form = topLevelForm;
            step.requirements = requirements;
            foreach (var req in requirements)
            {
                if (req is HintRequirement)
                {
                    var hint_req = (HintRequirement)req;
                    reqControls.Add(req.targetControl);
                    Hint hint = null;
                    foreach (var c in topLevelForm.Controls)
                    {
                        if (c is Hint)
                        {
                            hint = (Hint)c;
                            break;
                        }
                    }

                    if (hint == null)
                        hint =new Hint();

                    if (character != null)
                        hint.character = character;
                    else
                        hint.character = hint.images[hint.currentCharacterIndex];

                    hint.ArrowDirection = hint_req.direction;
                    hint.LabelText = hint_req.text;
                    hint.targetControl = req.targetControl;
                    hint.Parent = topLevelForm;
                    hint.Location = pos;
                    topLevelForm.Controls["richTextBox1"].Invalidate();
                    hint.OffsetPosition();
                    hint.BringToFront();
                    hint.Invalidate();
                    step.hints.Add(hint);
                }

                req.onMet += (s, e) => step.CheckRequirements();
            }

            foreach(dynamic c in topLevelForm.Controls)
            {
                if (!reqControls.Contains(c))
                {
                    if (c is RichTextBox)
                        c.ReadOnly = true;
                    else
                        c.Enabled = false;
                }
                    
            }
            foreach (dynamic c in reqControls)
            {
                EnableParentRecursive(c);
                c.Enabled = true; 
            }

            return step;
        }

        public static void EnableParentRecursive(dynamic Comp)
        {
            
            if (Comp is Control && Comp?.Parent != null)
            {
                Comp.Parent.Enabled = true;
                EnableParentRecursive(Comp.Parent);
            }

            else if (Comp is ToolStripItem && Comp?.Owner != null)
            {
                var owner = (ToolStrip)Comp.Owner;
                owner.Enabled = true;
                foreach (ToolStripItem i in owner.Items)
                {
                    if (i != Comp)
                        i.Enabled = false;
                }
            }
        }

        public async Task AwaitStep()
        {
            while (!isComplete)
                await Task.Delay(100);
            return;
        }

        public void UnlockTopFormControls()
        {
            foreach(dynamic c in form.Controls)
            {
                if (c is RichTextBox) 
                    c.ReadOnly = false;
                else
                    c.Enabled = true;
            }
        }
        public void Complete()
        {
            isComplete = true;
            if (lastStep)
            {
                UnlockTopFormControls();
                foreach (Hint h in hints)
                {
                    h.Hide();
                    h.Dispose();
                }
            }
            else
            {
                foreach (Hint h in hints)
                {
                    if (h.currentCharacterIndex + 1 > 2)
                        h.currentCharacterIndex = 0;
                    else
                        h.currentCharacterIndex++;
                }
            }    
            isRun = false;
        }

        public void CheckRequirements()
        {
            if (!isRun)
                return;
            foreach(var req in requirements)
            {
                if (!req.IsMet)
                    return;
            }
            Complete();

        }

        public HintStep(Requirement[] requirements)
        {
            this.requirements = requirements;
        }
    }




    public static class Utils 
    {
        public static Bitmap CombineAndResizeTwoImages(Image image1, Image image2, int width, int height)
        {
            //a holder for the result
            Bitmap result = new Bitmap(width, height);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the images into the target bitmap
                graphics.DrawImage(image1, 0, 0, result.Width, result.Height);
                graphics.DrawImage(image2, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }
    }
}

public class CustomLabel : Label
{
    protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
    {
        using (Brush aBrush = new SolidBrush(Color.White))
        {
            e.Graphics.DrawString(Text, Font, aBrush, ClientRectangle);
        }

    }
}
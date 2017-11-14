using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Diagnostics;
using System.Timers;

namespace AssetManager.UserInterface.CustomControls
{

    public enum SlideDirection
    {
        DefaultSlide,
        Up,
        Down,
        Left,
        Right

    }

    public enum SlideState
    {
        SlideIn,
        SlideOut,
        Paused,
        Done,
        Hold

    }

    public partial class SliderLabel
    {

        #region Fields

        private const int defaultDisplayTime = 4;
        private const SlideDirection defaultSlideInDirection = SlideDirection.Up;
        private const SlideDirection defaultSlideOutDirection = SlideDirection.Left;

        //  Private stepSize As Single = 0.25
        private float Acceleration = (float)(0.5F);

        private SlideDirection CurrentDirection;
        private SlideState CurrentSlideState = SlideState.Done;
        private float CurrentSpeed = 0;
        private int DisplayTime = 4;
        private List<MessageParameters> MessageQueue = new List<MessageParameters>();
        private int AnimationTimerInterval = 15;
        private SlideDirection SlideInDirection;
        private SlideDirection SlideOutDirection;
        private System.Timers.Timer SlideTimer;
        private SizeF TextSize;
        private PointF StartPosition = new PointF();
        private PointF EndPosition = new PointF();
        private PointF CurrentPosition = new PointF();
        private bool SlideComplete = false;
        private RectangleF lastPositionRect;

        #endregion

        #region Constructors

        public SliderLabel()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            SlideTimer = new System.Timers.Timer();
            SlideTimer.Interval = AnimationTimerInterval;
            SlideTimer.Stop();
            SlideTimer.Elapsed += new ElapsedEventHandler(Tick);

            SlideInDirection = defaultSlideInDirection;
            SlideOutDirection = defaultSlideOutDirection;
        }

        #endregion

        #region Properties

        public int DistplayTime
        {
            get
            {
                return DisplayTime;
            }
            set
            {
                DisplayTime = value;
            }
        }

        [Category("Appearance"), Browsable(true)]
        public string SlideText
        {
            get
            {
                return Text;
            }
            set
            {
                AddMessageToQueue(value, defaultSlideInDirection, defaultSlideOutDirection, defaultDisplayTime);
            }
        }

        #endregion

        //Sub New(text As String)
        //    InitializeComponent()

        //    ' text = text
        //    slideInDirection = SlideDirection.DefaultSlide
        //    slideOutDirection = SlideDirection.DefaultSlide
        //End Sub

        //Sub New(text As String, displayTime As Integer)
        //    InitializeComponent()

        //    ' text = text
        //    displayTime = displayTime
        //    slideInDirection = SlideDirection.DefaultSlide
        //    slideOutDirection = SlideDirection.DefaultSlide
        //End Sub

        //Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection)
        //    InitializeComponent()

        //    ' text = text
        //    displayTime = displayTime
        //    slideInDirection = slideInDirection
        //    slideOutDirection = SlideDirection.DefaultSlide
        //End Sub

        //Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection, slideOutDirection As SlideDirection)
        //    InitializeComponent()

        //    ' text = text
        //    displayTime = displayTime
        //    slideInDirection = slideInDirection
        //    slideOutDirection = slideOutDirection
        //End Sub

        #region Methods

        /// <summary>
        /// Primary text renderer.
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawText(Graphics canvas)
        {
            canvas.Clear(this.BackColor);
            using (var textBrush = new SolidBrush(this.ForeColor))
            {
                canvas.DrawString(this.SlideText, this.Font, textBrush, CurrentPosition);
            }

            lastPositionRect = new RectangleF(CurrentPosition.X, CurrentPosition.Y, TextSize.Width, TextSize.Height);
        }

        /// <summary>
        /// Adds new message to queue.
        /// </summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="slideInDirection">Slide in direction.</param>
        /// <param name="slideOutDirection">Slide out direction.</param>
        /// <param name="displayTime">How long (in seconds) the text will be displayed before sliding out. 0 = forever.</param>
        public void NewSlideMessage(string text, SlideDirection slideInDirection = SlideDirection.Up, SlideDirection slideOutDirection = SlideDirection.Left, int displayTime = 4)
        {
            AddMessageToQueue(text, slideInDirection, slideOutDirection, displayTime);
        }

        public void NewSlideMessage(string text, int displayTime)
        {
            AddMessageToQueue(text, defaultSlideInDirection, defaultSlideOutDirection, displayTime);
        }

        public void NewSlideMessage(string text)
        {
            AddMessageToQueue(text, defaultSlideInDirection, defaultSlideOutDirection, defaultDisplayTime);
        }


        /// <summary>
        /// Returns a <see cref="ToolStripControlHost"/> of this control for insertion into tool strips/status strips.
        /// </summary>
        /// <param name="parentControl">Target strip for this control. For inheriting font and color.</param>
        /// <returns></returns>
        public ToolStripControlHost ToToolStripControl(Control parentControl = null)
        {
            this.AutoSize = true;
            if (parentControl != null)
            {
                this.Font = parentControl.Font;
                this.BackColor = parentControl.BackColor;
            }
            var stripSlider = new ToolStripControlHost(this);
            stripSlider.AutoSize = false;
            return stripSlider;
        }

        /// <summary>
        /// Adds a new text message to the queue.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="slideInDirection"></param>
        /// <param name="slideOutDirection"></param>
        /// <param name="displayTime"></param>
        private void AddMessageToQueue(string text, SlideDirection slideInDirection, SlideDirection slideOutDirection, int displayTime)
        {
            MessageQueue.Add(new MessageParameters(text, slideInDirection, slideOutDirection, displayTime));
            ProcessQueue();
        }

        /// <summary>
        /// Displays the specified text and starts the animation.
        /// </summary>
        /// <param name="message"></param>
        private void DisplayText(MessageParameters message)
        {
            if (!string.IsNullOrEmpty(message.Message))
            {
                Text = message.Message;
                DisplayTime = message.DisplayTime;
                TextSize = GetTextSize(message.Message);
                SlideInDirection = message.SlideInDirection;
                SlideOutDirection = message.SlideOutDirection;
                SetControlSize();
                SetSlideInAnimation();
                SlideTimer.Start();
                this.Invalidate();
                this.Update();
            }

        }

        /// <summary>
        /// Measures the graphical size of the specified text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private SizeF GetTextSize(string text)
        {
            try
            {
                using (var gfx = this.CreateGraphics())
                {
                    return gfx.MeasureString(text, this.Font);
                }

            }
            catch (ObjectDisposedException)
            {
                return new SizeF();
                //We've been disposed. Do nothing.
            }
        }

        /// <summary>
        /// Async pause task.
        /// </summary>
        /// <param name="pauseTime"></param>
        /// <returns></returns>
        private Task Pause(int pauseTime)
        {
            return Task.Run(() =>
            {
                Task.Delay(pauseTime * 1000).Wait();
            });
        }

        /// <summary>
        /// Handles the message queue. Messages are queued until they their animation is complete. Messages with a display time of 0 are moved to the slide out animation status, then replaced with the next message when complete.
        /// </summary>
        private void ProcessQueue()
        {
            if (MessageQueue.Count > 0)
            {
                //If state is done, then we can display the next message
                if (CurrentSlideState == SlideState.Done)
                {
                    DisplayText(MessageQueue.Last());
                    MessageQueue.RemoveAt(MessageQueue.Count - 1);
                    //If the state is hold, then a permanent message is currently displayed. Trigger a slide out animation, which will change the state to done once complete.
                }
                else if (CurrentSlideState == SlideState.Hold)
                {
                    SetSlideOutAnimation();
                    SlideTimer.Start();
                }
            }

        }

        /// <summary>
        /// If autosize set to true, sets the control size to fit the text.
        /// </summary>
        private void SetControlSize()
        {
            this.BackColor = this.Parent.BackColor;
            if (this.AutoSize)
            {
                this.Size = GetTextSize(System.Convert.ToString(Text)).ToSize();
            }
        }

        /// <summary>
        /// Sets states, current positions and ending positions for a slide-in animation.
        /// </summary>
        private void SetSlideInAnimation()
        {
            CurrentDirection = SlideInDirection;
            CurrentSlideState = SlideState.SlideIn;
            CurrentPosition = new PointF(0, 0);
            CurrentSpeed = 0;
            SlideComplete = false;
            switch (SlideInDirection)
            {

                case SlideDirection.DefaultSlide:
                case SlideDirection.Up:
                    StartPosition.Y = TextSize.Height;
                    CurrentPosition.Y = StartPosition.Y;
                    EndPosition.Y = 0;
                    break;

                case SlideDirection.Down:
                    StartPosition.Y = -TextSize.Height;
                    CurrentPosition.Y = StartPosition.Y;
                    EndPosition.Y = 0;
                    break;

                case SlideDirection.Left:
                    StartPosition.X = TextSize.Width;
                    CurrentPosition.X = StartPosition.X;
                    EndPosition.X = 0;
                    break;

                case SlideDirection.Right:
                    StartPosition.X = -TextSize.Width;
                    CurrentPosition.X = StartPosition.X;
                    EndPosition.X = 0;
                    break;

            }
        }

        /// <summary>
        /// Sets states, current positions and ending positions for a slide-out animation.
        /// </summary>
        private void SetSlideOutAnimation()
        {
            CurrentDirection = SlideOutDirection;
            CurrentSlideState = SlideState.SlideOut;
            CurrentSpeed = 0;
            SlideComplete = false;
            switch (SlideOutDirection)
            {

                case SlideDirection.DefaultSlide:
                case SlideDirection.Up:
                    EndPosition.Y = -TextSize.Height;
                    break;

                case SlideDirection.Down:
                    EndPosition.Y = TextSize.Height;
                    break;

                case SlideDirection.Left:
                    EndPosition.X = -TextSize.Width;
                    break;

                case SlideDirection.Right:
                    EndPosition.X = TextSize.Width;
                    break;

            }
        }

        private void SliderLabelLoad(object sender, EventArgs e)
        {
            SetControlSize();
        }

        private void SliderTextBoxPaint(object sender, PaintEventArgs e)
        {
            DrawText(e.Graphics);
        }

        delegate void UpdateTextDelegate();

        /// <summary>
        /// Timer tick event for animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tick(object sender, EventArgs e)
        {
            try
            {
                if (!this.Disposing && !this.IsDisposed)
                {
                    UpdateTextDelegate d = new UpdateTextDelegate(UpdateTextPosition);
                    this.Invoke(d);
                }
                else
                {
                    SlideTimer.Stop();
                    SlideTimer.Dispose();
                }
            }
            catch (ObjectDisposedException)
            {
                //We've been disposed. Do nothing.
            }
        }

        /// <summary>
        /// Primary animation routine. Messages are animated per their current state and specified directions.
        /// </summary>
        private async void UpdateTextPosition()
        {

            //Check current direction and change X,Y positions/speeds as needed using an accumulating acceleration.
            switch (CurrentDirection)
            {
                case SlideDirection.DefaultSlide:
                case SlideDirection.Up:
                    if (CurrentPosition.Y + CurrentSpeed > EndPosition.Y)
                    {
                        CurrentSpeed -= Acceleration;
                        CurrentPosition.Y += CurrentSpeed;
                    }
                    else
                    {
                        CurrentPosition.Y = EndPosition.Y;
                        SlideComplete = true;
                    }
                    break;
                case SlideDirection.Down:
                    if (CurrentPosition.Y + CurrentSpeed < EndPosition.Y)
                    {
                        CurrentSpeed += Acceleration;
                        CurrentPosition.Y += CurrentSpeed;
                    }
                    else
                    {
                        CurrentPosition.Y = EndPosition.Y;
                        SlideComplete = true;
                    }
                    break;
                case SlideDirection.Left:
                    if (CurrentPosition.X + CurrentSpeed > EndPosition.X)
                    {
                        CurrentSpeed -= Acceleration;
                        CurrentPosition.X += CurrentSpeed;
                    }
                    else
                    {
                        CurrentPosition.X = EndPosition.X;
                        SlideComplete = true;
                    }
                    break;
                case SlideDirection.Right:
                    if (CurrentPosition.X + CurrentSpeed < EndPosition.X)
                    {
                        CurrentSpeed += Acceleration;
                        CurrentPosition.X += CurrentSpeed;
                    }
                    else
                    {
                        CurrentPosition.X = EndPosition.X;
                        SlideComplete = true;
                    }
                    break;
            }

            //Trigger redraw.
            lastPositionRect.Inflate(10, 5);
            Region updateRegion = new Region(lastPositionRect);
            this.Invalidate(updateRegion);
            this.Update();

            //Current slide animation complete.
            if (SlideComplete)
            {

                //Reset speed.
                CurrentSpeed = 0;

                //If current state is slide-in and display time is not forever.
                if (CurrentSlideState == SlideState.SlideIn & DisplayTime > 0)
                {

                    //Stop the animation timer, change state to paused, and pause for the specified display time.
                    SlideTimer.Stop();
                    CurrentSlideState = SlideState.Paused;

                    //Asynchronous wait task. (Keeps UI alive)
                    await Pause(DisplayTime);

                    //Once the wait is complete, set the next state (slide-out) and re-start the animation timer.
                    SetSlideOutAnimation();
                    SlideTimer.Start();
                }
                else
                {
                    //If the display time is forever
                    if (DisplayTime == 0)
                    {

                        //If the forever displayed message state is slide-out, then the forever message is being replaced with a new message, so change the state to done.
                        if (CurrentSlideState == SlideState.SlideOut)
                        {
                            CurrentSlideState = SlideState.Done;
                        }
                        else
                        {
                            //Otherwise, change the forever displayed message state to hold to keep it visible.
                            CurrentSlideState = SlideState.Hold;
                        }
                    }
                    else
                    {
                        //If the message has a display time, set state to done.
                        CurrentSlideState = SlideState.Done;
                    }

                    //Stop the animation timer.
                    SlideTimer.Stop();

                    //Add pause between messages if desired.
                    //Await Pause(1)
                }

            }

            //Check the queue for new messages.
            ProcessQueue();
        }

        private void SliderLabel_Disposed(object sender, EventArgs e)
        {
            MessageQueue.Clear();
            SlideTimer.Stop();
        }

        #endregion

        #region Structs

        /// <summary>
        /// Parameters for messages to be queued.
        /// </summary>
        private struct MessageParameters
        {

            #region Fields

            public int DisplayTime;
            public string Message;
            public SlideDirection SlideInDirection;
            public SlideDirection SlideOutDirection;

            #endregion

            #region Constructors

            public MessageParameters(string message, SlideDirection slideInDirection, SlideDirection slideOutDirection, int displayTime)
            {
                this.Message = message;
                this.DisplayTime = displayTime;
                this.SlideInDirection = slideInDirection;
                this.SlideOutDirection = slideOutDirection;
            }

            #endregion

        }

        #endregion

    }
}

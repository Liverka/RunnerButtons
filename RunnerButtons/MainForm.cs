using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
namespace RunnerButtons
{

    public delegate void HelperToCall(Button btn);
    public partial class MainForm : Form
    {
        Thread t1; 
        Thread t2;
        Thread t3; 

        ManualResetEvent mre;
  
        static Random r;  

        ButtonCompare[] button;

        HelperToCall helper;

        public MainForm()
        {
            InitializeComponent();

            button = new ButtonCompare[] { first_btn, second_btn, third_btn };

            helper = new HelperToCall(Motion);

            r = new Random();

            mre = new ManualResetEvent(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void start_btn_Click(object sender, EventArgs e)
        {
            switcher(false,true,true);
            if (t1 != null)
            {
                mre.Set();
                return;
            }
            else
            {
                t1 = new Thread(Movement1);
                t2 = new Thread(Movement2);
                t3 = new Thread(Movement3);                                

                t1.IsBackground = t2.IsBackground = t3.IsBackground = true;

                t1.Start();
                t2.Start();
                t3.Start();

                mre.Set();
            }
        }

        void Motion(Button button)
        {           
            button.Location = new Point(button.Location.X + r.Next(3), button.Location.Y);
            Lider();
            Finish(button);
        }

        private void Finish(Button button)
        {
            if (button.Location.X > (pictureBox1.Location.X - button.Width))
            {
                pause_btn_Click(new object(), new EventArgs());
                start_btn.Enabled = false;
                MessageBox.Show("Выиграла кнопка " + button.Text);
            }
        }

        private void Lider()   
        {
            Array.Sort(button);
            button[0].BackColor = Color.Yellow;
            for (int i = 1; i < button.Length;i++)
                button[i].BackColor = SystemColors.Control;
        }

        void Movement1()
        {
            while (true)
            {
                mre.WaitOne();
                Invoke(helper, first_btn);
                Thread.Sleep(r.Next(5,40));                              
            }
        }

        void Movement2()
        {
            while (true)
            {
                mre.WaitOne();
                Invoke(helper, second_btn);
                Thread.Sleep(r.Next(5, 40));                                            
            }
        }

        void Movement3()
        {
            while (true)
            {
                mre.WaitOne();
                Invoke(helper, third_btn);
                Thread.Sleep(r. Next(5, 40));                                             
            }
        }

        private void pause_btn_Click(object sender, EventArgs e)
        {
            switcher(true, false, true);            
            if (t1 != null)
            {
                mre.Reset();
            }       
        }        

        private void Reset()
        {
            first_btn.Location = new Point(13, first_btn.Location.Y);
            second_btn.Location = new Point(13, second_btn.Location.Y);
            third_btn.Location = new Point(13, third_btn.Location.Y);

            foreach (ButtonCompare buttons in button)
                buttons.BackColor = SystemColors.Control;
        }

        private void stop_btn_Click(object sender, EventArgs e)
        {   
            pause_btn_Click(sender, e);

            switcher(true, false, false);
            Reset();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (t1 != null)
            {
                t1.Abort();
                t2.Abort();
                t3.Abort();
            }
        }
        
        void switcher(bool flagStart,bool flagPause,bool flagStop)
        {
            start_btn.Enabled = flagStart;
            pause_btn.Enabled = flagPause;
            stop_btn.Enabled = flagStop;
        }
    }
}

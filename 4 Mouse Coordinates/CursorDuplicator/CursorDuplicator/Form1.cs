using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using System.Threading;

namespace CursorDuplicator
{
    public partial class Form1 : Form
    {

        globalKeyboardHook gkh = new globalKeyboardHook();
        public Form1()
        {
            HookManager.MouseMove += HookManager_MouseMove;
            HookManager.MouseDown += HookManager_MouseDown;
            HookManager.MouseUp += HookManager_MouseUp;
            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.B);
            gkh.HookedKeys.Add(Keys.C);
            gkh.HookedKeys.Add(Keys.D);
            gkh.HookedKeys.Add(Keys.E);
            gkh.HookedKeys.Add(Keys.F);
            gkh.HookedKeys.Add(Keys.G);
            gkh.HookedKeys.Add(Keys.H);
            gkh.HookedKeys.Add(Keys.I);
            gkh.HookedKeys.Add(Keys.J);
            gkh.HookedKeys.Add(Keys.K);
            gkh.HookedKeys.Add(Keys.L);
            gkh.HookedKeys.Add(Keys.M);
            gkh.HookedKeys.Add(Keys.N);
            gkh.HookedKeys.Add(Keys.O);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            thread = new Thread(ThreadManager);
            lstCoordinates = new List<Coordinates>();
            thread.Start();
            InitializeComponent();
        }
        bool keydown = false;
        bool keyup = false;
        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            keyup = true;
            keydown = false;
            int index = Convert.ToInt32(e.KeyData) - 65;
            if (numericUpDown1.Value >= index)
            {
                object xValue = dataGridView1.Rows[index].Cells[0].Value;
                object yValue = dataGridView1.Rows[index].Cells[1].Value;
                int newX = Cursor.Position.X + Convert.ToInt32(xValue);
                int newY = Cursor.Position.Y + Convert.ToInt32(yValue);
                Coordinates oldCoord = new Coordinates(Cursor.Position.X, Cursor.Position.Y);
                Cursor.Position = new Point(newX, newY);
                Clicker.StartClick(new Point(newX, newY));
                Pause(2);
                Clicker.EndClick(new Point(newX, newY));
                Cursor.Position = new Point(oldCoord.X, oldCoord.Y);
            }
            e.Handled = true;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            keydown = true;
            keyup = false;
            e.Handled = true;
        }
        struct Coordinates
        {
            public Coordinates(int x, int y) { this.X = x; this.Y = y; }
            public int X, Y;
        }
        Coordinates RealCursorCoordinates;
        List<Coordinates> lstCoordinates;
        Thread thread;
        static volatile int xVal, yVal;
        bool MouseDownEvent = false;

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread.Abort();
        }
        private void Pause(float ms = 0.1f)
        {
            DateTime until = DateTime.Now.AddMilliseconds(ms);
            while (DateTime.Now < until) { Application.DoEvents(); }
        }
        private bool IsNumeric(string value)
        {
            double myNum = 0;

            if (Double.TryParse(value, out myNum))
            {
                return true;
            }

            return false;
        }
        private void KeyManager()
        {
            while (chkEnable == null) { Application.DoEvents(); }
            while (true)
            {

            }
        }
        private void ThreadManager()
        {
            while (chkEnable == null) { Application.DoEvents(); }
            while (true)
            {
                bool wasDisabled = false;
                while (!chkEnable.Checked) { wasDisabled = true; };
                if (wasDisabled)
                {
                    DateTime until = DateTime.Now.AddMilliseconds(10);
                    while (DateTime.Now < until) { MouseDownEvent = false; lstCoordinates.Clear(); }
                }
                if (chkEnable.Checked)
                {
                    while (MouseDownEvent)
                    {
                        Coordinates c = new Coordinates(Cursor.Position.X, Cursor.Position.Y);
                        lstCoordinates.Add(c);
                        Pause(2);
                    }
                    if (!MouseDownEvent)
                    {
                        if (lstCoordinates.Count > 1)
                        {
                            int savedX = lstCoordinates[lstCoordinates.Count - 1].X;
                            int savedY = lstCoordinates[lstCoordinates.Count - 1].Y;
                            for (int t = 0; t < dataGridView1.Rows.Count; ++t)
                            {
                                for (int i = 0; i < lstCoordinates.Count; ++i)
                                {
                                    object xValue = dataGridView1.Rows[t].Cells[0].Value;
                                    object yValue = dataGridView1.Rows[t].Cells[1].Value;
                                    if (IsNumeric(Convert.ToString(xValue)) && IsNumeric(Convert.ToString(yValue)))
                                    {
                                        int newX = lstCoordinates[i].X + Convert.ToInt32(xValue);
                                        int newY = lstCoordinates[i].Y + Convert.ToInt32(yValue);
                                        if (i == 0)
                                        {
                                            Clicker.StartClick(new Point(newX, newY));
                                        }
                                        else if (i == lstCoordinates.Count - 1)
                                        {
                                            Clicker.EndClick(new Point(newX, newY));
                                        }
                                        else
                                        {
                                            if ((Cursor.Position.X != lstCoordinates[i - 1].X) ||
                                                (Cursor.Position.Y != lstCoordinates[i - 1].Y))
                                            {
                                                // user is trying to move cursor...
                                                PerformClick = false;
                                                EnableClickEvent = true;
                                                Clicker.EndClick(new Point(newX, newY));
                                                for (int n = 0; n < dataGridView1.Rows.Count; ++n)
                                                {
                                                    // remove additional coordinates for each cursor...
                                                    lstCoordinates.RemoveRange(i, lstCoordinates.Count - i);
                                                }
                                            }
                                            Clicker.SetCursorPos(newX, newY);//Cursor.Position = new Point(newX, newY);

                                        }
                                        Pause(2);
                                    }
                                }
                            }
                            Clicker.SetCursorPos(savedX, savedY);//Cursor.Position = new Point(newX, newY);
                            lstCoordinates.Clear();
                        }
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] values = { "0", "0" };
            for (int i = 0; i < 6; i++)
                dataGridView1.Rows.Add(values);
            oldNumValue = (int)numericUpDown1.Value;
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
        }
        bool PerformClick = false;
        bool EnableClickEvent = false;
        private void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            //if (EnableClickEvent)
            //{
            //    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            //    {
            //        Clicker.StartClick(new Point(e.X, e.Y));
            //        Pause(2);
            //        Clicker.EndClick(new Point(e.X, e.Y));
            //    }
            //    EnableClickEvent = false;
            //}
        }
        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownEvent = true;
            xVal = e.X;// - this.Location.X;
            yVal = e.Y;// - this.Location.Y;
            txtCursorLocation.Text = xVal + "," + yVal;
        }
        bool chkenabled = false;
        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkenabled)
                MouseDownEvent = false;
            chkenabled = chkEnable.Checked;
        }
        int oldNumValue = 0;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (oldNumValue > numericUpDown1.Value)
            {
                // decrease value
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                }
            }
            else
            {
                string[] values = { "0", "0" };
                dataGridView1.Rows.Add(values);
            }
            oldNumValue = (int)numericUpDown1.Value;
        }

        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDownEvent = false;
        }
    }
}

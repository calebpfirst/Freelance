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
        public Form1()
        {
            HookManager.MouseMove += HookManager_MouseMove;
            HookManager.MouseDown += HookManager_MouseDown;
            HookManager.MouseUp += HookManager_MouseUp;
            thread = new Thread(ThreadManager);
            lstCoordinates = new List<Coordinates>();
            thread.Start();
            InitializeComponent();
        }
        struct Coordinates
        {
            public Coordinates(int x, int y) { this.X = x; this.Y = y; }
            public int X, Y;
        }

        List<Coordinates> lstCoordinates;
        Thread thread;
        static volatile int xVal, yVal;
        bool MouseDownEvent = false;
        bool hookDisabled = false;

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
                        //if (lstCoordinates.Count > 0)
                        //{
                        //    if ((c.X != lstCoordinates[lstCoordinates.Count - 1].X) ||
                        //        (c.Y != lstCoordinates[lstCoordinates.Count - 1].Y))
                        //        lstCoordinates.Add(c);
                        //}
                        //else
                        lstCoordinates.Add(c);
                        Pause(5);
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
                                            Clicker.SetCursorPos(newX, newY);//Cursor.Position = new Point(newX, newY);
                                        }
                                        Pause(5);
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
        private void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownEvent = true;
            xVal = e.X;// - this.Location.X;
            yVal = e.Y;// - this.Location.Y;
            txtCursorLocation.Text = xVal + "," + yVal;
        }

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
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

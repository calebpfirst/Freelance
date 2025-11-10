using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gma.UserActivityMonitor;
using System.Threading;

using System.Runtime.InteropServices;

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
            gkh.HookedKeys.Add(Keys.P);
            gkh.HookedKeys.Add(Keys.Q);
            gkh.HookedKeys.Add(Keys.R);
            gkh.HookedKeys.Add(Keys.S);
            gkh.HookedKeys.Add(Keys.T);
            gkh.HookedKeys.Add(Keys.U);
            gkh.HookedKeys.Add(Keys.V);
            gkh.HookedKeys.Add(Keys.W);
            gkh.HookedKeys.Add(Keys.X);
            gkh.HookedKeys.Add(Keys.Y);
            gkh.HookedKeys.Add(Keys.Z);
            gkh.HookedKeys.Add(Keys.NumPad0);
            gkh.HookedKeys.Add(Keys.NumPad1);
            gkh.HookedKeys.Add(Keys.NumPad2);
            gkh.HookedKeys.Add(Keys.NumPad3);
            gkh.HookedKeys.Add(Keys.NumPad4);
            gkh.HookedKeys.Add(Keys.NumPad5);
            gkh.HookedKeys.Add(Keys.NumPad6);
            gkh.HookedKeys.Add(Keys.NumPad7);
            gkh.HookedKeys.Add(Keys.NumPad8);
            gkh.HookedKeys.Add(Keys.NumPad9);
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            lstKeys = new List<Coordinates>();
            lstForms = new List<Form2>();
            InitializeComponent();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public static void DoMouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
        static List<Coordinates> lstKeys;
        //static public void RemoveKey(Keys k)
        //{
        //    int index = GetKeyIndex(k);
        //    if (index > 0)
        //    {
        //        lstKeys.RemoveAt(index);
        //        lstForms.RemoveAt(index);
        //    }
        //}
        static int GetKeyIndex(Keys k)
        {
            for (int i = 0; i < lstKeys.Count; ++i)
            {
                if (lstKeys[i].Key == k)
                    return i;
            }

            return 0;
        }
        Keys selectedKey = Keys.A;
        string GetKeyStr(Keys k)
        {
            switch (k)
            {
                case Keys.A:
                    return "A";
                case Keys.B:
                    return "B";
                case Keys.C:
                    return "C";
                case Keys.D:
                    return "D";
                case Keys.E:
                    return "E";
                case Keys.F:
                    return "F";
                case Keys.G:
                    return "G";
                case Keys.H:
                    return "H";
                case Keys.I:
                    return "I";
                case Keys.J:
                    return "J";
                case Keys.K:
                    return "K";
                case Keys.L:
                    return "L";
                case Keys.M:
                    return "M";
                case Keys.N:
                    return "N";
                case Keys.O:
                    return "O";
                case Keys.P:
                    return "P";
                case Keys.Q:
                    return "Q";
                case Keys.R:
                    return "R";
                case Keys.S:
                    return "S";
                case Keys.T:
                    return "T";
                case Keys.U:
                    return "U";
                case Keys.V:
                    return "V";
                case Keys.W:
                    return "W";
                case Keys.X:
                    return "X";
                case Keys.Y:
                    return "Y";
                case Keys.Z:
                    return "Z";
                case Keys.NumPad0: return "0";
                case Keys.NumPad1: return "1";
                case Keys.NumPad2: return "2";
                case Keys.NumPad3: return "3";
                case Keys.NumPad4: return "4";
                case Keys.NumPad5: return "5";
                case Keys.NumPad6: return "6";
                case Keys.NumPad7: return "7";
                case Keys.NumPad8: return "8";
                case Keys.NumPad9: return "9";
            }

            return "#";
        }
        void gkh_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.Text == "SETUP")
            {
                // key presses will update the label..
                label1.Text = GetKeyStr(e.KeyCode);
                selectedKey = e.KeyCode;
            }
            else if (comboBox1.Text == "EXECUTE")
            {
                // key presses will make a click happen on the coordinate of the key.
                Keys key = e.KeyCode;
                if (GetKeyStr(key) != "#")
                {
                    int index = GetKeyIndex(key);
                    if (index < lstKeys.Count)
                    {
                        label1.Text = GetKeyStr(e.KeyCode);
                        Point oldCoord = Cursor.Position;
                        Cursor.Position = new Point(lstKeys[index].X, lstKeys[index].Y);
                        DoMouseClick();
                        //Pause(25);
                        Cursor.Position = new Point(oldCoord.X, oldCoord.Y);
                        //Clicker.SetCursorPos(oldCoord.X, oldCoord.Y);
                        //Pause(40);
                    }
                }
                else
                    MessageBox.Show("Error: Key, " + GetKeyStr(key) + " not registered");
            }

            e.Handled = true;
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        struct Coordinates
        {
            public Coordinates(int x, int y, Keys key) { this.Key = key; this.X = x; this.Y = y; }
            public int X, Y;
            public Keys Key;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void Pause(float ms = 0.1f)
        {
            DateTime until = DateTime.Now.AddMilliseconds(ms);
            while (DateTime.Now < until) { }
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

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void HookManager_MouseMove(object sender, MouseEventArgs e)
        {
        }
        private void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
        }
        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (GetKeyStr(selectedKey)!="#")
            {
                bool contains = false;
                for (int i = 0; i < lstKeys.Count; ++i)
                {
                    if (lstKeys[i].Key == selectedKey)
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    lstForms.Add(new Form2(selectedKey));
                    lstForms[lstForms.Count - 1].Show();
                    lstKeys.Add(new Coordinates(0, 0, selectedKey));
                    listBox1.Items.Add("Key:" + GetKeyStr(selectedKey));
                }
            }
            else
            {
                MessageBox.Show("please enter a valid key");
            }
        }
        static List<Form2> lstForms;
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "SETUP")
            {
                // display all forms..
                for (int i = 0; i < lstKeys.Count; ++i)
                {
                    if (!lstForms[i].Visible)
                    {
                        lstForms[i] = new Form2(lstKeys[i].Key);
                        lstForms[i].Show(this);
                        lstForms[i].Location = new Point(lstKeys[i].X, lstKeys[i].Y);
                    }
                }

                // key presses will update the label..
            }
            else if (comboBox1.Text == "EXECUTE")
            {
                // forms will disapear..
                for (int i = 0; i < lstKeys.Count; ++i)
                {
                    if (lstForms[i].Visible)
                    {
                        Coordinates tmp = new Coordinates(lstForms[i].Location.X, lstForms[i].Location.Y, lstKeys[i].Key);
                        lstKeys[i] = tmp;
                        lstForms[i].Close();
                    }
                }
                // key presses will make a click happen on the coordinate of the key.
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

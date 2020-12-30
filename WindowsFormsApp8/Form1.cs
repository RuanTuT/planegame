using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 飞机大战
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textBox3 .Focus();
        }
        int count = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            Random ran = new Random();
            int a = ran.Next(10000, 100000);
            textBox4.Text = Convert.ToString(a);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "42021188" && textBox2.Text == "12345" && textBox3.Text == textBox4.Text&&textBox3.Text !=null )
            {
                MessageBox.Show("飞机大战游戏开始！");
                this.Hide();
                Form2 form2 = new Form2();
                form2.ShowDialog();
                this.Close();
            }
            else
            {
                count++;
                if (count < 3)
                {
                    MessageBox.Show("用户名，密码或验证码输入错误，请检查信息后再次登录，还剩" + Convert.ToString(3 - count) + "次机会");
                }
                else
                {
                    MessageBox.Show("用户已锁定，无权继续登录");
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))

            {

                if (textBox1.Text == "42021188" && textBox2.Text == "12345" && textBox3.Text == textBox4.Text&&textBox3.Text!=null)

                {

                    MessageBox.Show("飞机大战游戏开始！");

                    this.Hide();

                    Form2 form2 = new Form2();

                    form2.ShowDialog();

                    this.Close();

                }

                else

                {

                    count++;

                    if (count < 3)

                        MessageBox.Show("用户名或密码错误，请检查信息后再次登录，还剩余" + Convert.ToString(3 - count) + "次机会");

                    else

                    {

                        MessageBox.Show("3次机会已用完，用户已锁定，无权继续登录");

                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            this.button2.Focus();
        }
    }
}

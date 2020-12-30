using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using 飞机大战.Properties;

namespace 飞机大战
{
    public partial class Form2 : Form
    {
        private const int speed = 2;//设置每次定时器触发时图片发生偏移的速度
        public int pic_x = 0;
        public int pic_y = 0; //背景图片移动起始的坐标
        int shot_x = 200;//定义一个全局变量，保存shotgun出现的x坐标
        int shot_y = -200;//炸弹出现时的y轴坐标
        int blood_x = 200;//医疗包出现时的x坐标
        int blood_y = -50;//医疗包出现时的y轴坐标
        public Image[] images = new Image[4]; //设置多张背景图片，每次运行程序随机产生背景图片
        private int index = 0;//背景图片索引
       Image boomimg= Resources.bomb4;
        Image shotImg = Resources.shotgun;
        Image bloodImg = Resources.bloodbox;
        bool isDropGun = false;      //是否产生炸弹的标志
        bool isDropBox = false;       //是否产生医疗包的标志

        public Form2()
        {
            InitializeComponent();
            this.Size = new Size(420, 630);//让窗体与图片一样大
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);//设置控件绘制方式
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);//设置双缓冲       
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            
            InitBackground();//初始化背景
            timer1.Interval = 80;//设置每次窗口重绘的时间
            timer1.Start();
            timer2.Interval = 60000;//设置计时器，使随着比赛时间的流逝，敌方飞机数增多
            timer2.Start();//启动计时器
            timer3.Interval = 75;//注意：这里需要将timer3的interval数值设置为略小于timer1的interval数值，以保证能显示飞机与敌机碰撞时的爆炸图像
            timer3.Start();
            timer4.Interval = 6000;
            timer4.Start();

        }
        /// 初始化背景，随机生成背景图片
        private void InitBackground()
        {
            Random ran = new Random();
            index = ran.Next(0, 4);
            images[0] = Resources.background1;//从资源获取图片，使用“using WindowsFormsApp8.Properties;”
            images[1] = Resources.background2;
            images[2] = Resources.background2;
            images[3] = Resources.background4;

        }
        /// 建立背景移动函数
        public void BackMove(Graphics g)//通过定时位置让图片发生偏移，防止有空白
        {
            g = this.CreateGraphics();
            pic_y += speed;
            if (pic_y > 630)
            {
                pic_y = 0;
            }
        }
        //
        //
        //定义一个随机产生炸弹的方法
        public void ProduceShotgun()
        {
            if (shot_y == -200)
            {
                shot_x = new Random().Next(0, 360);//使炸弹出现的位置随机
            }

            Rectangle rec1 = new Rectangle(shot_x, shot_y, shotImg.Width, shotImg.Height);//创建一个炸弹大小的矩形
            Rectangle rec2 = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImage.Width, MyPlane.myPlaneImage.Height);//创建一个包住plane的矩形
            if (new Random().Next(0, 1) == 0)//使炸弹随机投放
            {
                isDropGun = true;

            }
            if (isDropGun && !MyPlane.isGetGun && rec1.IntersectsWith(rec2))
            {
                MyPlane.isGetGun = true;
                shot_y = -200;//当飞机被炸弹击中时，使炸弹恢复初始位置
            }
            shot_y += 4;//当飞机没有被炸弹击中时，使炸弹在窗口中不断移动
            if (shot_y > 600)
            {
                shot_y = -200;
            }
        }
        //
        //
        //定义一个产生医疗包的方法
        public void ProduceBlood()
        {
            if (blood_y == -50)
            {
                blood_x = new Random().Next(100, 400);
            }

            Rectangle rec3 = new Rectangle(blood_x, 50, bloodImg.Width, bloodImg.Height);//创建一个医疗包大小的矩形
            Rectangle rec4 = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImage.Width, MyPlane.myPlaneImage.Height);
            if (new Random().Next(0, 200) == 50)//使医疗包随机投放
            {
                isDropBox = true;
                blood_y = 50;

            }
            if (isDropBox && !MyPlane.isGetBlood && rec3.IntersectsWith(rec4))
            {
                MyPlane.isGetBlood = true;

            }
        }
        //
        //
        //绘制界面上所有图像 ，避免闪烁
        private void DrawGame(Graphics g)
        {
            this.BackMove(g);
            g.DrawImage(images[index], pic_x, pic_y, 420, 630);
            g.DrawImage(images[index], pic_x, pic_y - 630, 420, 630);       //绘制背景
            MyPlane.MyPlaneShow(g);//绘制飞机
            MyPlane.MyPlaneMove();//通过键盘控制飞机移动


            g.DrawRectangle(new Pen(Color.Black), new Rectangle(10, 100, 100, 10));     //绘制血条矩形
            g.FillRectangle(Brushes.Red, 10, 101, MyPlane.health, 9);                   //填充血条矩形

            g.DrawRectangle(new Pen(Color.Blue), new Rectangle(10, 120, 100, 10));
            g.FillRectangle(Brushes.Green, 11, 121, MyPlane.score, 9);
            g.DrawString("Player：ruanyang", new Font("宋体", 9, FontStyle.Bold), Brushes.Yellow, new Point(10, 140));      //显示玩家
            g.DrawString("Score：" + MyPlane.score, new Font("宋体", 9, FontStyle.Bold), Brushes.Yellow, new Point(10, 160));      //显示分数           
            Fighter.ProduceFighter();
            Fighter.FighterMove(g); 
            
            MyBullet.ProduceBullet();
            MyBullet.MybulMove(g);
           MyBullet.isHitEnemy(g);
            EnemyBullet.ProduceEnbul();
            EnemyBullet.Move(g);
            EnemyBullet.isHitplane(g);
           

            this.ProduceShotgun();
            if (isDropGun && !MyPlane.isGetGun)          //判断是否产生shotgun,并绘制
            {
                g.DrawImage(shotImg, shot_x, shot_y);
            }
            if (MyPlane.isGetGun)
            {
                g.DrawImage(boomimg, MyPlane.x, MyPlane.y);
                shot_x = 200;
                shot_y = -200;
                MyPlane.health -= 10;
                MyPlane.isGetGun = false;
            }
            this.ProduceBlood();
            if (isDropBox && !MyPlane.isGetBlood)         //判断是否产生bloodbox,并绘制
            {
                g.DrawImage(bloodImg, blood_x, blood_y);
            }
            if (MyPlane.isGetBlood)
            {
                blood_y = -50;
                if (MyPlane.health <= 90)
                {
                    MyPlane.health += 10;
                }
                MyPlane.isGetBlood = false;

            }

        }
        protected override void OnPaint(PaintEventArgs e) //先将图像绘制到Bitmap图片中，再加载这个图片，以减少图像闪烁
        {
            Bitmap bmp = new Bitmap(this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            Graphics g = Graphics.FromImage(bmp);
            this.DrawGame(g);//将要绘制的内容先绘制到g上
            e.Graphics.DrawImage(bmp, 0, 0);
            g.Dispose();
            bmp.Dispose();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate(); //使当前窗口无效，系统自动调用OnPaint()函数重绘
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          Fighter.turn++;
            if (Fighter.turn >= 3)
            {
                timer2.Stop();
            }
            
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            MyPlane.Keydown(e.KeyCode);
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            MyPlane.Keyup(e.KeyCode);
            MyPlane.myPlaneImage = Resources.plane;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
             
                int i = 0;

                Rectangle rec1 = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImage.Width, MyPlane.myPlaneImage.Height);
                while (i < Fighter.fighters.Count)
                {
                    Rectangle rec2 = new Rectangle(Fighter.fighters[i].fighter_x, Fighter.fighters[i].fighter_y, 65, 45);
                    if (rec1.IntersectsWith(rec2))
                    {        
                    Graphics g = this.CreateGraphics();
                    g.DrawImage(boomimg,Fighter.fighters[i].fighter_x,Fighter.fighters[i].fighter_y);
                      Fighter.fighters.Remove(Fighter.fighters[i]);
                      MyPlane.health -= 10;
                      MyPlane.score += 1;
                        i--;
                    }
                    i++;
                }
           
        }

       private void timer4_Tick(object sender, EventArgs e)
        {
           if (MyPlane.result)
            {
                
                this.Close();
            }
        }
    }
    //
    //
    //
    //
    //定义一个MyPlane类
    public class MyPlane
    {
        public static int x = 180;
        public static int y = 530;//绘制的飞机左上角顶点坐标
        public static int health = 100; //血量

        private const int speed =6;//移动速度

        public static Image myPlaneImage = Resources.plane;//我方飞机图片
        static List<Keys> keys = new List<Keys>();//键盘键列表，用于控制飞机移动
        static Image gameOver = Resources.gameover;

        public static bool isGetGun = false;//是否得到shotgun的标志
        public static bool isGetBlood = false;//是否得到bloodbox的标志
        public static int score = 0;      //得分
        public static bool result = false;

        /// <summary>
        /// 显示我方飞机
        /// </summary>
        /// <param name="g"></param>
        public static void MyPlaneShow(Graphics g)
        {
            if (health > 0)
            {
                g.DrawImage(myPlaneImage, x, y);
            }
            else if (health <= 0)
            {
                g.DrawImage(myPlaneImage, 0, -300);
                x = 0;
                y = -300;
                g.DrawImage(gameOver, 10, 260);
                result = true;
            }

        }


        /// 松开键盘键
        public static void Keyup(Keys key)
        {
            keys.Remove(key);
        }

        /// 按下键盘键
        public static void Keydown(Keys key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }
        }


        /// 判断按键是否被按下
        public static bool IsKeyDown(Keys key)
        {
            return keys.Contains(key);
        }


        /// 用键盘控制我方飞机移动
        public static void MyPlaneMove()
        {
            if (IsKeyDown(Keys.A))
            {
                myPlaneImage = Resources.planeLeft;
                x -= speed;
                if (x < 5)
                    x = 5;
            }
            if (IsKeyDown(Keys.D))
            {
                myPlaneImage = Resources.planeRight;
                x += speed;
                if (x > 370)
                    x = 370;

            }
            if (IsKeyDown(Keys.A) && IsKeyDown(Keys.D))
            {
                myPlaneImage = Resources.plane;
            }
            if (IsKeyDown(Keys.W))
            {
                y -= speed;
                if (y < 5)
                    y = 5;
            }
            if (IsKeyDown(Keys.S))
            {
                y += speed;
                if (y > 530)
                    y = 530;
            }
        }
    }
    //
    //
    //
    //
    //定义一个产生敌方飞机的类
    public class Fighter
    {
        Image redImg;
        Image greenImg;
        Image yellowImg;
        public Image fighterImg;//敌机图片
        private const int speed = 4;//敌机图片移动速度
        public int fighter_x = 0;
        public int fighter_y = 0;//敌机图片移动起始的坐标
        public static List<Fighter> fighters = new List<Fighter>();//敌机对象列表
        List<Image> imgList = new List<Image>();//敌机图片列表
        public bool flag = false;//碰撞的标志
        public static int turn = 1;
        public Fighter(int x, int i)//构造函数，与类名相同
        {
            fighter_x = x;//横坐标
            redImg = Resources.fighterRed;
            greenImg = Resources.fighterGreen;
            yellowImg = Resources.fighterYellow;
            switch (i)
            {
                case 0:
                    fighterImg = redImg;
                    break;
                case 1:
                    fighterImg = greenImg;
                    break;
                case 2:
                    fighterImg = yellowImg;
                    break;
                default:
                    break;
            }

        }

        public static void ProduceFighter()
        {
            Random ran = new Random();
            switch (turn)
            {
                case 1:
                   if (ran.Next(0, 20)==4)
                    {
                        Fighter f = new Fighter(ran.Next(0, 350), ran.Next(0, 3));
                        fighters.Add(f);
                    }
                    break;
                case 2:
                    if (ran.Next(0, 12) == 4)
                    {
                        Fighter f = new Fighter(ran.Next(0, 350), ran.Next(0, 3));
                        fighters.Add(f);
                    }
                    break;
                case 3:
                    if (ran.Next(0, 8) == 4)
                    {
                        Fighter f = new Fighter(ran.Next(0, 350), ran.Next(0, 3));
                        fighters.Add(f);
                    }
                    break;
                default:
                    break;
            }
               
          

        }
        public void FighterShow(Graphics g)
        {
            g.DrawImage(fighterImg, fighter_x, fighter_y);
        }
        public static void FighterMove(Graphics g)
        {
            for (int i = 0; i < fighters.Count; i++)
            {
                fighters[i].FighterShow(g);
                fighters[i].fighter_y += speed;
                if (fighters[i].fighter_y > 650)
                {
                    fighters.Remove(fighters[i]);
                }
            }
        }
     }

         
        //
        //
        //
        //
        //定义一个新类，用于设置我方飞机的子弹发射
        public class MyBullet
    {
        private int x;//子弹横坐标
        private int y;//子弹纵坐标
        private const int speed = 23;//移动速度
        public int Angle;//子弹角度
        private Image bulImg;//定义子弹图片
        private const double PI = Math.PI;
        public static List<MyBullet> mybulList = new List<MyBullet>();//子弹对象集合
        public static Image booimg=Resources.bomb4;
        
        public MyBullet(int X, int Y, int angle)
        {
            x = X;
            y = Y;
            Angle = angle;
            switch (Angle)
            {
                case 0:
                    bulImg = Resources.bul02;
                    y -= 17;
                    break;
                case 30:
                    bulImg = Resources.bul02_30;
                    x += 12;
                    y -= 12;
                    break;
                case 60:
                    bulImg = Resources.bul02_60;
                    x += 2;
                    y -= 17;
                    break;
                case 120:
                    bulImg = Resources.bul02_120;
                    x -= 35;
                    y -= 12;
                    break;
                case 150:
                    bulImg = Resources.bul02_150;
                    x -= 20;
                    y -= 12;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 通过按键盘J键，K键，L键来产生我方子弹
        /// </summary>
        public static void ProduceBullet()
        {
            if (MyPlane.IsKeyDown(Keys.J))
            {
                mybulList.Add(new MyBullet(MyPlane.x+13, MyPlane.y - 10, 0));
            }
            else if (MyPlane.IsKeyDown(Keys.K))
            {
                mybulList.Add(new MyBullet(MyPlane.x+13, MyPlane.y - 10, 0));
                mybulList.Add(new MyBullet(MyPlane.x + 13, MyPlane.y - 8, 60));
                mybulList.Add(new MyBullet(MyPlane.x + 30, MyPlane.y - 12, 120));

            }
            else if (MyPlane.IsKeyDown(Keys.L))
            {
                mybulList.Add(new MyBullet(MyPlane.x+13, MyPlane.y - 10, 0));
                mybulList.Add(new MyBullet(MyPlane.x + 13, MyPlane.y - 8, 60));
                mybulList.Add(new MyBullet(MyPlane.x + 7, MyPlane.y - 8, 30));
                mybulList.Add(new MyBullet(MyPlane.x + 30, MyPlane.y - 12, 120));
                mybulList.Add(new MyBullet(MyPlane.x, MyPlane.y - 7, 150));
            }
        }
        /// <summary>
        /// 显示我方子弹
        /// </summary>
        /// <param name="g"></param>
        public void ShowMybul(Graphics g)
        {
            g.DrawImage(bulImg, x, y);
        }
        public static void MybulMove(Graphics g)
        {

            for (int i = 0; i < mybulList.Count; i++)
            {
                mybulList[i].ShowMybul(g);
                switch (mybulList[i].Angle)
                {
                    case 0:
                        mybulList[i].y -= (int)(speed / 2);
                        break;
                    case 30:
                        mybulList[i].x += (int)(speed * Math.Cos(PI / 6));
                        mybulList[i].y -= (int)(speed / 2);
                        break;
                    case 60:
                        mybulList[i].x += (int)(speed/ 2);
                        mybulList[i].y -= (int)(speed * Math.Cos(PI / 6));
                        break;
                    case 120:
                        mybulList[i].x -= (int)(speed / 2);
                        mybulList[i].y -= (int)(speed * Math.Cos(PI / 6));
                        break;
                    case 150:
                        mybulList[i].x -= (int)(speed * Math.Cos(PI / 6));
                        mybulList[i].y -= (int)(speed / 2);
                        break;
                }
                if (mybulList[i].y < -10 || mybulList[i].x > 415 || mybulList[i].x < 0)
                {
                    mybulList.Remove(mybulList[i]);
                }
            }
        }
        /// <summary>
        /// 敌机碰撞检测
        /// </summary>
        /// <param name="g"></param>
        public static void isHitEnemy(Graphics g)
        {
            Rectangle myplaneRec = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImage.Width, MyPlane.myPlaneImage.Height);
            int i = 0;
           while ( i<MyBullet.mybulList.Count)//用while循环，而不是if循环，防止i超过mybuList的索引值
            {   int j = 0;
                Rectangle mybulRec = new Rectangle(mybulList[i].x, mybulList[i].y, 8, 8);
                while  ( j < Fighter.fighters.Count)
                {
                    Rectangle fighterRec = new Rectangle(Fighter.fighters[j].fighter_x, Fighter.fighters[j].fighter_y, 65, 45);
                  if (mybulRec.IntersectsWith(fighterRec))//我方子弹击中敌方
                    { 
                        mybulList.Remove(mybulList[i]);
                        i--;
                        g.DrawImage(booimg, Fighter.fighters[j].fighter_x, Fighter.fighters[j].fighter_y);
                        Fighter.fighters.Remove(Fighter.fighters[j]);
                        j--;
                        if (MyPlane.score < 100)
                        {
                            MyPlane.score += 1;
                        }
                        if (i < 0)
                        {
                            i = 0;
                        }
                        else
                        {
                            i++;
                        }
                    }
                  
                    j++;               
                }
                if (i < 0)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }      
            }

        }

    }
    public class EnemyBullet
    {
        private int x;
        private int y;
        public Image enbul = Resources.en_bul01;
        public static List<EnemyBullet> enbulist = new List<EnemyBullet>();
        private int speed;
        private double k;
        
        
        public EnemyBullet(int bx,int by,int sp,int px,int py) 
        {
            x =bx;
            y = by;
            speed = sp;
            k =(1.0* (px - bx)) / (1.0*(py - by));
        }
        public static void ProduceEnbul()
        {
            for (int i = 0; i < Fighter.fighters.Count; i++)
            {
                if (new Random().Next(0, 10) == 5)
                {
                    EnemyBullet enbullet = new EnemyBullet(Fighter.fighters[i].fighter_x+25,Fighter.fighters[i].fighter_y+40,new Random().Next(10,15),MyPlane.x ,MyPlane.y);
                    enbulist.Add(enbullet);
                }
            }
        }
        public void ShowEnbul(Graphics g)
        {
            g.DrawImage(enbul, x, y);
        }

        public static void Move(Graphics g)
        {
            int i = 0;
            while (i < enbulist.Count)
            {
               enbulist[i] . ShowEnbul(g);
                enbulist[i].y += enbulist [i]. speed;
                enbulist[i].x += (int)(enbulist[i].speed * enbulist[i].k);
                if (enbulist[i].y > 700 || enbulist[i].x < 0 || enbulist[i].x > 420)
                {
                    enbulist.Remove(enbulist[i]);
                    i--;
                }
                if (i >= 0)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
            }
        }
        /// <summary>
        /// 我方飞机碰撞检测方法
        /// </summary>
        /// <param name="g"></param>
            public static void  isHitplane(Graphics g)
            {
            Rectangle rec1 = new Rectangle(MyPlane.x, MyPlane.y, 40, 50);
            int i = 0;
            while (i < enbulist.Count)
            { Rectangle rec2 = new Rectangle(enbulist[i].x, enbulist[i].y, 6, 6);
                if (rec1.IntersectsWith(rec2))
                {
                    enbulist.Remove(enbulist[i]);
                    MyPlane.health -= 1;
                    i--;
                }
                if (i >= 0)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }
            }
            }
    }

}
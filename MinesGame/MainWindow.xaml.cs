using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MinesGame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow main;
        private static int BLOCK_SIZE = 30;
        private static DispatcherTimer time;
        private static int TimeSecond = 0;
        public MainWindow()
        {
            InitializeComponent();
            //默认
            Primary_Button_Click(null, null);
            main = this;
        }
        //计时
        private void TimeStart()
        {
            //开始按钮
            time = new DispatcherTimer();
            TimeSecond = 0;
            time.Interval = new TimeSpan(0, 0, 1);
            time.Tick += (s, e1) => {//一段时间后调用单击并回到初始化
                TimeSecond++;
                TimeBlock.Text = TimeSecond + "S";
            };  //时钟触发信号
            time.Start();
        }
        private void TimeStop()
        {
            if (time != null && time.IsEnabled)
                time.Stop();
        }


        #region 事件（分单双击）
        private int i = 0;
        private bool isDouble = false;
        private void Grid_MouseLeftButtonDown(object sender, EventArgs e)   //MouseLeftButtonDown事件
        {
            i += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            timer.Tick += (s, e1) => {//一段时间后调用单击并回到初始化
                timer.IsEnabled = false;
                i = 0; Block_Click(sender);
                isDouble = false;
            };
            timer.IsEnabled = true;
            if (i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                isDouble = true;

                //双击时执行的代码
                Block_DoubleClick(sender);
            }
        }
        //鼠标右键
        private void Bt_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Button selected = sender as Button;
            Block_RightClick(sender);
            //throw new NotImplementedException();
        }

        //新游戏
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
        //初级
        private void Primary_Button_Click(object sender, RoutedEventArgs e)
        {
            MaxRow = 9; MaxCol = 9; MineNum = 10;
            NewGame();
        }
        //中级
        private void Middle_Button_Click(object sender, RoutedEventArgs e)
        {
            MaxRow = 16; MaxCol = 16; MineNum = 40;
            NewGame();
        }
        //高级
        private void High_Button_Click(object sender, RoutedEventArgs e)
        {
            MaxRow = 16; MaxCol = 30; MineNum = 99;
            NewGame();
        }

        //退出
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            this.Close();
        }
        //说明
        private void Info_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("单击、双击、右键！\n没什么好说的❥(ゝω・✿ฺ)", "游戏说明", MessageBoxButton.OK);
        }
        //关于
        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello！My creator is lzh!", "关于", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //解析坐标
        private bool SlectedIndex(String name, out int row, out int col)
        {
            row = 0; col = 0;
            String res = name.Substring(6);
            String[] RowAndCol = res.Split('_');
            if (int.TryParse(RowAndCol[0], out row) && int.TryParse(RowAndCol[1], out col))
                return true;
            return false;
        }

        #endregion

        #region 游戏
        public int MaxRow { get; set; }
        public int MaxCol { get; set; }
        public int MineNum { get; set; }
        //格子类
        class Block : INotifyPropertyChanged {
            //public int x { get; set; }
            //public int y { get; set; }
            private char flag { get; set; }
            public char Flag
            {
                get { return flag; }
                set
                {
                    flag = value;
                    if (this.PropertyChanged != null)
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Flag"));
                }
            }
            private bool isOpen { get; set; }
            public bool IsOpen {
                get { return isOpen; }
                set
                {
                    isOpen = value;
                    if (this.PropertyChanged != null)
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsOpen"));
                }
            }
            public static int[] around_x = { -1, -1, -1, 0, 0, 1, 1, 1 };
            public static int[] around_y = { -1, 0, 1, -1, 1, -1, 0, 1 };
            /*public Block(int x, int y, char flag)
            {
                this.x = x;
                this.y = y;
                this.flag = flag;
            }*/
            public Block()
            {
                this.flag = '0';
                isOpen = false;
            }
            public Block(char flag)
            {
                this.flag = flag;
                isOpen = false;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged(string _property)
            {
                PropertyChangedEventHandler eventhandler = this.PropertyChanged;
                if (null == eventhandler)
                    return;
                eventhandler(this, new PropertyChangedEventArgs(_property));
            }

        }

        private Block[,] pan;
        //新建游戏
        public void NewGame()
        {
            TimeStop();

            currentMine = MineNum;
            this.MineBlock.Text = currentMine.ToString();

            InitPan();
            InitGrid();
            //计时
            TimeStart();


            //test();

        }
        //测试函数
        public void test()
        {
            pan[0, 0].Flag = '0';
            pan[0, 1].Flag = '1';
            pan[0, 2].Flag = '2';
            pan[0, 3].Flag = '3';
            pan[0, 4].Flag = '4';
            pan[0, 5].Flag = '5';
            pan[0, 6].Flag = '6';
            pan[0, 7].Flag = '7';
            pan[0, 8].Flag = '8';
            pan[1, 1].Flag = '9';
            pan[1, 0].Flag = '#';
            pan[2, 0].Flag = '*';
            pan[3, 0].Flag = '?';
            pan[4, 0].Flag = 'F';

        }

        //打印盘
        public void ShowPan()
        {
            Console.WriteLine("打印棋盘");
            for (int i = 0; i < MaxRow; i++)
            {
                for (int j = 0; j < MaxCol; j++)
                {
                    Console.Write(pan[i, j].Flag);
                }
                Console.WriteLine("");

            }
            Console.WriteLine("打印开启情况");
            for (int i = 0; i < MaxRow; i++)
            {
                for (int j = 0; j < MaxCol; j++)
                {
                    Console.Write(pan[i, j].IsOpen ? 1 : 0);
                }
                Console.WriteLine("");

            }

        }
        //初始化盘
        public void InitPan()
        {
            //初始化
            pan = new Block[MaxRow, MaxCol];
            for (int i = 0; i < MaxRow; i++)
                for (int j = 0; j < MaxCol; j++)
                    pan[i, j] = new Block();

            //生成炸弹
            Random random = new Random(System.DateTime.Now.Millisecond);
            int n = 0;

            while (n != MineNum)
            {
                int tempRow = random.Next(MaxRow);
                int tempCol = random.Next(MaxCol);
                if (pan[tempRow, tempCol].Flag != '*')
                {
                    pan[tempRow, tempCol].Flag = '*';
                    //计算周围数字
                    for (int i = 0; i < 8; i++)
                    {
                        if (tempRow + Block.around_x[i] >= 0 && tempRow + Block.around_x[i] < MaxRow && tempCol + Block.around_y[i] >= 0 && tempCol + Block.around_y[i] < MaxCol)
                        {
                            if (pan[tempRow + Block.around_x[i], tempCol + Block.around_y[i]].Flag != '*')
                                pan[tempRow + Block.around_x[i], tempCol + Block.around_y[i]].Flag++;
                        }
                    }
                    n++;
                }
            }
        }
        //初始化UI
        public void InitGrid()
        {
            Mainbody.Children.Clear();
            Mainbody.ColumnDefinitions.Clear();
            Mainbody.RowDefinitions.Clear();

            for (int i = 0; i < MaxRow; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(BLOCK_SIZE);
                Mainbody.RowDefinitions.Add(rd);
            }
            for (int i = 0; i < MaxCol; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(BLOCK_SIZE);
                Mainbody.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < MaxRow; i++)
            {
                for (int j = 0; j < MaxCol; j++)
                {
                    Button bt = new Button();
                    bt.Name = "block_" + i + "_" + j;
                    bt.Click += Grid_MouseLeftButtonDown;
                    bt.MouseRightButtonDown += Bt_MouseRightButtonDown;
                    bt.Style = (Style)this.Resources["BlockButtonStyle"];
                    //显示，绑定到button
                    //bt.Content = pan[i, j].Flag;
                    //Binding EnableBinding = new Binding() { Path = new PropertyPath("IsOpen"), Source = pan[i, j] };
                    //bt.SetBinding(Button.IsEnabledProperty, EnableBinding);
                    bt.DataContext = pan[i, j];

                    Mainbody.Children.Add(bt);
                    Grid.SetRow(bt, i);
                    Grid.SetColumn(bt, j);


                }
            }
        }
        //判断是否赢了
        public bool IsGameOver()
        {
            int mine = 0;
            for (int i = 0; i < MaxRow; i++)
                for (int j = 0; j < MaxCol; j++)
                    if (!pan[i, j].IsOpen ||(pan[i, j].IsOpen&& pan[i, j].Flag == 'Z'))
                        mine++;

            if (mine == MineNum)
                return true;
            else
                return false;
        }

        public void GameOver(int Case)
        {
            if (Case == 0)
            {//输
             //游戏结束
                MessageBox.Show("BOOM!游戏结束！");
                TimeStop();

                //selected.Background = Brushes.Red;
                //全部显示
                for (int i = 0; i < MaxRow; i++)
                    for (int j = 0; j < MaxCol; j++)
                        pan[i, j].IsOpen = true;
                //pan[row, col].IsOpen = true;

                if (MessageBox.Show("是否开始新游戏？", "hh", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewGame();
                }
            }
            else if (Case == 1)
            {
                //游戏胜利   
                MessageBox.Show("You Win！");
                TimeStop();

                //全部显示
                for (int i = 0; i < MaxRow; i++)
                    for (int j = 0; j < MaxCol; j++)
                        pan[i, j].IsOpen = true;
                //pan[row, col].IsOpen = true;

                if (MessageBox.Show("是否开始新游戏？", "hh", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewGame();
                }
            }
        }

        //BFS开格子
        private static Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
        public int BFS_Open(int row, int col)
        {
            if (pan[row, col].Flag == '*')
                return -1;
            if (pan[row, col].Flag == '0')
            {
                q.Clear();
                q.Enqueue(new Tuple<int, int>(row, col));
                pan[row, col].IsOpen = true;
                BFS();
            }
            else {
                pan[row, col].IsOpen = true;
            }
            return 0;
        }
        public void BFS()
        {
            if (q.Count == 0)
                return;
            Tuple<int, int> temp = q.Dequeue();

            for (int i = 0; i < 8; i++)
            {
                if (temp.Item1 + Block.around_x[i] >= 0 && temp.Item1 + Block.around_x[i] < MaxRow && temp.Item2 + Block.around_y[i] >= 0 && temp.Item2 + Block.around_y[i] < MaxCol)
                {
                    Block t = pan[temp.Item1 + Block.around_x[i], temp.Item2 + Block.around_y[i]];
                    if (char.IsLetter(t.Flag) && t.Flag != 'Z')
                        SetFlag(temp.Item1 + Block.around_x[i], temp.Item2 + Block.around_y[i]);
                    if (t.Flag != '*' && !t.IsOpen)
                    {
                        t.IsOpen = true;
                        if (t.Flag == '0')
                            q.Enqueue(new Tuple<int, int>(temp.Item1 + Block.around_x[i], temp.Item2 + Block.around_y[i]));
                    }
                }
            }
            BFS();
        }

        //单击事件
        private void Block_Click(object sender)
        {
            if (!isDouble)
            {
                Button selected = sender as Button;
                //MessageBox.Show("Click!"+ selected.Name);
                //单击时执行的代码
                ShowPan();
                Console.WriteLine("Click!" + selected.Name);

                int row, col;
                if (SlectedIndex(selected.Name, out row, out col))
                {
                    //已经打开的无效（包括插旗子，为了防止误点）
                    if (!pan[row, col].IsOpen)
                    {
                        if (BFS_Open(row, col) == -1)
                        {
                            GameOver(0);
                        }
                        else if (IsGameOver())
                        {
                            GameOver(1);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("!!!下标出错！！！");
                }


            }
        }
        //双击事件
        private void Block_DoubleClick(object sender)
        {
            if (isDouble)
            {
                Button selected = sender as Button;
                //MessageBox.Show("DoubleClick!" + selected.Name);
                ShowPan();
                Console.WriteLine("DoubleClick!" + selected.Name);

                int row, col;
                if (SlectedIndex(selected.Name, out row, out col))
                {
                    if (!pan[row, col].IsOpen)
                    {
                        isDouble = false;
                        Block_Click(sender);
                    }
                    else if (char.IsDigit(pan[row, col].Flag))
                    {
                        //MessageBox.Show("双击！");
                        //当附近的地雷都可以确定时，直接打开
                        int right=0;//标记且正确
                        bool wrong = false;//标记且错误
                        int mine_around = pan[row, col].Flag - '0';//周围雷数
                        for (int i = 0; i < 8; i++)
                            if (row + Block.around_x[i] >= 0 && row + Block.around_x[i] < MaxRow && col + Block.around_y[i] >= 0 && col + Block.around_y[i] < MaxCol)
                            {//不出界
                                if (pan[row + Block.around_x[i], col + Block.around_y[i]].Flag == 'Z')
                                    right++;                
                                else if (char.IsLetter(pan[row + Block.around_x[i], col + Block.around_y[i]].Flag)|| pan[row + Block.around_x[i], col + Block.around_y[i]].Flag == '*')
                                {//没有标记的雷或标记错的数字都会爆
                                    wrong = true;
                                    break;
                                }
                            }
                        if (wrong)
                        {
                            GameOver(0);
                        }
                        else if (right == mine_around)
                        {//开雷了
                            for (int i = 0; i < 8; i++)
                                if (row + Block.around_x[i] >= 0 && row + Block.around_x[i] < MaxRow && col + Block.around_y[i] >= 0 && col + Block.around_y[i] < MaxCol)
                                {//不出界
                                    if(char.IsDigit(pan[row + Block.around_x[i], col + Block.around_y[i]].Flag)&& !pan[row + Block.around_x[i], col + Block.around_y[i]].IsOpen)
                                    {
                                        //打开附近的
                                        BFS_Open(row + Block.around_x[i], col + Block.around_y[i]);
                                    }
                                }
                            if (IsGameOver())
                            {
                                GameOver(1);
                            }
                        }
                        else
                        {//什么事也没发生...

                        }
                    }
                    else{//旗子不用管
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("!!!下标出错！！！");
                }
            }
        }
        //右键事件（插旗）
        private void Block_RightClick(object sender)
        {
            Button selected = sender as Button;
            //单击时执行的代码
            //MessageBox.Show("右键" + selected.Name);
            Console.WriteLine("RightClick!" + selected.Name);

            ShowPan();

            int row, col;
            if (SlectedIndex(selected.Name, out row, out col))
            {
                SetFlag(row, col);
                if (IsGameOver())
                {
                    GameOver(1);
                }
            }
            else
            {
                Console.WriteLine("!!!下标出错！！！");
            }
        }

        //操作旗子
        private static int currentMine = 0;
        private void SetFlag(int row,int col)
        {
            if (char.IsLetter(pan[row, col].Flag))
            {
                //插了旗子,取消插旗
                if (pan[row, col].Flag == 'Z')
                {//改变flag为插旗后的字母0->A 1->B ... 
                    pan[row, col].Flag = '*';
                }
                else
                {//雷的话就变成Z
                    pan[row, col].Flag = (char)(pan[row, col].Flag-'A'+'0');
                }
                currentMine++;
                this.MineBlock.Text = currentMine.ToString();
                pan[row, col].IsOpen = false;
            }
            else if (!pan[row, col].IsOpen)
            {//插了旗子肯定翻开了，所以没打开的都没插旗子
                if (char.IsDigit(pan[row, col].Flag))
                {//改变flag为插旗后的字母0->A 1->B ... 
                    pan[row, col].Flag = (char)(pan[row, col].Flag - '0' + 'A');
                }
                else if (pan[row, col].Flag == '*')
                {//雷的话就变成Z
                    pan[row, col].Flag = 'Z';
                }
                currentMine--;
                this.MineBlock.Text = currentMine.ToString();
                pan[row, col].IsOpen = true;
            }
            else
            {//已经点开了的数字和空地不能插旗子了
                return;
            }
            
        }

        #endregion

        //新建一个窗口
        private void Setting_Button_Click(object sender, RoutedEventArgs e)
        {
            setting setwin = new setting();
            setwin.ShowDialog();
            NewGame();
        }

 
        
    }
}

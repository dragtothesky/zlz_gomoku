using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Gomoku_ChessBoard
{
    public partial class Form1 : Form
    {
        public static int cellSize = 50;         //pixel
        public static int RowNum = 9;
        public static int ColumnNum = 9;
        public static int boardOffsetX = 50;
        public static int boardOffsetY = 20;
        List<BoardPoint> listAI = new List<BoardPoint>();
        List<BoardPoint> listPlayer = new List<BoardPoint>();
        List<BoardPoint> listBoth = new List<BoardPoint>();
        List<BoardPoint> listAll = new List<BoardPoint>();
        // List<BoardPoint> listBlank = new List<BoardPoint>();

        double styleRatio = 1;       //进攻的系数 大于1 进攻型，  小于1 防守型
        int DEPTH = 3;          //搜索深度
        int cut_count;
        int search_count;

        BoardPoint nextPoint = new BoardPoint(0, 0);

        //得分形状
        List<shapeScoreStruct> shapeScoreList = new List<shapeScoreStruct>(new shapeScoreStruct[]
            {
                new shapeScoreStruct(50, new int[] { 0, 1, 1, 0, 0 }),
                 new shapeScoreStruct(50,new int[] { 0, 0, 1, 1, 0 }),
                  new shapeScoreStruct(200, new int[] { 1, 1, 0, 1, 0 }),
                  new shapeScoreStruct(200, new int[] { 0, 1, 0, 1, 1 }),
                  new shapeScoreStruct(200, new int[] { 0, 1, 1, 0, 1 }),
                  new shapeScoreStruct(200, new int[] { 1, 0, 1, 1, 0 }),
                   new shapeScoreStruct(500, new int[] { 0, 0, 1, 1, 1 }),
                    new shapeScoreStruct(500, new int[] { 1, 1, 1, 0, 0 }),
                     new shapeScoreStruct(5000, new int[] { 0, 1, 1, 1, 0 }),
                      new shapeScoreStruct(5000, new int[] { 0, 1, 0,1, 1, 0 }),
                       new shapeScoreStruct(5000, new int[] { 0, 1, 1, 0, 1,0 }),
                        new shapeScoreStruct(5000, new int[] { 1, 1, 1, 0, 1 }),
                         new shapeScoreStruct(5000, new int[] { 1, 1, 0, 1, 1 }),
                          new shapeScoreStruct(5000, new int[] { 1, 0, 1, 1, 1 }),
                           new shapeScoreStruct(5000, new int[] { 1, 1, 1, 1, 0 }),
                            new shapeScoreStruct(5000, new int[] { 0, 1, 1, 1, 1 }),
                             new shapeScoreStruct(50000, new int[] { 0, 1, 1, 1,1, 0 }),
                              new shapeScoreStruct(99999999, new int[] { 1, 1, 1, 1, 1 })
            });
        public Form1()
        {
            // CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            // panel11 = panel1;
            //  panel1.Controls.Add(new WhitePiece(boardOffsetX + cellSize / 2 + cellSize * 2, boardOffsetY + cellSize / 2 + cellSize * 2));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxDpt.Text = DEPTH.ToString();
            for (int i = 0; i < ColumnNum + 1; i++)
            {
                for (int j = 0; j < RowNum + 1; j++)
                {
                    listAll.Add(new BoardPoint(i, j));
                }
            }
            panel1.Controls.Add(new BlackPiece(boardOffsetX - cellSize / 2 + cellSize * 4, boardOffsetY - cellSize / 2 + cellSize * 4));            //黑子先下一颗。此处有个bug未解决：只有下子了棋盘才会显现。
            listAI.Add(new BoardPoint(4, 4));
            listBoth.Add(new BoardPoint(4, 4));
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (!File.Exists(Application.StartupPath + "\\bg2.png"))
                {
                    MessageBox.Show("请把素材中的bg2.png等图片文件放到可执行程序相同文件夹中,并重新启动程序！", "提示"); Application.Exit();
                }
                Graphics g = panel1.CreateGraphics(); // 获取一个Graphics对象
                //  g.Clear(Color.Coral);  //用珊瑚色清除窗体
                Font font1 = new Font("黑体", 25, FontStyle.Regular);
                int s = (int)(font1.Size / 0.6);
                int s2 = (int)font1.Height;
                // Image img = Image.FromFile(Application.StartupPath + "\\bg.png");
                //g.DrawImage(Image.FromFile(Application.StartupPath + @"\" + "bg.png"), 0, 0, cellSize * ColumnNum + 20, cellSize * RowNum + 60);  //绘制背景图
                g.DrawRectangle(new Pen(Color.Black, 3), new Rectangle(new Point(boardOffsetX, boardOffsetY), new Size(cellSize * ColumnNum, cellSize * RowNum))); //绘制框图
                Pen pen = new Pen(Color.Black, 1);

                //绘制水平线
                for (int i = 1; i < RowNum; i++)
                {
                    g.DrawLine(pen, new Point(boardOffsetX, boardOffsetY + i * cellSize), new Point(boardOffsetX + cellSize * ColumnNum, boardOffsetY + i * cellSize));
                }

                //绘制垂直线
                for (int i = 1; i < ColumnNum; i++)
                {
                    g.DrawLine(pen, new Point(boardOffsetX + i * cellSize, boardOffsetY), new Point(boardOffsetX + i * cellSize, boardOffsetY + cellSize * RowNum));
                }
                g.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBoxDpt_TextChanged(object sender, EventArgs e)
        {
            int buffer;
            if (int.TryParse(textBoxDpt.Text, out buffer))
                DEPTH = buffer;
        }

        //player down
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (e.X < boardOffsetX - cellSize / 2 || e.X > boardOffsetX + cellSize / 2 + cellSize * ColumnNum || e.Y < boardOffsetY - cellSize / 2 || e.Y > boardOffsetY + cellSize / 2 + cellSize * RowNum)
                return;
            double x = e.X;
            double y = e.Y;
            x = (x - boardOffsetX) / cellSize;
            y = (y - boardOffsetY) / cellSize;
            x = Math.Round(x, 0);
            y = Math.Round(y, 0);
            listPlayer.Add(new BoardPoint((int)x, (int)y));
            listBoth.Add(new BoardPoint((int)x, (int)y));
            x = x * cellSize + boardOffsetX;
            y = y * cellSize + boardOffsetY;
            panel1.Controls.Add(new WhitePiece(Convert.ToInt32(x) - cellSize / 2, Convert.ToInt32(y) - cellSize / 2));
            Application.DoEvents();
            if (Evaluate(false) >= 9999999)         //似乎不严谨
            {
                MessageBox.Show("You Win！");
                return;
            }
            ComputerDown();
        }

        //computer down
        private void ComputerDown()
        {
            //Random r = new Random();
            //int x = r.Next(1, 14);
            //int y = r.Next(1, 14);
            //x = x * cellSize + 10;
            //y = y * cellSize + 10;
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            BoardPoint mptTmp = AIResult();
            panel1.Controls.Add(new BlackPiece(mptTmp.x_screen_corner, mptTmp.y_screen_corner));
            listAI.Add(new BoardPoint(mptTmp.XNum, mptTmp.YNum));
            listBoth.Add(new BoardPoint(mptTmp.XNum, mptTmp.YNum));
            textBoxTm.Text = (watch1.ElapsedMilliseconds / 1000).ToString();
            textBoxCut.Text = cut_count.ToString();
            textBoxSch.Text = search_count.ToString();
            if (Evaluate(true) >= 9999999)
            {
                MessageBox.Show("You Lose！HaHaHaHaHa");
                return;
            }
        }

        #region 算法
        private BoardPoint AIResult()
        {
            cut_count = 0;
            search_count = 0;
            int finalS = Minmax(true, DEPTH, -99999999, 99999999);
            return nextPoint;
        }

        //极小化极大、αβ剪枝
        private int Minmax(bool isAI, int depth, int alpha, int beta)
        {
            //if (depth == 0)
            if (depth == 0 || GameWin(listAI) || GameWin(listPlayer))         //depth==0在前
            {
                return Evaluate(isAI);
            }
            List<BoardPoint> blankTmp = new List<BoardPoint>(listAll);
            //listBlank = blankTmp;
            foreach (BoardPoint item in listBoth)
            {
                int indexTmp = blankTmp.FindIndex(new Predicate<BoardPoint>((BoardPoint bpt) => { if (bpt.XNum == item.XNum && bpt.YNum == item.YNum) return true; else return false; }));
                if (indexTmp == -1)
                    MessageBox.Show("listBlank或listAll异常");
                else
                {
                    blankTmp.RemoveAt(indexTmp);
                }
                //listBlank = listAll.Except(listBoth).ToList();            //判断的是索引类型的索引
            }
            if (depth > 1)
                OrderList(blankTmp, isAI);           //排序优化;剪枝力度对排序敏感
            else
                OrderList(blankTmp);
            for (int i = blankTmp.Count - 1; i >= 0; i--)
            {
                search_count += 1;
                if (isAI)
                    listAI.Add(blankTmp[i]);
                else
                    listPlayer.Add(blankTmp[i]);
                listBoth.Add(blankTmp[i]);
                int value = -Minmax(!isAI, depth - 1, -beta, -alpha);
                if (isAI)
                    listAI.Remove(blankTmp[i]);
                else
                    listPlayer.Remove(blankTmp[i]);
                listBoth.Remove(blankTmp[i]);
                if (value > alpha)
                {
                    if (depth == DEPTH)
                    {
                        nextPoint = blankTmp[i];
                    }
                    //alpha + beta剪枝点
                    if (value >= beta)
                    {
                        cut_count += 1;
                        return beta;
                    }
                    alpha = value;
                }
            }
            //GC.Collect();
            return alpha;
        }

        private bool GameWin(List<BoardPoint> list)          
        {
            for (int i = 0; i < ColumnNum; i++)
            {
                for (int j = 0; j < RowNum; j++)
                {
                    if (j < (RowNum - 4) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i && pt.YNum == j) return true; else return false;
                    })) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i && pt.YNum == j + 1) return true; else return false;
                    }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i && pt.YNum == j + 2) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i && pt.YNum == j + 3) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i && pt.YNum == j + 4) return true; else return false;
                      }
                    ))
                    )
                    { return true; }
                    else if (i < (ColumnNum - 4) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i && pt.YNum == j) return true; else return false;
                    })) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i + 1 && pt.YNum == j) return true; else return false;
                    }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 2 && pt.YNum == j) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 3 && pt.YNum == j) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 4 && pt.YNum == j) return true; else return false;
                      }
                    ))
                    )
                    { return true; }
                    else if (i < (ColumnNum - 4) && j < (RowNum - 4) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i && pt.YNum == j) return true; else return false;
                    })) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i + 1 && pt.YNum == j + 1) return true; else return false;
                    }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 2 && pt.YNum == j + 2) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 3 && pt.YNum == j + 3) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 4 && pt.YNum == j + 4) return true; else return false;
                      }
                    ))
                    )
                    { return true; }
                    else if (i < (ColumnNum - 4) && j > 3 &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i && pt.YNum == j) return true; else return false;
                    })) &&
                    list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (pt.XNum == i + 1 && pt.YNum == j - 1) return true; else return false;
                    }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 2 && pt.YNum == j - 2) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 3 && pt.YNum == j - 3) return true; else return false;
                      }
                    )) &&
                      list.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                      {
                          if (pt.XNum == i + 4 && pt.YNum == j - 4) return true; else return false;
                      }
                    ))
                    )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private int Evaluate(bool isAI)
        {
            double totalScore = 0;
            List<BoardPoint> myList;
            List<BoardPoint> enemyList;
            if (isAI)
            {
                myList = new List<BoardPoint>(listAI);
                enemyList = new List<BoardPoint>(listPlayer);
            }
            else
            {
                myList = new List<BoardPoint>(listPlayer);
                enemyList = new List<BoardPoint>(listAI);
            }
            List<MaxScoreStruct> MaxScoreStructArr = new List<MaxScoreStruct>();
            List<MaxScoreStruct> MaxScoreStructArr_enemy = new List<MaxScoreStruct>();
            int myScore = 0;
            int enemyScore = 0;
            foreach (BoardPoint pt in myList)
            {
                myScore += CalScore(pt, 0, 1, enemyList, myList, MaxScoreStructArr);
                myScore += CalScore(pt, 1, 0, enemyList, myList, MaxScoreStructArr);
                myScore += CalScore(pt, 1, 1, enemyList, myList, MaxScoreStructArr);
                myScore += CalScore(pt, -1, 1, enemyList, myList, MaxScoreStructArr);
            }
            foreach (BoardPoint pt in enemyList)
            {
                enemyScore += CalScore(pt, 0, 1, myList, enemyList, MaxScoreStructArr_enemy);
                enemyScore += CalScore(pt, 1, 0, myList, enemyList, MaxScoreStructArr_enemy);
                enemyScore += CalScore(pt, 1, 1, myList, enemyList, MaxScoreStructArr_enemy);
                enemyScore += CalScore(pt, -1, 1, myList, enemyList, MaxScoreStructArr_enemy);
            }
            totalScore = myScore - enemyScore * styleRatio;
            return (int)totalScore;

        }


        //疑似仍然存在bug
        private int CalScore(BoardPoint point, int direct_x, int direct_y, List<BoardPoint> enemylist, List<BoardPoint> mylist, List<MaxScoreStruct> maxScoreStruct_All)
        {
            int addScore = 0;
            List<int> pos = new List<int>(6);
            MaxScoreStruct maxScoreShape = new MaxScoreStruct();
            foreach (MaxScoreStruct item in maxScoreStruct_All)         //重复的排除计分
            {
                foreach (BoardPoint pt in item.Type)
                {
                    if (pt.Equals(point) && direct_x == item.directionX && direct_y == item.directionY)
                    {
                        return 0;
                    }
                }
            }

            //在落子点左右方向上循环查找得分形状.六颗子一验，注意对角边上正好只可以摆五颗子的情况
            for (int offset = -5; offset < 1; offset++)
            {
                //两颗子及以上出界的直接排除
                if (point.XNum + (offset + 1) * direct_x < 0 || point.XNum + (offset + 1) * direct_x > ColumnNum + 1 || point.XNum + (offset + 4) * direct_x < 0 || point.XNum + (offset + 4) * direct_x > ColumnNum + 1 || point.YNum + (offset + 1) * direct_y < 0 || point.YNum + +(offset + 4) * direct_y > RowNum + 1)
                    continue;
                //List<int> pos = new List<int>();
                pos.Clear();
                for (int i = 0; i < 6; i++)
                {
                    bool isExist;
                    if (point.XNum + (i + offset) * direct_x < 0 || point.XNum + (i + offset) * direct_x > ColumnNum + 1 || point.YNum + (i + offset) * direct_y < 0 || point.YNum + (i + offset) * direct_y > RowNum + 1)
                    {
                        pos.Add(3);         //界外
                        continue;
                    }
                    isExist = enemylist.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                    {
                        if (point.XNum + (i + offset) * direct_x == pt.XNum && point.YNum + (i + offset) * direct_y == pt.YNum)
                        {
                            pos.Add(2);
                            return true;
                        }
                        else return false;
                    }));
                    if (!isExist)
                    {
                        isExist = mylist.Exists(new Predicate<BoardPoint>((BoardPoint pt) =>
                        {
                            if (point.XNum + (i + offset) * direct_x == pt.XNum && point.YNum + (i + offset) * direct_y == pt.YNum)
                            {
                                pos.Add(1);
                                return true;
                            }
                            else return false;
                        }));
                        if (!isExist)
                        {
                            pos.Add(0);
                        }
                    }
                }
                int[] shape5_tmp = new int[] { pos[0], pos[1], pos[2], pos[3], pos[4] };
                int[] shape5_tmp2 = new int[] { pos[1], pos[2], pos[3], pos[4], pos[5] };
                // shape5_tmp.AddRange(new int[] { pos[0], pos[1], pos[2], pos[3], pos[4] });
                int[] shape6_tmp = new int[] { pos[0], pos[1], pos[2], pos[3], pos[4], pos[5] };
                //shape6_tmp.AddRange(new int[] { pos[0], pos[1], pos[2], pos[3], pos[4], pos[5] });
                foreach (shapeScoreStruct shapeScore in shapeScoreList)
                {
                    if (shape5_tmp.SequenceEqual(shapeScore.shape) || shape5_tmp2.SequenceEqual(shapeScore.shape) || shape6_tmp.SequenceEqual(shapeScore.shape))           //值、顺序、长度都相等
                    {
                        if (shapeScore.score > maxScoreShape.score)
                        {
                            maxScoreShape.score = shapeScore.score;
                            maxScoreShape.Type = new List<BoardPoint>(new BoardPoint[]
                            {
                                new BoardPoint(point.XNum + (offset + 0) * direct_x, point.YNum + (offset + 0) * direct_y),
                                new BoardPoint(point.XNum + (offset + 1) * direct_x, point.YNum + (offset + 1) * direct_y),
                                new BoardPoint(point.XNum + (offset + 2) * direct_x, point.YNum + (offset + 2) * direct_y),
                                new BoardPoint(point.XNum + (offset + 3) * direct_x, point.YNum + (offset + 3) * direct_y),
                                new BoardPoint(point.XNum + (offset + 4) * direct_x, point.YNum + (offset + 4) * direct_y),
                                new BoardPoint(point.XNum + (offset + 5) * direct_x, point.YNum + (offset + 5) * direct_y)
                            });
                            maxScoreShape.directionX = direct_x;
                            maxScoreShape.directionY = direct_y;
                        }
                    }
                }

            }
            if (maxScoreShape.Type != null)
            {
                //foreach (MaxScoreStruct item in maxScoreStruct_All)           //同一点多个得分形状的附加分，似乎没有必要
                //{
                //    foreach (BoardPoint pt1 in item.Type)
                //    {
                //        foreach (BoardPoint pt2 in maxScoreShape.Type)
                //        {
                //            if (pt1.Equals(pt2) && maxScoreShape.score > 10 && item.score > 10)
                //            {
                //                addScore += item.score + maxScoreShape.score;
                //            }

                //        }
                //    }
                //}
                maxScoreStruct_All.Add(maxScoreShape);
            }
            return addScore + maxScoreShape.score;

        }

        //最后落子点周围优先
        private void OrderList(List<BoardPoint> blankList)
        {
            BoardPoint lastPt = listBoth.Last();         //最后落子点
            List<BoardPoint> neighborhood = new List<BoardPoint>();
            for (int itemnum = blankList.Count - 1; itemnum >= 0; itemnum--)
            {
                if (Math.Abs(blankList[itemnum].XNum - lastPt.XNum) < 3 && Math.Abs(blankList[itemnum].YNum - lastPt.YNum) < 3)
                {
                    neighborhood.Add(new BoardPoint(blankList[itemnum].XNum, blankList[itemnum].YNum));
                    blankList.RemoveAt(itemnum);
                }
            }
            blankList.AddRange(neighborhood);           //放到末尾
        }

        //根据落子后场上形势排序，权衡使用
        private void OrderList(List<BoardPoint> blankList, bool isAI)
        {

            List<BoardPoint> myList;
            List<int> listblank_Score = new List<int>();
            if (isAI)
                myList = listAI;
            else
                myList = listPlayer;
            for (int i = 0; i < blankList.Count; i++)           //各点带来的分数
            {
                myList.Add(blankList[i]);
                listBoth.Add(blankList[i]);
                listblank_Score.Add(Evaluate(isAI));
                myList.Remove(blankList[i]);
                listBoth.Remove(blankList[i]);
            }
            for (int itemnum = 0; itemnum < blankList.Count - 1; itemnum++)         //冒泡排序
            {
                bool isDone = true;         
                for (int j = 0; j < blankList.Count - itemnum - 1; j++)
                {
                    if (listblank_Score[j] > listblank_Score[j + 1])
                    {
                        BoardPoint tmpPt = new BoardPoint(blankList[j].XNum, blankList[j].YNum);
                        blankList[j] = blankList[j + 1];
                        blankList[j + 1] = tmpPt;
                        int scoreTmp = listblank_Score[j];
                        listblank_Score[j] = listblank_Score[j + 1];
                        listblank_Score[j + 1] = scoreTmp;
                        isDone = false;
                    }
                }
                if (isDone)         //为真说明已排好序
                {
                    break;
                }
            }

        }

        //分数、位置、方向
        struct MaxScoreStruct
        {
            public int score;
            public List<BoardPoint> Type;
            public int directionX;
            public int directionY;
        }

        //分数、得分形状
        struct shapeScoreStruct
        {
            public int score;
            // public List<int> shape;
            public int[] shape;

            public shapeScoreStruct(int score, int[] shape)
            {
                this.score = score;
                this.shape = shape;
            }
        }
        #endregion
    }

    //棋子抽象类
    abstract class Piece : PictureBox
    {
        // 构造函数，传入坐标值x，y
        public Piece(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.BackColor = Color.Transparent; // 背景颜色
            this.Location = new Point(x, y); // 位置x，y是一个Point的实例对象
            this.Size = new Size(Form1.cellSize, Form1.cellSize);// 大小
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Piece)) return false;
            Piece bpt = (Piece)obj;
            return X == bpt.X && Y == bpt.Y;
            //return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            int hashcode = X.GetHashCode();
            if (hashcode != Y.GetHashCode())
                hashcode ^= Y.GetHashCode();         //异或
            return hashcode;
            // return base.GetHashCode();
        }

    }
    class BlackPiece : Piece
    {
        public BlackPiece(int x, int y)
            : base(x, y)
        {
            Image img = Properties.Resources.heizi;
            this.Image = img;
            this.SizeMode = PictureBoxSizeMode.StretchImage;

        }
    }

    class WhitePiece : Piece
    {
        public WhitePiece(int x, int y)
            : base(x, y)
        {
            this.Image = Properties.Resources.baizi; // 素材
            this.SizeMode = PictureBoxSizeMode.StretchImage;

        }
    }

    class BoardPoint
    {
        public int x_screen_corner;         //对应棋子的左上角坐标（屏幕坐标）
        public int y_screen_corner;

        public BoardPoint(int xNum, int yNum)
        {
            this.XNum = xNum;
            this.YNum = yNum;
        }

        public int XNum         //棋盘坐标
        {
            get { return (x_screen_corner + Form1.cellSize / 2 - Form1.boardOffsetX) / Form1.cellSize; }
            set { x_screen_corner = (value) * Form1.cellSize - Form1.cellSize / 2 + Form1.boardOffsetX; }
        }
        public int YNum
        {
            get { return (y_screen_corner + Form1.cellSize / 2 - Form1.boardOffsetY) / Form1.cellSize; }
            set { y_screen_corner = (value) * Form1.cellSize - Form1.cellSize / 2 + Form1.boardOffsetY; }
        }
        public override bool Equals(Object obj)
        {
            if (!(obj is BoardPoint)) return false;
            BoardPoint bpt = (BoardPoint)obj;
            return XNum == bpt.XNum && YNum == bpt.YNum;
        }
        public override int GetHashCode()
        {
            int hashcode = XNum.GetHashCode();
            if (hashcode != YNum.GetHashCode())
                hashcode ^= YNum.GetHashCode();         //异或
            return hashcode;
            //return XNum * 100 + YNum;         //另一种映射
            // return base.GetHashCode();
        }

    }

}

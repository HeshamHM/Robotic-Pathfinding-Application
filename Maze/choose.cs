using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class choose : Form
    {
        Random rand;//random number
        Button [,]button;// to save button as 2D array
        int rowRObOT = 0;// the robot row index
        int colRObOT = 0;// the robot col index
        int rowStop = 0;// the robot row stop index
        int colStop = 0;// the robot col stop index
        bool flagstop = false; 
        bool flagBlock = false;
        Graph gp;
        Queue<Node> move;
        bool start = false;
        int count = 0;
// create 2d buttons
        Button btn(int i,int j)
        {
            Button B = new Button();
            B.Name = i.ToString()+" "+j.ToString();
            B.Width = 45;
            B.Height = 25;
         //  B.Text =i.ToString()+"," + j.ToString();
            B.Click += b_Click;
            B.Parent = flowLayoutPanel1;
            B.FlatStyle= FlatStyle.Flat;
            B.BackColor = Color.White;
            B.Enabled = false;
            return B;
        }
        // class to solve our problem
        class Graph
        {
            // private int node;
            // private List<int>[] adj;
            private int[,] grid;
            private bool[,] vis;
            int n, m;
            public Graph(int x, int y)
            {
                n = x;
                m = y;
                vis = new bool[n, m];
                grid = new int[n, m];
            }
            public void setBolck(int i,int j)
            {
               vis[i, j] = true;
            }
            public Stack<Node> Find_Shortest_Path_By_BFS(int start_row, int start_col, int end_row, int end_col)
            {
                bool falg = false;
                Stack<Node> stack = new Stack<Node>();
                Queue<KeyValuePair<int, int>> st = new Queue<KeyValuePair<int, int>>();
                st.Enqueue(new KeyValuePair<int, int>(start_row, start_col));
                vis[start_row, start_col] = true;
                int[] dx = { -1, 1, 0, 0, 1, -1, 1, -1 };
                int[] dy = { 0, 0, -1, 1, 1, -1, -1, 1 };
                int[,] dis = new int[n, m];
                Node[,] way = new Node[n, m];
                while (st.Count() != 0)
                {
                    KeyValuePair<int, int> current = new KeyValuePair<int, int>();
                    current = st.Dequeue();
                    for (int i = 0; i < 8; i++)
                    {
                        int new_x = current.Key + dx[i];
                        int new_y = current.Value + dy[i];
                        if (new_x >= 0 && new_x < n && new_y >= 0 && new_y < m)
                        {
                            if (!vis[new_x, new_y])
                            {
                                vis[new_x, new_y] = true;
                                dis[current.Key, current.Value] = dis[new_x, new_y] + 1;
                                way[new_x, new_y] = new Node();
                                way[new_x, new_y].x = current.Key;
                                way[new_x, new_y].y = current.Value;
                                st.Enqueue(new KeyValuePair<int, int>(new_x, new_y));
                                if (new_x == end_row && new_y == end_col)
                                {
                                    falg = true;
                                    stack.Push(new Node { x = new_x, y = new_y });
                                    break;
                                }
                            }
                        }
                    }

                }


                while (falg)
                {
                    Node temp = new Node();
                    temp.x = way[end_row, end_col].x;
                    temp.y = way[end_row, end_col].y;
                    if (way[end_row, end_col].x == start_row && way[end_row, end_col].y == start_col)
                    {
                        break;
                    }
                    stack.Push(temp);
                    end_row = temp.x;
                    end_col = temp.y;
                }
                //MessageBox.Show("End: "+start);
                //return que.Count;
                return stack;
            }
        }
        // class to save x and y axis
        public class Node
        {
                public int x { set; get; }
                public int y { set; get; }
        }
        
            public choose()
            {
              InitializeComponent();
            }

        private void choose_Load(object sender, EventArgs e)
        {

            gp = new Graph(20, 20);
            button = new Button[20, 20];
            // create implace buttons 20 *20
            for(int i=0;i<20;i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    button[i, j] = btn(i, j);
                    flowLayoutPanel1.Controls.Add(button[i,j]);
                }
            }
            guna2Button1.Hide();
            guna2Button2.Hide();
           // guna2Button3.Hide();
            guna2Button4.Hide();
            guna2Button5.Hide();
        }
        // when we click in any buttons in 2D array
        void b_Click(object sender,EventArgs e)
        {
            Button butt = (Button)sender;
            if(flagstop==true)
            {
               /// to set the stop icon
                string c = butt.Name.ToString();
                int size = butt.Name.Length;
                string temp = "";
                int count = 0;
                foreach (var i in c)
                {
                    count++;
                    if (i == ' ')
                    {
                        rowStop = int.Parse(temp.ToString());
                        temp = "";
                        continue;
                    }
                    temp += i;

                }

               colStop= int.Parse(temp.ToString());
                if ((rowStop== rowRObOT && colStop==colRObOT))
                {
                    MessageBox.Show("Sorry can not set block here", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        button[i, j].Enabled = true;
                    }
                }
                
                    // MessageBox.Show("   " + rowStop + "  " + colStop);
                    flagstop = false;
                    butt.BackgroundImage = Image.FromFile("icons8-stop-sign-80.png");
                    butt.BackgroundImageLayout = ImageLayout.Zoom;
                    guna2Button2.Show();
                    guna2Button1.Hide();
                }  
            }
            else if(flagBlock == true)
            {
               // to set block icon
                string c = butt.Name.ToString();
                int size = butt.Name.Length;
                string temp = "";
                int x=0, y=0;
                    int count = 0;
                    foreach (var i in c)
                    {
                        count++;
                        if (i ==' ')
                        {
                           x= int.Parse(temp.ToString());
                            temp = "";
                            continue;
                        }
                        temp += i;

                    }
                    y = int.Parse(temp.ToString());
                if ((x == rowStop && y == colStop) || (x == rowRObOT && y == colRObOT))
                {
                    MessageBox.Show("Sorry can not set block here", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    butt.BackgroundImage = Image.FromFile("icons8-block-80.png");
                    butt.BackgroundImageLayout = ImageLayout.Zoom;
                    gp.setBolck(x, y);

                }
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        button[i, j].Enabled = true;
                    }
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            rand = new Random();
            rowRObOT = rand.Next() % (20 - 1);
            colRObOT = rand.Next() % (20 - 1);
            button[rowRObOT, colRObOT].BackgroundImage = Image.FromFile("icons8-robot-2-96.png");
            button[rowRObOT, colRObOT].BackgroundImageLayout = ImageLayout.Zoom;
            guna2Button3.Enabled = false;
            guna2Button1.Show();
            guna2Button3.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            flagstop = true;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    button[i, j].Enabled=true;
                }
            }
            guna2Button1.Enabled = false;   
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (flagBlock == false)
            {
                flagBlock = true;
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        button[i, j].Enabled = true;
                    }
                }
            }
            else
            {
                flagBlock = false;
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        button[i, j].Enabled = true;
                    }
                }
            }
            guna2Button2.Enabled=false;
            guna2Button2.Hide();
            guna2Button4.Show();
        }


        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
          // solve our problem
            var que = gp.Find_Shortest_Path_By_BFS(rowRObOT, colRObOT, rowStop, colStop);
            if (que.Count == 0)
            {
                MessageBox.Show("there is no way to move", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                guna2Button4.Hide();
                guna2Button5.Show();
                start = false;
            }
            else
            {
                move = new Queue<Node>();
                while (que.Count != 0)
                {

                    var see = que.Pop();
                    move.Enqueue(see);
                    //button[see.x, see.y].ForeColor = Color.DarkGreen;
                    button[see.x, see.y].FlatAppearance.BorderColor = Color.DarkGreen;
                    button[see.x, see.y].FlatAppearance.BorderSize = 2;

                   

                }
                MessageBox.Show("End", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                start = true;
            }
            guna2Button1.Enabled = false;
            guna2Button3.Enabled = false;
            guna2Button4.Enabled = false;
            guna2Button2.Enabled = false;
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    button[i, j].Enabled = false;
                }
            }
          
           

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // restart
            choose ch = new choose();
            ch.Show();
            this.Hide();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //move the robot
            if(start)
            {
                count++;
                if(count==5)
                {
                    if (move.Count > 0)
                    {
                        var see = move.Dequeue();
                        if (move.Count >=1)
                        {
                            button[rowRObOT, colRObOT].BackgroundImage = button[see.x, see.y].BackgroundImage;
                        }
                        rowRObOT = see.x;
                        colRObOT = see.y;
                        button[see.x, see.y].BackgroundImage = Image.FromFile("icons8-robot-2-96.png");
                        button[see.x, see.y].BackgroundImageLayout = ImageLayout.Zoom;
                        count = 0;
                    }
                else
                    {
                        guna2Button4.Hide();
                        guna2Button5.Show();
                        start = false;
                    }
                }
            }
        }
    }
}

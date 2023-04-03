using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldOfWarships
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateGrid(dataGridView1);
            CreateGrid(dataGridView2);
            radioButton1.Checked = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            HideLabels();
        }

        Player player;
        Enemy enemy;

        int fieldSize = 11;

        List<int> ShipList= new List<int>();

        int Ship4 = 1, Ship2 = 3, Ship3 = 2, Ship1 = 4;

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            dataGridView2.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = true;
            HideAllStart();
            ShowAndUpdate();
        }

        /// <summary>
        /// Нужно сделать проверку на количество кораблей.
        /// Если кораблей 0 - Делаем кнопку начать бой доступной
        /// </summary>

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            dataGridView2.Enabled = true;
            player = new Player();
            enemy = new Enemy(dataGridView1);
            button3.Enabled = true;
            dataGridView1.Enabled = false;
            dataGridView1.ClearSelection();

            HideLabels();

            if (radioButton2.Checked)
            {
                enemy.DoStep();
                ShowFields();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.White && e.RowIndex > 0 && e.ColumnIndex > 0)
            {
                player.Action(e.RowIndex - 1, e.ColumnIndex - 1);
            }
            else
            {
                MessageBox.Show("Выбери доступную клетку (Белую)");
                return;
            }

            if (!player.TurnStatus)
            {
                enemy.DoStep();
            }

            ShowFields();

            dataGridView2.ClearSelection();

            if (CheckOnWin(player.EnemyField))
            {
                ImplementFromFields(enemy.EnemyField, dataGridView1, true);
                ImplementFromFields(player.EnemyField, dataGridView2, true);
                MessageBox.Show("Победа за игроком!");
            }
            else if (CheckOnWin(enemy.EnemyField))
            {
                ImplementFromFields(enemy.EnemyField, dataGridView1, true);
                ImplementFromFields(player.EnemyField, dataGridView2, true);
                MessageBox.Show("Победа за компьютером!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Point> Selected = SelectedCellsToArray(dataGridView1.SelectedCells);
            if (AllowedSelection(Selected, dataGridView1))
            {
                if (Selected.Count == 4 && Ship4 != 0)
                {
                    for (int i = 0; i < Selected.Count; i++)
                    {
                        dataGridView1[Selected[i].X, Selected[i].Y].Style.BackColor = Color.Black;
                    }
                    Ship4--;
                }

                if (Selected.Count == 3 && Ship3 != 0)
                {
                    for (int i = 0; i < Selected.Count; i++)
                    {
                        dataGridView1[Selected[i].X, Selected[i].Y].Style.BackColor = Color.Black;
                    }
                    Ship3--;
                }

                if (Selected.Count == 2 && Ship2 != 0)
                {
                    for (int i = 0; i < Selected.Count; i++)
                    {
                        dataGridView1[Selected[i].X, Selected[i].Y].Style.BackColor = Color.Black;
                    }
                    Ship2--;
                }

                if (Selected.Count == 1 && Ship1 != 0)
                {
                    for (int i = 0; i < Selected.Count; i++)
                    {
                        dataGridView1[Selected[i].X, Selected[i].Y].Style.BackColor = Color.Black;
                    }
                    Ship1--;
                }
            }

            dataGridView1.ClearSelection();

            if (Ship1 + Ship2 + Ship3 + Ship4 == 0)
            {
                button2.Enabled = true;
                button4.Enabled = false;
                HideLabels();
            }
            else
            {
                ShowAndUpdate();
            }


        }

        #region AllMethods

        public void HideLabels()
        {
            label1.Hide();
            label2.Hide();
            label3.Hide();
            label4.Hide();
        }

        public void ShowAndUpdate()
        {
            label1.Show();
            label2.Show();
            label3.Show();
            label4.Show();

            label1.Text = "1P : " + Ship1.ToString();
            label2.Text = "2P : " + Ship2.ToString();
            label3.Text = "3P : " + Ship3.ToString();
            label4.Text = "4P : " + Ship4.ToString();
        }

        public void ShowFields()
        {
            ImplementFromFields(enemy.EnemyField, dataGridView1, true);
            ImplementFromFields(player.EnemyField, dataGridView2, false);
        }

        public void RestartGame()
        {
            CreateGrid(dataGridView1);
            CreateGrid(dataGridView2);
            button1.Enabled = true;
            button3.Enabled = false;
            radioButton1.Enabled= true;
            radioButton2.Enabled= true;
            dataGridView1.Enabled = false;
            dataGridView2.Enabled = false;
            RefreshShips();
        }

        public void CreateGrid(DataGridView grid)
        {
            string[] abc = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            grid.RowCount = 11;
            grid.ColumnCount = 11;

            for (int i = 0; i < grid.RowCount; i++)
            {
                grid.Rows[i].Height = grid.Height / grid.RowCount;
                grid.Columns[i].Width = grid.Width / grid.ColumnCount;

                for (int j = 0; j < grid.ColumnCount; j++)
                {
                    grid[j, i].Style.BackColor = Color.White;
                    grid[j, i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grid[j, i].Style.Font = new Font("Microsoft Sans Serif", 18);
                    grid[j, i].Value = "";
                }
            }

            for (int i = 1; i < dataGridView1.RowCount; i++)
            {
                grid[i, 0].Value = abc[i - 1];
                grid[0, i].Value = i.ToString();
            }
        }

        public void HideAllStart()
        {
            button1.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        public void ImplementFromFields(int[,] arr, DataGridView x, bool player)
        {
            for (int i = 0; i < Math.Sqrt(arr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(arr.Length); j++)
                {
                    switch (player)
                    {
                        case true:
                            switch (arr[i, j])
                            {
                                case 0:
                                    x[j + 1, i + 1].Style.BackColor = Color.White;
                                    break;
                                case 2:
                                    x[j + 1, i + 1].Style.BackColor = Color.Black;
                                    break;
                                case 1:
                                    x[j + 1, i + 1].Style.BackColor = Color.Orange;
                                    break;
                                case -3:
                                case -2:
                                    x[j + 1, i + 1].Style.BackColor = Color.Gray;
                                    break;
                                case -1:
                                    x[j + 1, i + 1].Style.BackColor = Color.Red;
                                    break;
                            }
                            break;
                        case false:
                            switch (arr[i, j])
                            {
                                case 0:
                                case 2:
                                    x[j + 1, i + 1].Style.BackColor = Color.White;
                                    break;
                                case 1:
                                    x[j + 1, i + 1].Style.BackColor = Color.Orange;
                                    break;
                                case -3:
                                case -2:
                                    x[j + 1, i + 1].Style.BackColor = Color.Gray;
                                    break;
                                case -1:
                                    x[j + 1, i + 1].Style.BackColor = Color.Red;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        List<Point> SelectedCellsToArray(DataGridViewSelectedCellCollection dataGridCells)
        {
            List<Point> result = new List<Point>();
            for (int i = 0; i < dataGridCells.Count; i++)
            {
                result.Add(new Point(dataGridCells[i].ColumnIndex, dataGridCells[i].RowIndex));
            }
            return result;
        }

        public void RefreshShips()
        {
            Ship4 = 1;
            Ship3 = 2;
            Ship2 = 3;
            Ship1 = 4;
        }

        public bool CheckOnWin(int[,] arr)
        {
            for (int i = 0; i < Math.Sqrt(arr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(arr.Length); j++)
                {
                    if (arr[i, j] == 2)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool AllowedSelection(List<Point> Sel, DataGridView x)
        {
            if(Sel.Count > 4)
            {
                return false;
            }

            for (int i = 0; i < Sel.Count; i++)
            {
                if (Sel[i].X == 0 || Sel[i].Y == 0)
                {
                    return false;
                }
            }

            bool inRow = true;
            bool inCol = true;

            for (int i = 0; i < Sel.Count; i++)
            {
                if (Sel[i].X != Sel[0].X)
                {
                    inRow = false;
                }

                if(Sel[i].Y != Sel[0].Y)
                {
                    inCol = false;
                }
            }

            if(inRow == false && inCol == false)
            {
                return false;
            }

            for (int i = 0; i < Sel.Count; i++)
            {
                if (x[Sel[i].X, Sel[i].Y].Style.BackColor != Color.White)
                {
                    return false;
                }
            }

            //Проверка на нахождение рядом другого корабля

            for (int i = 0; i < Sel.Count; i++)
            {
                if (Sel[i].X > 1)
                {
                    if (x[Sel[i].X - 1, Sel[i].Y].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if (Sel[i].Y > 1)
                {
                    if (x[Sel[i].X, Sel[i].Y - 1].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if (Sel[i].X < fieldSize - 1)
                {
                    if (x[Sel[i].X + 1, Sel[i].Y].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if (Sel[i].Y < fieldSize - 1)
                {
                    if (x[Sel[i].X, Sel[i].Y + 1].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if (Sel[i].X > 1 && Sel[i].Y > 1)
                {
                    if (x[Sel[i].X - 1, Sel[i].Y - 1].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if (Sel[i].X < fieldSize - 1 && Sel[i].Y < fieldSize - 1)
                {
                    if (x[Sel[i].X + 1, Sel[i].Y + 1].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if(Sel[i].X < fieldSize - 1 && Sel[i].Y > 1)
                {
                    if (x[Sel[i].X + 1, Sel[i].Y - 1].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }

                if (Sel[i].Y < fieldSize - 1 && Sel[i].X > 1)
                {
                    if (x[Sel[i].X - 1, Sel[i].Y + 1].Style.BackColor == Color.Black)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}

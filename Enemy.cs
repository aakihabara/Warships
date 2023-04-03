using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldOfWarships
{
    class Enemy : WoWCheck
    {
        /// <summary>
        /// Индексы:
        /// -3 - Клетки, которые будут вокруг уничтоженного корабля, куда нельзя будет выстрелить (Недоступная клетка)
        /// -2 - Клетка, куда выстрелили, но там ничего не было (Недоступная клетка)
        /// -1 - Уничтожен (Недоступная клетка)
        /// 0 - Пустая клетка
        /// 1 - Ранен (Недоступная клетка)
        /// 2 - Полностью живые корабли
        /// </summary>

        public int[,] EnemyField { get; set; } = new int[FieldSize, FieldSize];
        Random rnd = new Random();
        public bool TurnStatus { get; set; } = false;

        List<Point> DamageList = new List<Point>();

        List<Point> FieldXY = new List<Point>();

        int tempX, tempY;

        Point point;

        public Enemy(DataGridView x)
        {
            for (int i = 1; i < x.RowCount; i++)
            {
                for (int j = 1; j < x.ColumnCount; j++)
                {
                    if (x[j, i].Style.BackColor == Color.White)
                    {
                        EnemyField[i - 1, j - 1] = 0;
                    }
                    else
                    {
                        EnemyField[i - 1, j - 1] = 2;
                    }
                }
            }

            for (int i = 0; i < Math.Sqrt(EnemyField.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(EnemyField.Length); j++)
                {
                    FieldXY.Add(new Point(i, j));
                }
            }
        }


        public void DoStep()
        {
            bool allowShoot = true;

            while (allowShoot == true)
            {
                if(DamageList.Count == 0)//Если мы промахнулись до этого, то рандомный шаг
                {

                    if(FieldXY.Count == 0)
                    {
                        return;
                    }

                    int tempRandom = rnd.Next(0, FieldXY.Count);

                    tempX = FieldXY[tempRandom].X;
                    tempY = FieldXY[tempRandom].Y;

                    FieldXY.RemoveAt(tempRandom);

                    if(CanToShot(EnemyField, tempX, tempY))
                    {
                        if(Shoot(EnemyField, tempX, tempY))//Если мы попали, то создаём массив на 4 направления
                        {
                            if (EnemyField[tempX, tempY] == 1)
                            {
                                FillCorrectArounds(EnemyField, tempX, tempY);
                            }
                        }
                        else
                        {
                            allowShoot = false;
                        }
                    }
                }
                else //Если мы попали до этого, и не уничтожили корабль
                {

                    int pick = 0;

                    Point tempPick = DamageList[pick];

                    if(Shoot(EnemyField, tempPick.X, tempPick.Y))
                    {
                        DamageList.RemoveAt(pick);

                        if (tempPick.X - tempX != 0)
                        {

                            FillBlacks(EnemyField, tempPick.X, tempPick.Y, 0);

                            //if(tempPick.X - tempX < 0)
                            //{
                            //    if(tempPick.X - 1 > 0)
                            //    {
                            //        if (EnemyField[tempPick.X - 1, tempPick.Y] == 0 || EnemyField[tempPick.X - 1, tempPick.Y] == 2)
                            //        {
                            //            Point tempPick2 = new Point(tempPick.X - 1, tempPick.Y);

                            //            DamageList.Add(tempPick2);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    if (tempPick.X + 1 < FieldSize - 1)
                            //    {
                            //        if (EnemyField[tempPick.X + 1, tempPick.Y] == 0 || EnemyField[tempPick.X + 1, tempPick.Y] == 2)
                            //        {
                            //            Point tempPick2 = new Point(tempPick.X + 1, tempPick.Y);

                            //            DamageList.Add(tempPick2);
                            //        }
                            //    }
                            //}

                            for (int i = 0; i < DamageList.Count;)
                            {
                                Point tempPick2 = new Point(DamageList[i].X, DamageList[i].Y);

                                if(tempPick2.X == tempX)
                                {
                                    DamageList.RemoveAt(i);
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                        else if(tempPick.Y - tempY != 0)
                        {
                            FillBlacks(EnemyField, tempPick.X, tempPick.Y, 1);

                            //if (tempPick.Y - tempY < 0)
                            //{
                            //    if (tempPick.Y - 1 > 0)
                            //    {
                            //        if (EnemyField[tempPick.X, tempPick.Y - 1] == 0 || EnemyField[tempPick.X, tempPick.Y - 1] == 2)
                            //        {
                            //            Point tempPick2 = new Point(tempPick.X, tempPick.Y - 1);

                            //            DamageList.Add(tempPick2);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    if (tempPick.Y + 1 < FieldSize - 1)
                            //    {
                            //        if (EnemyField[tempPick.X, tempPick.Y + 1] == 0 || EnemyField[tempPick.X, tempPick.Y + 1] == 2)
                            //        {
                            //            Point tempPick2 = new Point(tempPick.X, tempPick.Y + 1);

                            //            DamageList.Add(tempPick2);
                            //        }
                            //    }
                            //}

                            for (int i = 0; i < DamageList.Count;)
                            {
                                Point tempPick2 = new Point(DamageList[i].X, DamageList[i].Y);

                                if (tempPick2.Y == tempY)
                                {
                                    DamageList.RemoveAt(i);
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                    }
                    else
                    {
                        DamageList.RemoveAt(pick);
                        allowShoot = false;
                    }

                }

                RebuildField(EnemyField);
            }

            for (int i = 0; i < Math.Sqrt(EnemyField.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(EnemyField.Length); j++)
                {
                    if (EnemyField[i, j] == 1)
                    {
                        if (!CheckOnAroundCellsOrange(EnemyField, i, j))
                        {
                            EnemyField[i, j] = -1;
                        }
                    }
                }
            }

            if (EnemyField[tempX, tempY] == -1)
            {
                DamageList.Clear();
            }
        }

        public void FillBlacks(int[,] arr, int x, int y, int rotation)
        {
            if(arr[x, y] != -3 && arr[x, y] != -2)
            {
                switch (rotation)
                {
                    case 0:
                        if (x > 0)
                        {
                            if (arr[x - 1, y] == 2 || arr[x - 1, y] == 0)
                            {
                                point = new Point(x - 1, y);
                                DamageList.Add(point);
                            }

                        }

                        if (x < FieldSize - 1)
                        {
                            if (arr[x + 1, y] == 2 || arr[x + 1, y] == 0)
                            {
                                point = new Point(x + 1, y);
                                DamageList.Add(point);
                            }
                        }
                        break;

                    case 1:
                        if (y > 0)
                        {
                            if (arr[x, y - 1] == 2 || arr[x, y - 1] == 0)
                            {
                                point = new Point(x, y - 1);
                                DamageList.Add(point);
                            }
                        }
                        if (y < FieldSize - 1)
                        {
                            if (arr[x, y + 1] == 2 || arr[x, y + 1] == 0)
                            {
                                point = new Point(x, y + 1);
                                DamageList.Add(point);
                            }
                        }
                        break;
                }
            }
        }

        public void FillCorrectArounds(int[,] arr, int x, int y)
        {
            if(x > 0)
            {
                if (arr[x - 1, y] == 0 || arr[x - 1, y] == 2)
                {
                    point = new Point(x - 1, y);
                    DamageList.Add(point);
                }
                
            }

            if(y > 0)
            {
                if (arr[x, y - 1] == 0 || arr[x, y - 1] == 2)
                {
                    point = new Point(x, y - 1);
                    DamageList.Add(point);
                }
            }

            if(x < FieldSize - 1)
            {
                if (arr[x + 1, y] == 0 || arr[x + 1, y] == 2)
                {
                    point = new Point(x + 1, y);
                    DamageList.Add(point);
                }
            }

            if(y < FieldSize - 1)
            {
                if (arr[x, y + 1] == 0 || arr[x, y + 1] == 2)
                {
                    point = new Point(x, y + 1);
                    DamageList.Add(point);
                }
            }
        }
    }
}

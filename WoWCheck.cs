using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfWarships
{
    class WoWCheck : IWar
    {
        public static int FieldSize { get; set; } = 10;

        public static int maxShipSize = 4;

        public override bool Shoot(int[,] arr, int x, int y)
        {
            switch (arr[x, y])
            {
                case 0:
                    arr[x, y] = -2;
                    return false;
                case 2:
                    arr[x, y] = 1;
                    break;
            }

            for (int i = 0; i < Math.Sqrt(arr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(arr.Length); j++)
                {
                    if (arr[i, j] == 1)
                    {
                        if (!CheckOnAroundCellsOrange(arr, i, j))
                        {
                            arr[i, j] = -1;
                        }
                    }
                }
            }

            return true;
        }//Выстрел

        public bool CheckOnAroundCellsOrange(int[,] arr, int x, int y)
        {
            if (x > 0)
            {
                for (int i = 1; i < maxShipSize;)
                {
                    if(x - i >= 0)
                    {
                        if (arr[x - i, y] == 2)
                        {
                            return true;
                        }
                        else if (arr[x - i, y] != 1)
                        {
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (y > 0)
            {
                for (int i = 1; i < maxShipSize;)
                {
                    if (y - i >= 0)
                    {
                        if (arr[x, y - i] == 2)
                        {
                            return true;
                        }
                        else if (arr[x, y - i] != 1)
                        {
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (x < FieldSize - 1)
            {
                for (int i = 1; i < maxShipSize;)
                {
                    if (x + i <= FieldSize - 1)
                    {
                        if (arr[x + i, y] == 2)
                        {
                            return true;
                        }
                        else if (arr[x + i, y] != 1)
                        {
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (y < FieldSize - 1)
            {
                for (int i = 1; i < maxShipSize;)
                {
                    if (y + i <= FieldSize - 1)
                    {
                        if (arr[x, y + i] == 2)
                        {
                            return true;
                        }
                        else if(arr[x, y + i] != 1)
                        {
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        public bool CheckOnAroundCells(int[,] arr, int x, int y)
        {
            if(x > 0)
            {
                if (arr[x - 1, y] == 2)
                {
                    return true;
                }
            }

            if (y > 0)
            {
                if (arr[x, y - 1] == 2)
                {
                    return true;
                }
            }

            if(x < FieldSize - 1)
            {
                if (arr[x + 1, y] == 2)
                {
                    return true;
                }
            }

            if (y < FieldSize - 1)
            {
                if (arr[x, y + 1] == 2)
                {
                    return true;
                }
            }

            return false;
        }//Проверяем, есть ли вокруг точки ещё корабли

        public override bool CanToShot(int[,] arr, int x, int y)
        {
            if (arr[x, y] == 2 || arr[x, y] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }//Проверяется, можем ли мы выстрелить в эту точку

        public void RebuildField(int[,] arr)//Помечаем недоступные точки
        {
            for (int i = 0; i < Math.Sqrt(arr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(arr.Length); j++)
                {
                    if (arr[i, j] == -1)
                    {
                        if(i > 0)
                        {
                            if(arr[i - 1, j] == 0)
                            {
                                arr[i - 1, j] = -3;
                            }
                        }

                        if(j > 0)
                        {
                            if (arr[i, j - 1] == 0)
                            {
                                arr[i, j - 1] = -3;
                            }
    }

                        if(i < FieldSize - 1)
                        {
                            if(arr[i + 1, j] == 0)
                            {
                                arr[i + 1, j] = -3;
                            }
                        }

                        if(j < FieldSize - 1)
                        {
                            if(arr[i, j + 1] == 0)
                            {
                                arr[i, j + 1] = -3;
                            }
                        }

                        if(i > 0 && j > 0)
                        {
                            if (arr[i - 1, j - 1] == 0)
                            {
                                arr[i - 1, j - 1] = -3;
                            }
                        }

                        if(i < FieldSize - 1 && j < FieldSize - 1)
                        {
                            if(arr[i + 1, j + 1] == 0)
                            {
                                arr[i + 1, j + 1] = -3;
                            }
                        }

                        if(i < FieldSize - 1 && j > 0)
                        {
                            if(arr[i + 1, j - 1] == 0)
                            {
                                arr[i + 1, j - 1] = -3;
                            }
                        }

                        if(j < FieldSize - 1 && i > 0)
                        {
                            if(arr[i - 1, j + 1] == 0)
                            {
                                arr[i - 1, j + 1] = -3;
                            }
                        }
                    }
                }
            }
        }

        public bool CheckOnEmptyCells(int[,] arr)
        {
            for (int i = 0; i < Math.Sqrt(arr.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(arr.Length); j++)
                {
                    if (arr[i, j] == 0 || arr[i, j] == 2)
                    {
                        return true;                    
                    }
                }
            }

            return false;
        }
    }
}

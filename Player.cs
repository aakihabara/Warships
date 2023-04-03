using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldOfWarships
{
    class Player : WoWCheck
    {

        /// <summary>
        /// Индексы:
        /// -3 - Клетки, которые будут вокруг уничтоженного корабля, куда нельзя будет выстрелить
        /// -2 - Клетка, куда выстрелили, но там ничего не было
        /// -1 - уничтожен
        /// 0 - пустая клетка(неизвестное)
        /// 1 - ранен (Не занимаемая клетка)
        /// 2 - полностью живые корабли
        /// </summary>
        
        ///Направление:
        ///0 - По горизонтали
        ///1 - По вертикали

        public int[,] EnemyField { get; set; } = new int[FieldSize, FieldSize];

        List<int> ShipList = new List<int>();

        Random rnd = new Random();

        public bool TurnStatus { get; set; } = false;

        public Player()
        {

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 4 - i + 1; j > 0; j--)
                {
                    ShipList.Add(i);
                }
            }


            while(ShipList.Count > 0)
            {
                while (true)
                {
                    int rotation = rnd.Next(0, 2);
                    int currShip = ShipList[rnd.Next(0, ShipList.Count)];
                    int positionX = rnd.Next(0, FieldSize);
                    int positionY = rnd.Next(0, FieldSize);

                    if (EnemyField[positionX, positionY] == 2)
                    {
                        continue;
                    }

                    if(CheckOnBorders(positionX, positionY, currShip, rotation))
                    {
                        continue;
                    }

                    if(CheckOnAnotherShip(EnemyField, positionX, positionY, -1))
                    {
                        continue;
                    }

                    bool good = true;

                    switch (rotation)
                    {
                        case 0:
                            for (int i = 1; i < currShip; i++)
                            {
                                if (CheckOnAnotherShip(EnemyField, positionX + i, positionY, rotation))
                                {
                                    good = false;
                                }
                            }

                            if (!good)
                            {
                                continue;
                            }

                            for (int i = 0; i < currShip; i++)
                            {
                                EnemyField[positionX + i, positionY] = 2;
                            }
                            break;
                        case 1:
                            for (int i = 1; i < currShip; i++)
                            {
                                if (CheckOnAnotherShip(EnemyField, positionX, positionY + i, rotation))
                                {
                                    good = false;
                                }
                            }

                            if (!good)
                            {
                                continue;
                            }

                            for (int i = 0; i < currShip; i++)
                            {
                                EnemyField[positionX, positionY + i] = 2;
                            }
                            break;
                    }

                    ShipList.Remove(currShip);

                    break;
                }
            }
        }

        public void Action(int x, int y)
        {
            TurnStatus = true;

            if(!Shoot(EnemyField, x, y))
            {
                TurnStatus = false;
            }

            RebuildField(EnemyField);


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
        }

        public bool CheckOnAnotherShip(int[,] arr, int x, int y, int rotation)
        {
            if(x > FieldSize - 2 || y > FieldSize - 2)
            {
                return true;
            }

            if (x > 0 && y > 0)
            {
                if (arr[x - 1, y - 1] == 2)
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

            if(y < FieldSize - 1 && x < FieldSize - 1)
            {
                if (arr[x + 1, y + 1] == 2)
                {
                    return true;
                }    
            }

            if(x < FieldSize - 1 && y > 0)
            {
                if (arr[x + 1, y - 1] == 2)
                {
                    return true;
                }
            }

            if(y < FieldSize - 1 && x > 0)
            {
                if (arr[x - 1, y + 1] == 2)
                {
                    return true;
                }
            }

            switch (rotation)
            {
                case 0://Условие проверки по горизонтали
                    if (y > 0)
                    {
                        if (arr[x, y - 1] == 2)
                        {
                            return true;
                        }
                    }
                    break;
                case 1://Условие проверки по вертикали
                    if (x > 0)
                    {
                        if (arr[x - 1, y] == 2)
                        {
                            return true;
                        }
                    }
                    break;
                case -1://Для общей проверки
                    if (x > 0)
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
                    break;
            }

            return false;
        }

        public bool CheckOnBorders(int posX, int posY, int shipSize, int rotation)
        {
            switch (rotation)
            {
                case 0:
                    if (posX + shipSize - 1 > FieldSize - 1)
                    {
                        return true;
                    }
                    break;

                case 1:
                    if (posY + shipSize - 1 > FieldSize - 1)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
    }
}

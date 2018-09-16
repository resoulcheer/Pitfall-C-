using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Media;

namespace Pitfall1
{
    class Program
    {
        struct Player
        {
            public int x;
            public int y;
            public char symbol;
            public ConsoleColor color;
            public int lives;
        }
        static void Main(string[] args)
        {
            SoundPlayer pbg = new SoundPlayer("04_All_of_Us.wav");
            pbg.Play();
            // Intialize Game
            const int WINDOW_HEIGHT = 30;
            const int WINDOW_WIDTH = 50;
            bool isGamePlay = true;
            Random randomGenerator = new Random();
            List<Player> RockList = new List<Player>();

            Console.BufferHeight = Console.WindowHeight = WINDOW_HEIGHT;
            Console.BufferWidth = Console.WindowWidth = WINDOW_WIDTH;
            Console.Clear();

            // Intialize hero
            Player hero;
            hero.x = WINDOW_WIDTH / 2 - 1;
            hero.y = WINDOW_HEIGHT - 15;
            hero.lives = 3;
            hero.color = ConsoleColor.Cyan;
            hero.symbol = '$';
            int score = 0;
            int speed = 0;

            while (isGamePlay)
            {

                bool isHitted = false;
                // Input
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true); //วงเล็บทรูเพื่อให้ปุ่มที่กดไม่แสดง
                    while (Console.KeyAvailable) { Console.ReadKey(true); }
                    if (keyPressed.Key == ConsoleKey.LeftArrow)
                    {
                        if (hero.x > 0)
                        {
                            hero.x--;
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.RightArrow)
                    {
                        if (hero.x < WINDOW_WIDTH - 2)
                        {
                            hero.x++;
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.UpArrow)
                    {
                        if (hero.y > WINDOW_HEIGHT - 26)
                        {
                            hero.y--;
                        }
                    }
                    if (keyPressed.Key == ConsoleKey.DownArrow)
                    {
                        if (hero.y < WINDOW_HEIGHT - 2)
                        {
                            hero.y++;
                        }
                    }
                }

                // Update logic
                int spawnRockChance = randomGenerator.Next(0, 100);

                if (spawnRockChance < 15)
                { // spawn lives
                    Player newLives = new Player();
                    newLives.x = randomGenerator.Next(0, WINDOW_WIDTH - 1);
                    newLives.y = 29;
                    newLives.color = ConsoleColor.Magenta;
                    newLives.symbol = '*';
                    RockList.Add(newLives);

                }
                else if (spawnRockChance >= 15 && spawnRockChance < 60)
                {
                    //spawn rocks
                    Player newRock = new Player();
                    newRock.x = randomGenerator.Next(0, WINDOW_WIDTH - 1);
                    newRock.y = 29;
                    newRock.color = ConsoleColor.Red;
                    newRock.symbol = '๑';
                    RockList.Add(newRock);

                }

                //spawn wall
                Player newWall1 = new Player();
                for (int i = randomGenerator.Next(10, WINDOW_WIDTH / 2); i > -1; i--)
                {
                    newWall1.x = i;
                    newWall1.y = 29;
                    newWall1.color = ConsoleColor.Green;
                    newWall1.symbol = '}';
                    RockList.Add(newWall1);
                }
                Player newWall2 = new Player();
                for (int i = randomGenerator.Next(WINDOW_WIDTH / 2, WINDOW_WIDTH - 10); i < 50; i++)
                {
                    newWall2.x = i;
                    newWall2.y = 29;
                    newWall2.color = ConsoleColor.Green;
                    newWall2.symbol = '{';
                    RockList.Add(newWall2);
                }
 



                //update rocks
                List<Player> newList = new List<Player>();

                for (int i = 0; i < RockList.Count; i++)
                {
                    Player oldRock = RockList[i];
                    Player newRock = new Player();
                    newRock.x = oldRock.x;
                    newRock.y = oldRock.y - 1;
                    newRock.color = oldRock.color;
                    newRock.symbol = oldRock.symbol;

                    // Collision detection
                        if ((newRock.x == hero.x) &&
                            (newRock.y == hero.y))
                        {
                            if (newRock.symbol == '๑')
                            {
                           
                            SoundPlayer phr = new SoundPlayer("270327__littlerobotsoundfactory__hit-00.wav");
                            phr.Play();
                            Thread.Sleep(500);
                            SoundPlayer p1 = new SoundPlayer("04_All_of_Us.wav");
                            p1.Play();
                            hero.lives--;
                            isHitted = true;
                            
                            }
                            else if (newRock.symbol == '}' || newRock.symbol == '{')
                            {
                            
                            SoundPlayer phw = new SoundPlayer("270326__littlerobotsoundfactory__hit-01.wav");
                            phw.Play();
                            Thread.Sleep(500);
                            SoundPlayer p1 = new SoundPlayer("04_All_of_Us.wav");
                            p1.Play();
                            hero.lives--;
                                isHitted = true;
                            
                        }                            
                            else if (newRock.symbol == '*')
                            {
                            
                            SoundPlayer pp = new SoundPlayer("270341__littlerobotsoundfactory__pickup-04.wav");
                            pp.Play();
                            Thread.Sleep(500);
                            SoundPlayer p1 = new SoundPlayer("04_All_of_Us.wav");
                            p1.Play();
                            hero.lives++;
                            
                        }
                        }
                    

                    if (newRock.y < WINDOW_HEIGHT && newRock.y > WINDOW_HEIGHT - 26)
                    {
                        newList.Add(newRock);
                    }
                    else
                    {
                        score++;
                    }
                    if (hero.lives >= 3)
                    {
                        PrintString(10, 29, "$ $ $", ConsoleColor.Cyan);
                    }
                    else if (hero.lives == 2)
                    {
                        PrintString(10, 29, "$ $", ConsoleColor.Cyan);
                    }
                    else if (hero.lives == 1)
                    {
                        PrintString(10, 29, "$", ConsoleColor.Cyan);
                    }

                    else if (hero.lives < 0)
                    {
                        SoundPlayer pl = new SoundPlayer("270334__littlerobotsoundfactory__jingle-lose-01.wav");
                        pl.Play();
                        hero.symbol = '.';
                        PrintString(hero.x, hero.y, "*", ConsoleColor.Cyan);
                        Thread.Sleep(500);
                        PrintString(hero.x + 1, hero.y - 1, ".", ConsoleColor.Cyan);
                        PrintString(hero.x + 2, hero.y - 2, ".", ConsoleColor.Cyan);
                        PrintString(hero.x + 2, hero.y, ".", ConsoleColor.Cyan);
                        PrintString(hero.x + 4, hero.y, ".", ConsoleColor.Cyan);
                        PrintString(hero.x - 3, hero.y, ".", ConsoleColor.Cyan);
                        PrintString(hero.x - 5, hero.y, ".", ConsoleColor.Cyan);
                        PrintString(hero.x + 1, hero.y + 1, ".", ConsoleColor.Cyan);
                        PrintString(hero.x - 1, hero.y - 1, ".", ConsoleColor.Cyan);
                        PrintString(hero.x - 1, hero.y + 1, ".", ConsoleColor.Cyan);
                        PrintString(hero.x + 1, hero.y - 1, ".", ConsoleColor.Cyan);
                        Thread.Sleep(300);
                        PrintString(hero.x + 2, hero.y + 2, ".", ConsoleColor.Cyan);                                             
                        PrintString(hero.x - 2, hero.y - 2, ".", ConsoleColor.Cyan);
                        PrintString(hero.x - 2, hero.y + 2, ".", ConsoleColor.Cyan);
                        Thread.Sleep(300);
                        PrintString(hero.x - 5, hero.y + 4, ".", ConsoleColor.Cyan);
                        PrintString(hero.x - 3, hero.y - 4, ".", ConsoleColor.Cyan);
                        PrintString(hero.x + 3, hero.y - 5, ".", ConsoleColor.Cyan);
                        Thread.Sleep(2300);
                        
                        PrintString(7, 10, " #####      ###    ###    ### #######", ConsoleColor.Yellow);
                        PrintString(7, 11, "##         ## ##   ###    ### ##", ConsoleColor.Yellow);
                        PrintString(7, 12, "##  ###   #######  ## #  # ## #######", ConsoleColor.Yellow);
                        PrintString(7, 13, "##   ##  ##     ## ##  ##  ## ##", ConsoleColor.Yellow);
                        PrintString(7, 14, " #####   ##     ## ##  ##  ## #######", ConsoleColor.Yellow);
                        PrintString(7, 15, "  ", ConsoleColor.Yellow);
                        PrintString(7, 16, "    #####  ##   ## ####### ######", ConsoleColor.Yellow);
                        PrintString(7, 17, "   ##   ## ##   ## ##      ##   ##", ConsoleColor.Yellow);
                        PrintString(7, 18, "   ##   ## ##   ## ####### ######", ConsoleColor.Yellow);
                        PrintString(7, 19, "   ##   ##  ## ##  ##      ##  ##", ConsoleColor.Yellow);
                        PrintString(7, 20, "    #####    ###   ####### ##   ##", ConsoleColor.Yellow);
                        Console.ReadLine();
                        Environment.Exit(0);





                    }
                }
                RockList = newList;



                //Draw game
                Console.Clear();
                PrintCharacter(hero.x, hero.y, hero.symbol, hero.color);

                for (int i = 0; i < RockList.Count; i++)
                {
                    PrintCharacter(RockList[i].x, RockList[i].y,
                        RockList[i].symbol, RockList[i].color);
                }

                //  foreach(Player rock in RockList)
                //  {
                //      PrintCharacter(rock.x, rock.y, rock.symbol, rock.color);

                // Display lives and score
                for (int i = 0; i < WINDOW_WIDTH; i++)
                {
                    PrintCharacter(i, 2, '=', ConsoleColor.White);
                }
                PrintString(20, 1, "PITFALL", ConsoleColor.White);
                PrintString(2, 29, "Lives:", ConsoleColor.White);
                PrintString(39, 29, "Score:" + score, ConsoleColor.White);

                if (speed < 300)
                {
                    speed++;
                }
                Thread.Sleep(400 - speed);
            }



        

    }

    static void PrintCharacter(int x, int y, char symbol, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }


        static void PrintString(int x, int y, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(text);
        }
        
    }
}

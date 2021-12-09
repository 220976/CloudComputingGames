using System;
using System.Collections.Generic;
using System.IO;
namespace DBGames.Models
{
    public class Context
    {
        public List<Game> Games { get; set; }
        public Context()
        {
            string path = Directory.GetCurrentDirectory() + "/DataContext/vgsales.csv";
            Games = new List<Game>();
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader rd = new StreamReader(fs))
                {
                    string line;
                    rd.ReadLine();
                    while ((line = rd.ReadLine()) != null)
                    {
                        Game game = new Game();
                        string[] columns = line.Split(",");
                        game.Rank = Convert.ToInt32(columns[0]);
                        game.Name = columns[1];
                        game.Platform = columns[2];
                        try
                        {
                            game.Year = Convert.ToInt32(columns[3]);
                        }
                        catch (System.Exception)
                        {
                            game.Year = 0;
                        }
                        game.Genre = columns[4];
                        game.Publisher = columns[5];
                        try
                        {
                            game.NA_Sales = Convert.ToDouble(columns[6]);
                        }
                        catch (System.Exception)
                        {
                            game.NA_Sales = 0;
                        }
                        try
                        {
                            game.EU_Sales = Convert.ToDouble(columns[7]);
                        }
                        catch (System.Exception)
                        {
                            game.EU_Sales = 0;
                        }
                        try
                        {
                            game.JP_Sales = Convert.ToDouble(columns[8]);
                        }
                        catch (System.Exception)
                        {
                            game.JP_Sales = 0;
                        }
                        try
                        {
                            game.Other_Sales = Convert.ToDouble(columns[9]);
                        }
                        catch (System.Exception)
                        {
                            game.Other_Sales = 0;
                        }
                        try
                        {
                            game.Global_Sales = Convert.ToDouble(columns[10]);
                        }
                        catch (System.Exception)
                        {
                            game.Global_Sales = 0;
                        }

                        Games.Add(game);
                    }
                }
            }
        }
    }
}
using AnalysisService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AnalysisService.Controllers
{
    public class HomeController : Controller
    {
        Context _context;
        public HomeController()
        {
            _context = new Context();
        }



        [HttpGet]
        [Route("/Home/CompareSalesTowGame/{Platform1}/{Platform2}")]
        public IActionResult CompareSalesTowGame(string Platform1, string Platform2)
        {
            #region CheckAuthentication
            string UserName = Request.Headers["UserName"];
            string Token = Request.Headers["Token"];
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Token))
            {
                return StatusCode(401);
            }
            if (!Authentication.IsAuthenticate(UserName,Token))
            {
                return StatusCode(401);
            }
            #endregion

            List<vmCompareTowGame> games = new List<vmCompareTowGame>();

            List<Game> Platform1Games = _context.Games.Where(g => g.Platform == Platform1).ToList();
            List<Game> Platform2Games = _context.Games.Where(g => g.Platform == Platform2).ToList();
            games.Add(new vmCompareTowGame()
            {
                Platform = Platform1,
                EU_Sales = Platform1Games.Select(g => g.EU_Sales).Sum(),
                JP_Sales = Platform1Games.Select(g => g.JP_Sales).Sum(),
                NA_Sales = Platform1Games.Select(g => g.NA_Sales).Sum(),
                Global_Sales = Platform1Games.Select(g => g.Global_Sales).Sum(),
                Other_Sales = Platform1Games.Select(g => g.Other_Sales).Sum()
            });
            games.Add(new vmCompareTowGame()
            {
                Platform = Platform2,
                EU_Sales = Platform2Games.Select(g => g.EU_Sales).Sum(),
                JP_Sales = Platform2Games.Select(g => g.JP_Sales).Sum(),
                NA_Sales = Platform2Games.Select(g => g.NA_Sales).Sum(),
                Global_Sales = Platform2Games.Select(g => g.Global_Sales).Sum(),
                Other_Sales = Platform2Games.Select(g => g.Other_Sales).Sum()
            });


            return View(games);
        }

        [HttpGet]
        [Route("/Home/CompareSalesInTowYear/{from}/{to}")]
        public IActionResult CompareSalesInTowYear(int from, int to)
        {
            if (from>to)
            {
                return BadRequest();
            }
            #region CheckAuthentication
            string UserName = Request.Headers["UserName"];
            string Token = Request.Headers["Token"];
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Token))
            {
                return StatusCode(401);
            }
            if (!Authentication.IsAuthenticate(UserName, Token))
            {
                return StatusCode(401);
            }
            #endregion
            List<vmCompareSalesInTowYear> salesRepoer = new List<vmCompareSalesInTowYear>();
            for (int i = from; i <= to; i++)
            {
                vmCompareSalesInTowYear vmCompare = new vmCompareSalesInTowYear();
                vmCompare.Year = i;
                vmCompare.Sales = _context.Games.Where(g=>g.Year==i).Select(g => g.Global_Sales).Sum();
                salesRepoer.Add(vmCompare);
            }
            return View(salesRepoer);
        }


        [HttpGet]
        [Route("/Home/CompareSalesTowPublisherInTowYear/{Publisher1}/{Publisher2}/{from}/{to}")]
        public IActionResult CompareSalesTowPublisherInTowYear(string Publisher1,string Publisher2,int from,int to)
        {

            if (from > to)
            {
                return BadRequest();
            }
            #region CheckAuthentication
            string UserName = Request.Headers["UserName"];
            string Token = Request.Headers["Token"];
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Token))
            {
                return StatusCode(401);
            }
            if (!Authentication.IsAuthenticate(UserName, Token))
            {
                return StatusCode(401);
            }
            #endregion


            List<vmCompareSalesTowPublisherInTowYear> result = new List<vmCompareSalesTowPublisherInTowYear>();

            for (int i = from; i <= to; i++)
            {
                vmCompareSalesTowPublisherInTowYear t = new vmCompareSalesTowPublisherInTowYear();
                t.Year = i;
                t.SellPublisher1 = _context.Games.Where(g=>g.Publisher==Publisher1&&g.Year==i).Select(g=>g.Global_Sales).Sum();
                t.SellPublisher2 = _context.Games.Where(g => g.Publisher == Publisher2 && g.Year == i).Select(g => g.Global_Sales).Sum();
                result.Add(t);
            }

            return View(result);
        }




        [HttpGet]
        [Route("/Home/CompareSalesGenresInTowYear/{from}/{to}")]
        public IActionResult CompareSalesGenresInTowYear(int from, int to)
        {
            if (from > to)
            {
                return BadRequest();
            }
            #region CheckAuthentication
            string UserName = Request.Headers["UserName"];
            string Token = Request.Headers["Token"];
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Token))
            {
                return StatusCode(401);
            }
            if (!Authentication.IsAuthenticate(UserName, Token))
            {
                return StatusCode(401);
            }
            #endregion
            List<vmCompareSalesGenresInTowYear> Genres = new List<vmCompareSalesGenresInTowYear>();

            List<string> distictGenres = _context.Games.Select(g => g.Genre).Distinct().ToList();
            foreach (var genre in distictGenres)
            {
                vmCompareSalesGenresInTowYear p = new vmCompareSalesGenresInTowYear();
                p.Genre = genre;
                p.Sales = _context.Games.Where(g => g.Genre == genre && g.Year >= from && g.Year <= to).Select(g => g.Global_Sales).Sum();
                if (p.Sales>0)
                {
                    Genres.Add(p);
                }

            }
            return View(Genres);
        }
    }
}
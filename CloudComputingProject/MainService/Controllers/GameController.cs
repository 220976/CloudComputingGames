using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MainService.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace MainService.Controllers
{
    public class GameController : Controller
    {
        Context _context;
        public GameController()
        {
            _context = new Context();
        }

        [HttpGet]
        [Route("/Game/AccordingToRank/{n}")]
        public IActionResult AccordingToRank(int n)
        {
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
            #endregion CheckAuthentication
            
            
            
            return Ok(_context.Games.FirstOrDefault(g => g.Rank == n));
        }


        [HttpGet]
        [Route("/Game/AccordingToPartialName/{name}")]
        public IActionResult AccordingToPartialName(string name)
        {
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
            #endregion CheckAuthentication

            
            if (String.IsNullOrEmpty(name))
            {
                return Ok(new List<Game>());
            }
            return Ok(_context.Games.Where(g => g.Name.Contains(name)).ToList());
        }

        [HttpGet]
        [Route("/Game/EUMoreThanNA/")]
        public IActionResult EUMoreThanNA()
        {

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
            #endregion CheckAuthentication


            return Ok(_context.Games.Where(g => g.EU_Sales > g.NA_Sales).ToList());
        }

        [HttpGet]
        [Route("/Game/EUMoreThanNA/{name}/{Year}")]
        public IActionResult FiveTopBestSellingOnPlatform(string name, int Year)
        {
            if (String.IsNullOrEmpty(name))
            {
                return Ok(new List<Game>());
            }
            return Ok(_context.Games.Where(g => g.Platform == name && g.Year == Year).OrderByDescending(g => g.Global_Sales).Take(5).ToList());
        }


        [HttpGet]
        [Route("/Game/NTopGamesInYear/{n}/{Year}")]
        public IActionResult NTopGamesInYear(int n, int Year)
        {


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
            #endregion CheckAuthentication





            return Ok(_context.Games.Where(g => g.Year == Year).OrderByDescending(g => g.Rank).Take(n).ToList());
        }

        [HttpGet]
        [Route("/Game/NTopGameAccordingToPlatformName/{n}/{PlatformName}")]
        public IActionResult NTopGameAccordingToPlatformName(int n, string PlatformName)
        {
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
            #endregion CheckAuthentication



            return Ok(_context.Games.Where(g => g.Platform == PlatformName).OrderByDescending(g => g.Rank).Take(n).ToList());
        }


        [HttpGet]
        [Route("/Game/NTopGameAccordingToGenre/{n}/{Genre}")]
        public IActionResult NTopGameAccordingToGenre(int n, string Genre)
        {
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
            #endregion CheckAuthentication
            return Ok(_context.Games.Where(g => g.Genre == Genre).OrderByDescending(g => g.Rank).Take(n).ToList());
        }
    }
}
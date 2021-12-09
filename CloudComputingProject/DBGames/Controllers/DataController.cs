using System.Collections.Generic;
using DBGames.Models;
using Microsoft.AspNetCore.Mvc;

namespace DBGames{
    public class DataController:Controller
    {
        Context _context;
        public DataController()
        {
            _context=new Context();
        }
        public List<Game> GetData(){
            return _context.Games;
        }
    }
}
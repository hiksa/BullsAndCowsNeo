using BullsAndCowsNeo.Web.Hubs;
using BullsAndCowsNeo.Web.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCowsNeo.Web.Controllers
{
    public class GameController : BaseApiController
    {
        private readonly IHubContext<GameHub> _roomHubCtx;
        private readonly GameEngine _gamesPool;

        public GameController(IHubContext<GameHub> roomHubCtx, GameEngine gamesPool)
        {
            _roomHubCtx = roomHubCtx;
            _gamesPool = gamesPool;
        }

        //[HttpPost]
        //public async Task<IActionResult> Join()
        //{
        //    //var groups = _roomHubCtx.Groups.AddToGroupAsync()
        //    //var connectiondId = _roomHubCtx.Clients.
        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> SetNumber()
        //{
        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Guess()
        //{
        //    return Ok();
        //}

        //[HttpPost]
        //public async Task<IActionResult> List()
        //{
        //    return Ok();
        //}
    }
}

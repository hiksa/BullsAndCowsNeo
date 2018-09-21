using BullsAndCowsNeo.Web.Hubs;
using BullsAndCowsNeo.Web.Infra;
using BullsAndCowsNeo.Web.Infra.Game;
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
        private readonly IHubContext<GameHub> roomHubCtx;
        private readonly GameEngine gamesPool;

        public GameController(IHubContext<GameHub> roomHubCtx, GameEngine gamesPool)
        {
            this.roomHubCtx = roomHubCtx;
            this.gamesPool = gamesPool;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework;

namespace XNALib
{
    public static class PlayerMgr
    {
        /// <summary>
        /// http://blog.nickgravelyn.com/2008/12/how-to-test-if-a-player-can-purchase-your-game/
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool CanPlayerBuyGame(PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];

            // if the player isn't signed in, they can't buy games
            if (gamer == null)
                return false;

            // lastly check to see if the account is allowed to buy games
            return gamer.Privileges.AllowPurchaseContent;
        }
    }
}

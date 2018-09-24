using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame
{
	public class SimpleGameObject
	{
		public new BazingaGame Game;

		public SimpleGameObject(BazingaGame game)
        {
            Game = game;
        }
    }
}

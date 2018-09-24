using BazingaGame.Animations;
using BazingaGame.Prefabs;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.States.Player
{
    class PlayerAttackState : IPlayerState
    {
        private const int FixtureRadiusX = 31;
        private const int FixtureRadiusY = 65;
        private readonly Vector2 FixtureOffset = new Vector2(-9, 0);

        private readonly float WeaponFixtureRadiusX = ConvertUnits.ToSimUnits(20);
        private readonly float WeaponFixtureRadiusY = ConvertUnits.ToSimUnits(20);
        private readonly Vector2 WeaponFixtureOffset = ConvertUnits.ToSimUnits(25, 5);

        public void Enter(Prefabs.BazingaPlayer player)
        {
            var offset = FixtureOffset * (player.Animation.IsFlippedHorizontally ? -1 : 1);
            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);

            // Custom fixture for a weapon attached to the same body
            var offsetW = WeaponFixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);
            FixtureFactory.AttachEllipse(WeaponFixtureRadiusX, WeaponFixtureRadiusY, 6, 1.0f, offsetW, player.Body).CollisionCategories = Category.Cat4;

            player.Animation.PlaySprite(SpriteState.Melee, false);
        }

        public IPlayerState HandleInput(BazingaPlayer player, KeyboardState input)
        {
            if (input.IsKeyDown(Keys.A))
            {
                player.Animation.PlaySprite(SpriteState.Melee, false);
            }

            return null;
        }

        public IPlayerState Update(BazingaPlayer player, GameTime gameTime)
        {
            if (player.Animation.AnimationCompleted)
            {
                return new PlayerIdleState();
            }

            return null;
        }

        public void Exit(BazingaPlayer player)
        {
            // Nop
        }
    }
}

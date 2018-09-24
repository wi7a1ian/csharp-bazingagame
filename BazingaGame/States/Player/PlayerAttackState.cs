using BazingaGame.Animations;
using BazingaGame.Prefabs;
using BazingaGame.Sounds;
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
    class PlayerAttackState : IGameComponentState
    {
        private const int FixtureRadiusX = 31;
        private const int FixtureRadiusY = 65;
        private readonly Vector2 FixtureOffset = new Vector2(-9, 0);

        private readonly float WeaponFixtureRadiusX = ConvertUnits.ToSimUnits(20);
        private readonly float WeaponFixtureRadiusY = ConvertUnits.ToSimUnits(20);
        private readonly Vector2 WeaponFixtureOffset = ConvertUnits.ToSimUnits(25, 5);

        private const string SoundEffect = @"Sounds/attack";

        private BazingaPlayer player;

        public void EnterState(StatefulGameComponent target)
        {
            player = target as BazingaPlayer;

            var offset = FixtureOffset * (player.Animation.IsFlippedHorizontally ? -1 : 1);
            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);

            // Custom fixture for a weapon attached to the same body
            var offsetW = WeaponFixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);
            var weaponFixture = FixtureFactory.AttachEllipse(WeaponFixtureRadiusX, WeaponFixtureRadiusY, 6, 1.0f, offsetW, player.Body);
            weaponFixture.CollisionCategories = BazingaCollisionGroups.PlayerWeapon;

            player.Animation.PlaySprite(SpriteState.Melee, false);

            player.Sounds.PlaySound(SoundEffect, false);
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.A))
            {
                player.Animation.PlaySprite(SpriteState.Melee, false);
            }

            return null;
        }

        public IGameComponentState Update(GameTime gameTime)
        {
            if (player.Animation.AnimationCompleted)
            {
                return new PlayerIdleState();
            }

            return null;
        }

        public void ExitState()
        {
            // Nop
        }
    }
}

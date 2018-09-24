using BazingaGame.Animations;
using BazingaGame.Prefabs;
using BazingaGame.Sounds;
using FarseerPhysics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.States.Player
{
    /// <summary>
    /// Finite State Mchine Pattern
    /// http://gameprogrammingpatterns.com/state.html
    /// </summary>
    public class PlayerRunningState : IGameComponentState
    {
        const float RunForce = 50;
        const float MinTurnAroundVelocity = 5;
        const float MinSlideVelocity = 5;
        const float MaxAttackVelocity = 1;

        private const int FixtureRadiusX = 31;
        private const int FixtureRadiusY = 65;
        private readonly Vector2 FixtureOffset = new Vector2(0, 0);

        private const string SoundEffect = @"Sounds/run";

        private BazingaPlayer player;

        public void EnterState(StatefulGameComponent target)
        {
            player = target as BazingaPlayer;

            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, FixtureOffset);
            player.Body.Awake = true;

            player.Sounds.PlaySound(SoundEffect, false);
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            IGameComponentState newState = null;

            float xForce = 0;
            float yForce = 0;

            if (input.IsKeyDown(Keys.Right))
            {
                if (player.Body.LinearVelocity.X < MinTurnAroundVelocity)
                {
                    xForce = RunForce;
                }

                player.Animation.Effect = SpriteEffects.None;
                player.Animation.PlaySprite(SpriteState.Run, true);
            }
            else if (input.IsKeyDown(Keys.Left))
            {
                if (player.Body.LinearVelocity.X > -MinTurnAroundVelocity)
                {
                    xForce = -RunForce;
                }

                player.Animation.Effect = SpriteEffects.FlipHorizontally;
                player.Animation.PlaySprite(SpriteState.Run, true);
            }

            player.Body.ApplyForce(new Vector2(xForce, yForce));

            if (input.IsKeyDown(Keys.Down) && Math.Abs(player.Body.LinearVelocity.X) >= MinSlideVelocity)
            {
                return new PlayerSlideState();

            }
            else if (input.IsKeyDown(Keys.LeftControl) && Math.Abs(player.Body.LinearVelocity.X) <= MaxAttackVelocity)
            {
                return new PlayerAttackState();
            }
            else if (input.IsKeyDown(Keys.Up))
            {
                return new PlayerJumpState();
            }
            else if (input.IsKeyUp(Keys.Left) && input.IsKeyUp(Keys.Right))
            {
                return new PlayerIdleState();
            }

            return newState;
        }


        public IGameComponentState Update(GameTime gameTime)
        {
            return null;
        }

        public void ExitState()
        {
            //soundEffect.Stop();
        }
    }
}

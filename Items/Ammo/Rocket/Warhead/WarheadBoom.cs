using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;

namespace SpiritMod.Items.Ammo.Rocket.Warhead
{
	public class WarheadBoom : Particle
	{
		private const int _numFrames = 10;
		private int _frame;
		private const int _displayTime = 30;

		public WarheadBoom(Vector2 position, float scale, float rotation = 0)
		{
			Position = position;
			Scale = scale;
			Rotation = rotation;
		}

		public override void Update()
		{
			_frame = (int)(_numFrames * TimeActive / _displayTime);
			if (TimeActive > _displayTime)
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D tex = ParticleHandler.GetTexture(Type);
			Texture2D glow = SpiritMod.instance.GetTexture("Items/Ammo/Rocket/Warhead/WarheadBoom_glow");
			Rectangle DrawFrame = new Rectangle(0, _frame * tex.Height/_numFrames, tex.Width, tex.Height/_numFrames);
			spriteBatch.Draw(tex, Position - Main.screenPosition, DrawFrame, Lighting.GetColor(Position.ToTileCoordinates().X, Position.ToTileCoordinates().Y), Rotation, DrawFrame.Size() / 2, Scale, SpriteEffects.None, 0);
			spriteBatch.Draw(glow, Position - Main.screenPosition, DrawFrame, Color.White, Rotation, DrawFrame.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}

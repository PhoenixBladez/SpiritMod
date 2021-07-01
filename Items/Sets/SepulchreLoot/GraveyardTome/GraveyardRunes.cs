using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class GraveyardRunes : Particle
	{
		private float opacity;
		public int MaxTime;

		private Player player;
		private Vector2 offset;

		private int FadeTime => 40;

		private float FadeRate => 1f / FadeTime;

		private float MinimumOpacity => 0.66f;

		private readonly int frame;

		public override bool UseAdditiveBlend => true;

		public GraveyardRunes(Player owner, Vector2 position, Vector2 velocity, float scale, int maxTime)
		{
			player = owner;
			offset = position;
			Velocity = velocity;
			Scale = scale;
			MaxTime = maxTime;
			Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
			frame = Main.rand.Next(4);
		}

		public override void Update()
		{
			if (TimeActive >= MaxTime || !player.channel)
				opacity -= FadeRate;
			else if (TimeActive < FadeTime && opacity < MinimumOpacity)
				opacity += FadeRate;
			else
				opacity = MinimumOpacity + (float)Math.Sin((TimeActive - FadeTime) / 30f) * 0.3f;

			Color = new Color(255, 0, 0) * opacity;
			Lighting.AddLight(Position, Color.R / 255f, Color.G / 255f, Color.B / 255f);

			Position = player.Center + new Vector2(20 * player.direction, 0) + offset;
			offset += Velocity;

			if (opacity <= 0f || !player.active || player.HeldItem.type != ModContent.ItemType<Graveyard>())
				Kill();
		}

		public override bool UseCustomDraw => true;

		public override void CustomDraw(SpriteBatch spriteBatch)
		{
			Texture2D basetexture = ParticleHandler.GetTexture(Type);

			Rectangle drawframe = new Rectangle(0, frame * basetexture.Height / 4, basetexture.Width, basetexture.Height / 4);
			spriteBatch.Draw(basetexture, Position - Main.screenPosition, drawframe, Color, Rotation, drawframe.Size() / 2, Scale, SpriteEffects.None, 0);
		}
	}
}

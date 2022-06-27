using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	public class Manajinx : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Manajinx Pylon");

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.timeLeft = 360;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.scale = 0.5f;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			float grabrange = (owner.manaMagnet) ? 400 : 100;

			if(Projectile.Distance(owner.Center) < grabrange)
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(owner.Center) * 4, 0.05f);
			else
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.1f);

			if (Projectile.Hitbox.Intersects(owner.Hitbox))
			{
				Projectile.Kill();
				owner.statMana = owner.statManaMax;
				owner.ManaEffect(owner.statMana);
				owner.AddBuff(Mod.Find<ModBuff>("ManajinxBuff").Type, 360);
				SoundEngine.PlaySound(SoundID.Item29, owner.Center);
			}
			if(Projectile.timeLeft > 40)
			{
				if (Projectile.alpha > 0)
					Projectile.alpha -= 7;
				else
				{
					Projectile.alpha = 0;
					Projectile.ai[0] = 1;
				}
			}
			else
			{
				Projectile.alpha += 7;
				if (Projectile.alpha > 230)
					Projectile.Kill();
			}
			if(Main.rand.Next(20) == 0)
			{
				Vector2 spawnpos = Projectile.position + Main.rand.NextVector2Circular(70, 70);
				Vector2 gorevel = (Projectile.position - spawnpos)/45;
				Gore.NewGorePerfect(spawnpos, gorevel, Mod.Find<ModGore>("Gores/StarjinxGore").Type, 0.4f);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.ai[0] == 0)
			{
				Player owner = Main.player[Projectile.owner];
				Texture2D tex = Mod.Assets.Request<Texture2D>("Textures/Medusa_Ray").Value;
				Vector2 scale = new Vector2(Projectile.Distance(owner.Center) / tex.Width, 1) * 0.75f;
				spriteBatch.Draw(tex,
					owner.Center - Main.screenPosition + new Vector2(tex.Size().X * scale.X, 0).RotatedBy(Projectile.AngleFrom(owner.Center))/2,
					null,
					SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 4) * 0.5f * (1 - (Math.Abs(0.5f - Projectile.Opacity) * 2)),
					Projectile.AngleFrom(owner.Center),
					tex.Size()/2,
					scale,
					SpriteEffects.None,
					0);
			}
			for (int i = 0; i < 3; i++)
			{
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Textures/StardustPillarStar").Value,
					Projectile.Center - Main.screenPosition,
					null,
					SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 4) * 0.75f * Projectile.Opacity * (1 - (i * 0.33f)),
					0,
					new Vector2(36, 36),
					Projectile.scale * ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 4 + 1) * 0.8f * (1 + (i * 0.5f)),
					SpriteEffects.None,
					0);
			}

			return false;
		}
	}
}
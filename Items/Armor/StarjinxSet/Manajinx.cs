using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	public class Manajinx : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manajinx Pylon");
		}
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.timeLeft = 360;
			projectile.friendly = true;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.scale = 0.5f;
		}
		public override bool CanDamage() => false;
		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			float grabrange = (owner.manaMagnet) ? 400 : 100;
			if(projectile.Distance(owner.Center) < grabrange)
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(owner.Center) * 4, 0.05f);
			}
			else
			{
				projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.1f);
			}
			if (projectile.Hitbox.Intersects(owner.Hitbox))
			{
				projectile.Kill();
				owner.statMana = owner.statManaMax;
				owner.ManaEffect(owner.statMana);
				owner.AddBuff(mod.BuffType("ManajinxBuff"), 360);
				Main.PlaySound(SoundID.Item29, owner.Center);
			}
			if(projectile.timeLeft > 40)
			{
				if (projectile.alpha > 0)
					projectile.alpha -= 7;
				else
				{
					projectile.alpha = 0;
					projectile.ai[0] = 1;
				}
			}
			else
			{
				projectile.alpha += 7;
				if (projectile.alpha > 230)
					projectile.Kill();
			}
			if(Main.rand.Next(20) == 0)
			{
				Vector2 spawnpos = projectile.position + Main.rand.NextVector2Circular(70, 70);
				Vector2 gorevel = (projectile.position - spawnpos)/45;
				Gore.NewGorePerfect(spawnpos, gorevel, mod.GetGoreSlot("Gores/StarjinxGore"), 0.4f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (projectile.ai[0] == 0)
			{
				Player owner = Main.player[projectile.owner];
				Texture2D tex = mod.GetTexture("Extras/Medusa_Ray");
				Vector2 scale = new Vector2(projectile.Distance(owner.Center) / tex.Width, 1) * 0.75f;
				spriteBatch.Draw(tex,
					owner.Center - Main.screenPosition + new Vector2(tex.Size().X * scale.X, 0).RotatedBy(projectile.AngleFrom(owner.Center))/2,
					null,
					SpiritMod.StarjinxColor(Main.GlobalTime * 4) * 0.5f * (1 - (Math.Abs(0.5f - projectile.Opacity) * 2)),
					projectile.AngleFrom(owner.Center),
					tex.Size()/2,
					scale,
					SpriteEffects.None,
					0);
			}
			for (int i = 0; i < 3; i++)
			{
				spriteBatch.Draw(mod.GetTexture("Extras/StardustPillarStar"),
					projectile.Center - Main.screenPosition,
					null,
					SpiritMod.StarjinxColor(Main.GlobalTime * 4) * 0.75f * projectile.Opacity * (1 - (i * 0.33f)),
					0,
					new Vector2(36, 36),
					projectile.scale * ((float)Math.Sin(Main.GlobalTime * 3) / 4 + 1) * 0.8f * (1 + (i * 0.5f)),
					SpriteEffects.None,
					0);
			}

			return false;
		}
	}
}
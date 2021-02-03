using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class MinifishProj : ModProjectile
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Minifish");

		public override void SetDefaults()
		{
            projectile.width = 8;
            projectile.height = 8;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ranged = true;
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.alpha = 255;
		}
		public override bool CanDamage() => false;

		Vector2 newCenter = Vector2.Zero;
		float offset = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(newCenter);
			writer.Write(offset);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			newCenter = reader.ReadVector2();
			offset = reader.ReadSingle();
		}
		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			Vector2 homeCenter = owner.Center;
			homeCenter.Y -= 50;
			if (Main.myPlayer == owner.whoAmI)
				projectile.ai[0] = Utils.AngleLerp(owner.AngleTo(Main.MouseWorld), projectile.AngleTo(Main.MouseWorld), 0.5f);

			if (projectile.localAI[1] == 0) {
				projectile.rotation = projectile.ai[0];
				newCenter = projectile.Center - homeCenter;
				projectile.localAI[1]++;
				projectile.netUpdate = true;
			}
			var projsofthistype = Main.projectile.Where(x => x.type == projectile.type && x.owner == projectile.owner);
			if (projectile.Distance(homeCenter) > 30) {
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(homeCenter) * 4, 0.05f);
			}
			newCenter += projectile.velocity;
			projectile.Center = homeCenter + newCenter;
			if (owner.HeldItem.type != mod.ItemType("Minifish"))
				projectile.ai[1]++;

			projectile.damage = (int)(owner.HeldItem.damage * owner.rangedDamage * 0.8f);
			projectile.spriteDirection = (Math.Abs(projectile.rotation) > MathHelper.PiOver2) ? -1 : 1;

			if (projectile.ai[1] > 0 || projectile.timeLeft < 30) {
				projectile.alpha += 10;
				if (projectile.alpha >= 255)
					projectile.Kill();
			}
			else {
				projectile.alpha = (projectile.alpha > 0) ? projectile.alpha - 10 : 0;

				projectile.rotation = Utils.AngleLerp(projectile.rotation, projectile.ai[0], 0.05f);
				offset = (owner.itemTime > 0) ? MathHelper.Lerp(offset, 50, 0.07f) : MathHelper.Lerp(offset, 0, 0.07f);

				if (owner.itemTime > 1) {
					if (--projectile.localAI[0] == 0) {
						int shoot = 0;
						float speed = 0;
						float knockback = 0;
						int damage = projectile.damage;
						bool canshoot = true;
						owner.PickAmmo(owner.inventory[owner.selectedItem], ref shoot, ref speed, ref canshoot, ref damage, ref knockback, true);
						if (canshoot && Main.netMode != NetmodeID.MultiplayerClient) {
							Main.PlaySound(SoundID.Item11, projectile.Center);
							Projectile.NewProjectile(projectile.Center + (Vector2.UnitX.RotatedBy(projectile.rotation) * offset), 
								Vector2.UnitX.RotatedBy(projectile.rotation) * (speed + 10), shoot, damage, knockback + projectile.knockBack, projectile.owner);
						}
					}
				}
				else
					projectile.localAI[0] = (projsofthistype.Where(x => x.whoAmI < projectile.whoAmI).Count() + 1) * 4;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center + (Vector2.UnitX.RotatedBy(projectile.rotation) * offset) - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			Color color = projectile.GetAlpha(lightColor);
			SpriteEffects spriteeffects = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(Main.projectileTexture[projectile.type], 
				drawPos, 
				null, 
				color, 
				projectile.rotation - ((projectile.spriteDirection < 0) ? MathHelper.Pi : 0), 
				Main.projectileTexture[projectile.type].Size()/2, 
				projectile.scale, 
				spriteeffects, 
				0f);

			return false;
		}
    }
}

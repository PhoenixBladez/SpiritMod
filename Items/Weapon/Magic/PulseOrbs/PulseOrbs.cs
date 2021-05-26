using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.PulseOrbs
{
	public class PulseOrbs : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 51;
			item.knockBack = 3.3f;
			item.magic = true;
			item.useStyle = 5;
			item.useAnimation = 25;
			item.useTime = 25;
			item.channel = true;
			item.width = 26;
			item.height = 26;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.sellPrice(gold: 2);
			item.rare = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulse Orbs");
			Tooltip.SetDefault("Hold left click to create an electric field");
		}	
		public override void HoldItem(Player player)
		{
			bool orbsActive = false;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.type == ModContent.ProjectileType<PulseOrbProj>() && proj.owner == player.whoAmI)
				{
					orbsActive = true;
					break;
				}
			}
			if (!orbsActive)
			{
				for (int i = 0; i < 3; i++)
				{
					Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<PulseOrbProj>(), (int)(item.damage * player.magicDamage), item.knockBack, player.whoAmI, i *  2.09f, i * 10);
				}
			}
		}
	}
	public class PulseOrbProj : ModProjectile
	{
		bool holdingItem => Main.player[projectile.owner].HeldItem.type == ModContent.ItemType<PulseOrbs>();
		public bool channeling = false;
		float radians {get{return projectile.ai[0];}set{projectile.ai[0] = value;}}
		Vector2 mousePos = Vector2.Zero;
		Vector2 posToBe => mousePos - (radians.ToRotationVector2() * (50 + (25 * (float)Math.Cos(Main.GlobalTime * 3f))));
		int launchCounter;
		bool drawInFront = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulse Orbs");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.magic = true;
			projectile.timeLeft = 2;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.timeLeft = 2;
			if (holdingItem)
			{
				radians += 0.1f;
				if (player.channel)
				{
					if (!channeling)
					{
						if (projectile.ai[1] == 0)
						{
							for (int i = 0; i < Main.projectile.Length; i++)
							{
								Projectile proj = Main.projectile[i];
								if (proj.active && proj.type == projectile.type && proj.owner == player.whoAmI && proj != projectile)
								{
									SpiritMod.primitives.CreateTrail(new PulseOrbPrimTrail(projectile, proj));
								}
							}
						}
						else if (projectile.ai[1] == 10)
						{
							for (int i = 0; i < Main.projectile.Length; i++)
							{
								Projectile proj = Main.projectile[i];
								if (proj.active && proj.type == projectile.type && proj.owner == player.whoAmI && proj != projectile && proj.ai[1] > 15)
								{
									SpiritMod.primitives.CreateTrail(new PulseOrbPrimTrail(projectile, proj));
								}
							}
						}
						channeling = true;
						mousePos = Main.MouseWorld;
					}
					launchCounter--;
					if (launchCounter <= 0)
					{
						Vector2 dir = posToBe - projectile.position;
						float speed = (float)Math.Sqrt(dir.Length());
						dir.Normalize();
						projectile.velocity = dir * speed * 1.5f;
						projectile.scale = 1;
					}
					else
					{
						RotateAroundPlayer(player);
					}
				}
				else
				{
					channeling = false;
					launchCounter = (int)projectile.ai[1];
					RotateAroundPlayer(player);
				}
			}
			else
			{
				Vector2 vel = player.Center - projectile.position;
				if (vel.Length() < 40 || vel.Length() > 1500)
				{
					projectile.active = false;
				}
				vel.Normalize();
				vel *= 20;
				projectile.velocity = vel;
			}
		}

		private void RotateAroundPlayer(Player player)
		{
			Vector2 offset = radians.ToRotationVector2();
			offset.X *= 50;
			offset.Y *= 10;
			if (offset.Y > 0 && Math.Abs(offset.X) < 25)
			{
				player.heldProj = projectile.whoAmI;
				drawHeldProjInFrontOfHeldItemAndArms = true;
			}
			else
			{
				drawHeldProjInFrontOfHeldItemAndArms = false;
			}
			projectile.scale = 1 + (offset.Y * 0.01f);
			Vector2 posToBe = player.Center + offset;
			Vector2 vel = posToBe - projectile.position;
			float speed = (float)Math.Sqrt(vel.Length());
			if (vel.Length() < 20 || vel.Length() > 1500)
			{
				projectile.Center = posToBe;
			}
			else
			{
				vel.Normalize();
				vel *= speed * 1.5f;
				projectile.velocity = vel;
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Vector2 center = new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / 2));
            float num341 = 0f;
            float num340 = projectile.height;
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;

            Texture2D texture2D6 = Main.projectileTexture[projectile.type];
            Vector2 vector15 = new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / 2));
            SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 vector33 = new Vector2(projectile.Center.X, projectile.Center.Y) - Main.screenPosition + new Vector2(0, projectile.gfxOffY) - projectile.velocity;
            Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(127 - projectile.alpha, 127 - projectile.alpha, 127 - projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
            Microsoft.Xna.Framework.Color color28 = color29;
            color28 = projectile.GetAlpha(color28);
            color28 *= 1f - num107;

            Microsoft.Xna.Framework.Color color30 = color29;
            color30 = projectile.GetAlpha(color28);
            color30 *= 1.18f - num107;
            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = new Vector2(projectile.Center.X, projectile.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (3f * num107 + 2f) - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
                Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Magic/PulseOrbs/PulseOrbProjGlow"), vector29, null, color28, projectile.rotation, texture2D6.Size() / 2f, projectile.scale, spriteEffects3, 0f);
            }
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			Player player = Main.player[projectile.owner];
			bool ret = false;
			if (projectile.ai[1] == 0 && channeling)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.type == projectile.type && proj.owner == player.whoAmI && proj != projectile)
					{
						float collisionPoint = 0f;
						Vector2 dir = proj.Center - projectile.Center;
						if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + dir, (projectile.width + projectile.height) * 0.5f * projectile.scale, ref collisionPoint)) 
							ret = true;
					}
				}
			}
			if (ret)
				return true;
			return base.Colliding(projHitbox, targetHitbox);
		}
		
	}
}

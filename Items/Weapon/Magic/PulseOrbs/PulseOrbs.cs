using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SpiritMod.Items.Weapon.Magic.PulseOrbs
{
	public class PulseOrbs : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 41;
			item.knockBack = 3.3f;
			item.magic = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.useTime = 25;
			item.channel = true;
			item.width = 26;
			item.height = 26;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.LightRed;
			item.mana = 2;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulse Orbs");
			Tooltip.SetDefault("Hold left click to create an electric field toward the cursor\nConsumes 20 mana per second");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) => tooltips.RemoveAt(tooltips.FindIndex(x => x.Name == "UseMana" && x.mod == "Terraria")); //So we don't show the 2 mana usage

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
					Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<PulseOrbProj>(), (int)(item.damage * player.magicDamage), item.knockBack, player.whoAmI, i *  2.09f, i * 10);
			}
		}
	}

	public class PulseOrbProj : ModProjectile, ITrailProjectile
	{
		private bool HoldingItem => Main.player[projectile.owner].HeldItem.type == ModContent.ItemType<PulseOrbs>();
		private ref float Radians => ref projectile.ai[0];
		Vector2 PosToBe => mousePos - (Radians.ToRotationVector2() * (50 + (25 * (float)Math.Cos(Main.GlobalTime * 6f))));

		public bool channeling = false;
		Vector2 mousePos = Vector2.Zero;
		private int launchCounter;
		private int manaTimer;

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
			Lighting.AddLight(projectile.Center, new Vector3(0.25f, 0.25f, 0.25f));

			Player player = Main.player[projectile.owner];
			projectile.timeLeft = 2;
			if (HoldingItem)
			{
				Radians += 0.1f;
				if (player.channel)
				{
					if (projectile.soundDelay <= 0) {
						projectile.soundDelay = 10;
						projectile.soundDelay *= 4;
						if (projectile.ai[0] != 1f)
							Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 15);
					}
					if (Main.myPlayer == projectile.owner) {
						if (Main.player[projectile.owner].channel && Main.player[projectile.owner].statMana > 0) {
							manaTimer++;
							if (manaTimer >= 9) {
								manaTimer = 0;
								Main.player[projectile.owner].statMana--;
							}
						}
						if (Main.player[projectile.owner].statMana <= 0)
							projectile.Kill();
					}
					if (!channeling)
					{
						if (projectile.ai[1] == 0)
						{
							for (int i = 0; i < Main.projectile.Length; i++)
							{
								Projectile proj = Main.projectile[i];
								if (proj.active && proj.type == projectile.type && proj.owner == player.whoAmI && proj != projectile)
									SpiritMod.primitives.CreateTrail(new PulseOrbPrimTrail(projectile, proj));
							}
						}
						else if (projectile.ai[1] == 10)
						{
							for (int i = 0; i < Main.projectile.Length; i++)
							{
								Projectile proj = Main.projectile[i];
								if (proj.active && proj.type == projectile.type && proj.owner == player.whoAmI && proj != projectile && proj.ai[1] > 15)
									SpiritMod.primitives.CreateTrail(new PulseOrbPrimTrail(projectile, proj));
							}
						}
						channeling = true;
						mousePos = Main.MouseWorld;
					}
					launchCounter--;
					if (launchCounter <= 0)
					{
						Vector2 dir = PosToBe - projectile.position;
						float speed = (float)Math.Sqrt(dir.Length());
						dir.Normalize();
						projectile.velocity = dir * speed * 2.75f;
						projectile.scale = 1;
					}
					else
						RotateAroundPlayer(player);
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
					projectile.active = false;
				vel.Normalize();
				vel *= 25;
				projectile.velocity = vel;
			}
		}

		private void RotateAroundPlayer(Player player)
		{
			Vector2 offset = Radians.ToRotationVector2();
			offset.X *= 50;
			offset.Y *= 10;
			if (offset.Y > 0 && Math.Abs(offset.X) < 25)
			{
				player.heldProj = projectile.whoAmI;
				drawHeldProjInFrontOfHeldItemAndArms = true;
			}
			else
				drawHeldProjInFrontOfHeldItemAndArms = false;
			projectile.scale = 1 + (offset.Y * 0.01f);
			Vector2 posToBe = player.Center + offset;
			Vector2 vel = posToBe - projectile.position;
			float speed = (float)Math.Sqrt(vel.Length());
			if (vel.Length() < 20 || vel.Length() > 1500)
				projectile.Center = posToBe;
			else
			{
				vel.Normalize();
				vel *= speed * 1.5f;
				projectile.velocity = vel;
			}
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			var sh = new ImageShader(mod.GetTexture("Textures/Trails/CrystalTrail"), 0.01f, 1f, 1f);
			tManager.CreateTrail(projectile, new GradientTrail(new Color(196, 48, 255), new Color(163, 11, 140)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, sh);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;

            Texture2D texture2D6 = Main.projectileTexture[projectile.type];
            SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Color color29 = new Color(127 - projectile.alpha, 127 - projectile.alpha, 127 - projectile.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
            Color color28 = color29;
            color28 = projectile.GetAlpha(color28);
            color28 *= 1f - num107;

            Color color30 = projectile.GetAlpha(color28);
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

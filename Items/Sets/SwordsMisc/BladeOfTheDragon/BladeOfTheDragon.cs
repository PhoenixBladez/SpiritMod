﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.SwordsMisc.BladeOfTheDragon
{
    public class BladeOfTheDragon : ModItem
    {
		public int combo;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade of the Dragon");
            Tooltip.SetDefault("Hold and release to slice through nearby enemies\nBuild up a combo be repeatedly hitting enemies");
            Item.staff[item.type] = true;
		}

		public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 100;
            item.width = 60;
            item.height = 60;
            item.useTime = 60;
            item.useAnimation = 60;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 1;
            item.useTurn = false;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.LightPurple;
			item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<BladeOfTheDragonProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void HoldItem(Player player) => player.noFallDmg = true;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
			var modProj = proj.modProjectile as BladeOfTheDragonProj;
			if (combo % 4 != 0)
				modProj.charge = 39;
			return false;
		}
	}

    public class BladeOfTheDragonProj : ModProjectile
    {
		const int DISTANCE = 700;

		Vector2 direction = Vector2.Zero;
		Vector2 startPos = Vector2.Zero;
		Vector2 endPos = Vector2.Zero;
		Vector2 oldCenter = Vector2.Zero;

		bool comboActivated;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Blade of the Dragon");

		public override void SetDefaults()
		{
            projectile.width = projectile.height = 40;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
            projectile.alpha = 255;
			projectile.timeLeft = 240;
		}

        public readonly int MAXCHARGE = 56;
        public int charge = 0;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.Center = player.Center;

            if (player.channel && projectile.timeLeft > 237)
            {
				player.heldProj = projectile.whoAmI;
				player.itemTime = 5;
				player.itemAnimation = 5;
				projectile.timeLeft = 240;
                charge++;

                if (charge < 40)
                    charge++;

                if (charge == 40)
                {
					startPos = player.Center;
					direction = Main.MouseWorld - (player.Center);
					direction.Normalize();
					direction *= DISTANCE;
					endPos = player.Center + direction;
					Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/slashdash").WithPitchVariance(0.4f).WithVolume(0.4f), projectile.Center);
					SpiritMod.primitives.CreateTrail(new DragonPrimTrail(projectile));
					oldCenter = player.Center;
				}

                if (charge > 40 && charge < MAXCHARGE)
                {
					float progress = (charge - 41) / (float)(MAXCHARGE - 41);
					progress = EaseFunction.EaseCircularInOut.Ease(progress);

					float nextProgress = (charge - 40) / (float)(MAXCHARGE - 41);
					nextProgress = EaseFunction.EaseCircularInOut.Ease(nextProgress);

					var nextPoint = Vector2.Lerp(startPos, endPos, nextProgress);
					var currentPoint = Vector2.Lerp(startPos, endPos, progress);

					float oldSpeed = player.velocity.Length();
					if (oldSpeed > 2)
					{
						for (int i = 0; i < oldSpeed / 7f; i++)
						{
							var line = new ImpactLine(Vector2.Lerp(oldCenter, projectile.Center, Main.rand.NextFloat()) + Main.rand.NextVector2Circular(35, 35), Vector2.Normalize(player.velocity) * 0.5f, Color.Lerp(Color.Green, Color.LightGreen, Main.rand.NextFloat()), new Vector2(0.25f, Main.rand.NextFloat(0.5f, 1.5f)) * 3, 60);
							line.TimeActive = 30;
							ParticleHandler.SpawnParticle(line);
						}
					}

					player.velocity = nextPoint - currentPoint;

					player.GetModPlayer<MyPlayer>().AnimeSword = true;
					player.GetModPlayer<DragonPlayer>().DrawSparkle = true;

					oldCenter = player.Center;
                }
                if (charge == MAXCHARGE)
                {
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
					player.velocity = Vector2.Zero;
					projectile.Kill();
                }
            }
            else
            {
				if (charge > 40 && charge < MAXCHARGE)
				{
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
					player.velocity = Vector2.Zero;
				}
				projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
			float colissionPoint = 0f;
            if (charge > 40 && charge < MAXCHARGE)
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center, player.oldPosition + (new Vector2(player.width,player.height) / 2), 60, ref colissionPoint);
            return false;
        }

        public override bool? CanHitNPC(NPC target)
		{
            if (charge > 40 && charge < MAXCHARGE)
				return base.CanHitNPC(target);
			return false;
        }

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];

			if (player.HeldItem.modItem is BladeOfTheDragon modItem)
			{
				if (!comboActivated)
					modItem.combo = 0;
				else
					modItem.combo++;
			}

			Main.player[projectile.owner].GetModPlayer<MyPlayer>().AnimeSword = false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			comboActivated = true;
			Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<DragonSlash>(), 0, 0, projectile.whoAmI, target.whoAmI);
		}
	}

	internal class DragonSlash : ModProjectile, IDrawAdditive
	{
		int frameX = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Slash");
			Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(88, 123);
			projectile.penetrate = -1;
			projectile.hide = true;
		}

		public override void AI()
		{
			if (projectile.frameCounter == 0)
			{
				frameX = Main.rand.Next(2);
				projectile.rotation = Main.rand.NextFloat(6.28f);
			}

			projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
			projectile.velocity = Vector2.Zero;
			projectile.frameCounter++;

			if (projectile.frameCounter % 5 == 0)
				projectile.frame++;
			if (projectile.frame >= Main.projFrames[projectile.type])
				projectile.active = false;
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			//Adjust framing due to secondary column
			Rectangle frame = projectile.DrawFrame();
			frame.Width /= 2;
			frame.X = frame.Width * frameX;

			void Draw(Vector2 offset, float opacity)
			{
				sB.Draw(Main.projectileTexture[projectile.type], projectile.Center + offset - Main.screenPosition, frame, 
					Color.White * opacity, projectile.rotation, frame.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			}

			PulseDraw.DrawPulseEffect((float)Math.Asin(-0.6), 8, 12, delegate (Vector2 posOffset, float opacityMod)
			{
				Draw(posOffset, opacityMod * 0.33f);
			});
			Draw(Vector2.Zero, 1);
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}

	public class DragonPlayer : ModPlayer
	{
		public bool DrawSparkle;

		public override void ResetEffects()
		{
			if (!player.channel)
				DrawSparkle = false;
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			if (player.HeldItem.type == ModContent.ItemType<BladeOfTheDragon>())
			{
				layers.Insert(layers.FindIndex(x => x.Name == "HeldItem" && x.mod == "Terraria"), new PlayerLayer(mod.Name, "BladeOfTheDragonHeld",
					delegate (PlayerDrawInfo info) {
						DrawItem(mod.GetTexture("Items/Sets/SwordsMisc/BladeOfTheDragon/BladeOfTheDragon_held"), mod.GetTexture("Items/Sets/SwordsMisc/BladeOfTheDragon/BladeOfTheDragon_sparkle"), info);
					}));
			}
		}

		public static void DrawItem(Texture2D texture, Texture2D sparkle, PlayerDrawInfo info)
		{
			Item item = info.drawPlayer.HeldItem;
			if (info.shadow != 0f || info.drawPlayer.frozen || info.drawPlayer.dead || (info.drawPlayer.wet && item.noWet))
				return;

			Rectangle drawFrame = texture.Bounds;
			int numFrames = 2;
			drawFrame.Height /= numFrames;
			drawFrame.Y = (drawFrame.Height) * (info.drawPlayer.channel ? 1 : 0);

			Vector2 offset = new Vector2(6, texture.Height / (2 * numFrames));

			ItemLoader.HoldoutOffset(info.drawPlayer.gravDir, item.type, ref offset);
			Vector2 origin = new Vector2(texture.Width * 0.75f, texture.Height * 0.2f);

			offset = new Vector2(6, offset.Y);
			if (info.drawPlayer.direction == -1)
			{
				origin.X = texture.Width - origin.X;
				offset.X = -6;
			}

			Main.playerDrawData.Add(new DrawData(
				texture,
				info.drawPlayer.Center - Main.screenPosition + offset,
				drawFrame,
				Lighting.GetColor((int)info.drawPlayer.Center.X / 16, (int)info.drawPlayer.Center.Y / 16),
				0,
				origin,
				item.scale,
				info.spriteEffects,
				0
			));

			offset.X = 0;
			if (info.drawPlayer.GetModPlayer<DragonPlayer>().DrawSparkle)
			{
				Main.playerDrawData.Add(new DrawData(
					sparkle,
					info.drawPlayer.Center - Main.screenPosition + offset - new Vector2(0, 9),
					null,
					Color.White,
					Main.GlobalTime * 0.5f,
					sparkle.Size() / 2,
					item.scale,
					SpriteEffects.None,
					0
				));
			}
		}
	}
}
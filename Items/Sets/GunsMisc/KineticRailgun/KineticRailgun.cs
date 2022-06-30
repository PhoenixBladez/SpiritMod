using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.KineticRailgun
{
	public class KineticRailgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Cannon");
			Tooltip.SetDefault("Consumes gel\nDoes more damage to single targets \n'Zap your enemies to oblivion!'");
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.width = 65;
			Item.height = 21;
			Item.useTime = 65;
			Item.useAnimation = 65;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<KineticRailgunProj>();
			Item.shootSpeed = 20f;
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.useAmmo = AmmoID.Gel;
			Item.UseSound = SoundID.DD2_SkyDragonsFuryShot;
		}

		public override bool CanConsumeAmmo(Item item, Player player) => false;
		public override Vector2? HoldoutOffset() => new Vector2(-15, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FragmentVortex, 18);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	public class KineticRailgunProj : ModProjectile
	{
		const int RANGE = 600;
		const float CONE = 0.7f;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Tesla Cannon");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 999999;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Main.projFrames[Projectile.type] = 4;
		}

		public Vector2 direction = Vector2.Zero;
		int counter;
		public List<NPC> targets = new List<NPC>();

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

			if (Projectile.owner != Main.myPlayer)
				return;

			player.itemTime = 5; // Set item time to 2 frames while we are used
			player.itemAnimation = 5; // Set item animation time to 2 frames while we are used
			direction = Vector2.Normalize(Main.MouseWorld - player.Center) * 70f;
			Projectile.position = player.Center + direction;
			Projectile.velocity = Vector2.Zero;
			player.itemRotation = direction.ToRotation();
			player.heldProj = Projectile.whoAmI;

			if (player.direction != 1)
				player.itemRotation -= 3.14f;

			if (player.channel)
			{
				if (Projectile.soundDelay <= 0 && targets.Count > 0) //Create sound & use ammo
				{
					Projectile.soundDelay = 20;
					SoundEngine.PlaySound(SoundID.Item15, Projectile.position);
					player.PickAmmo(player.HeldItem, out int _, out float _, out int _, out float _, out int _, false);
				}

				Projectile.timeLeft = 2;
				counter++;
				Projectile.frame = (counter / 5) % 4;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					Vector2 toNPC = npc.Center - player.Center;

					if (toNPC.Length() < RANGE && npc.CanBeChasedBy() && AnglesWithinCone(toNPC.ToRotation(), direction.ToRotation()))
					{
						bool targetted = false;

						foreach (var npc2 in targets)
						{
							if (npc2.active)
							{
								if (npc2 == npc)
									targetted = true;
							}
						}

						if (!targetted)
						{
							SpiritMod.primitives.CreateTrail(new RailgunPrimTrail(Projectile, npc));
							targets.Add(npc);
							npc.GetGlobalNPC<TeslaCannonGNPC>().charging = true;
						}
					}
					else
					{
						if (npc.active)
							npc.GetGlobalNPC<TeslaCannonGNPC>().charging = false;
						targets.Remove(npc);
					}
				}

				foreach (var npc2 in targets.ToArray())
				{
					if (!npc2.active)
						targets.Remove(npc2);
				}
			}
			else
				Projectile.active = false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[Projectile.owner];
			Vector2 toNPC = targetHitbox.Center.ToVector2() - player.Center;
			return toNPC.Length() < RANGE && AnglesWithinCone(toNPC.ToRotation(), direction.ToRotation());
		}

		public override bool? CanHitNPC(NPC target)
		{
			foreach (var npc2 in targets)
			{
				if (npc2.active && npc2 == target && npc2.GetGlobalNPC<TeslaCannonGNPC>().charge > 5)
					return null;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(target.position, target.width, target.height, DustID.Electric);
				Main.dust[dust].scale = 0.8f;
			}

			if (target.life <= 0)
				Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<VortexExplosion>(), 0, 0);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (targets.Count == 1)
				damage *= 2;
		}

		private bool AnglesWithinCone(float angle1, float angle2)
		{
			if (Math.Abs(MathHelper.WrapAngle(angle1) % 6.28f - MathHelper.WrapAngle(angle2) % 6.28f) < CONE)
				return true;
			if (Math.Abs((MathHelper.WrapAngle(angle1) % 6.28f - MathHelper.WrapAngle(angle2) % 6.28f) + 6.28f) < CONE)
				return true;
			if (Math.Abs((MathHelper.WrapAngle(angle1) % 6.28f - MathHelper.WrapAngle(angle2) % 6.28f) - 6.28f) < CONE)
				return true;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D proj = ModContent.Request<Texture2D>("Items/Sets/GunsMisc/KineticRailgun/KineticRailgunProj", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Texture2D glow = ModContent.Request<Texture2D>("Items/Sets/GunsMisc/KineticRailgun/KineticRailgunProj_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			if (player.direction == 1)
			{
				SpriteEffects effects1 = SpriteEffects.None;
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
				int height = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
				int y2 = height * Projectile.frame;
				Vector2 position = (Projectile.position - (0.5f * direction) + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
				Main.spriteBatch.Draw(proj, position, new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height), lightColor, direction.ToRotation(), new Vector2((float)texture.Width / 2f, (float)height / 2f), Projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(glow, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, direction.ToRotation(), new Vector2((float)texture.Width / 2f, (float)height / 2f), Projectile.scale, effects1, 0.0f);
			}
			else if (player.direction != 1)
			{
				SpriteEffects effects1 = SpriteEffects.FlipHorizontally;
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
				int height = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
				int y2 = height * Projectile.frame;
				Vector2 position = (Projectile.position - (0.5f * direction) + new Vector2((float)Projectile.width, (float)Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition).Floor();
				Main.spriteBatch.Draw(proj, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, direction.ToRotation() - 3.14f, new Vector2((float)texture.Width / 2f, (float)height / 2f), Projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(glow, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, direction.ToRotation() - 3.14f, new Vector2((float)texture.Width / 2f, (float)height / 2f), Projectile.scale, effects1, 0.0f);
			}
			return false;
		}
	}

	internal class VortexExplosion : ModProjectile
	{

		int frameX = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Explosion");
			Main.projFrames[Projectile.type] = 7;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(166, 158);
			Projectile.penetrate = -1;
		}
		public override void AI()
		{
			if (Projectile.frameCounter == 0)
			{
				frameX = Main.rand.Next(2);
			}
			Projectile.velocity = Vector2.Zero;
			Projectile.frameCounter++;
			if (Projectile.frameCounter % 4 == 0)
				Projectile.frame++;
			if (Projectile.frame >= Main.projFrames[Projectile.type])
				Projectile.active = false;

		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle((tex.Width / 2) * frameX, frameHeight * Projectile.frame, tex.Width / 2, frameHeight);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, new Vector2(tex.Width / 4, frameHeight / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
	public class TeslaCannonGNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public int charge;
		public bool charging;

		public override void ResetEffects(NPC npc)
		{
			if (!charging)
				charge = 0;
			else
				charge++;
		}
	}
}
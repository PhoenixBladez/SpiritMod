using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Weapon.Gun.KineticRailgun
{
	public class KineticRailgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Cannon");
			Tooltip.SetDefault("Consumes gel\n'Zap your enemies to oblivion!'");
		}

		public override void SetDefaults()
		{
			item.damage = 480;
			item.width = 65;
			item.height = 21;
			item.useTime = 65;
			item.useAnimation = 65;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Red;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<KineticRailgunProj>();
			item.shootSpeed = 20f;
			item.ranged = true;
			item.channel = true;
			item.noUseGraphic = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-15, 0);
		}
	}
	public class KineticRailgunProj : ModProjectile
	{
		const int RANGE = 500;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Cannon");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 999999;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			Main.projFrames[projectile.type] = 4;
		}
		public Vector2 direction = Vector2.Zero;
		int counter;
		 public List<NPC> targets = new List<NPC>();
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			player.ChangeDir(Main.MouseWorld.X > player.position.X ? 1 : -1);

			player.itemTime = 5; // Set item time to 2 frames while we are used
			player.itemAnimation = 5; // Set item animation time to 2 frames while we are used
			direction = Main.MouseWorld - (player.Center);
			direction.Normalize();
			direction *= 70f;
			projectile.position = player.Center + direction;
			projectile.velocity = Vector2.Zero;
			player.itemRotation = direction.ToRotation();
			player.heldProj = projectile.whoAmI;
			if (player.direction != 1)
			{
				player.itemRotation -= 3.14f;
			}

			if (player.channel)
			{
				projectile.timeLeft = 2;
				counter++;
				projectile.frame = (counter / 5) % 4;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc = Main.npc[i];
					Vector2 toNPC = npc.Center - player.Center;
					if (toNPC.Length() < RANGE && npc.active && Math.Abs(MathHelper.WrapAngle(toNPC.ToRotation()) % 6.28f - MathHelper.WrapAngle(direction.ToRotation()) % 6.28f) < 0.5f)
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
						if (!targetted && Main.rand.Next(15) == 0)
						{
							SpiritMod.primitives.CreateTrail(new RailgunPrimTrail(projectile, npc));
							targets.Add(npc);
						}
					}
					else
					{
						if (npc.active)
							npc.GetGlobalNPC<RailNPC>().charge = 0;
						targets.Remove(npc);
					}
				}
				foreach (var npc2 in targets.ToArray())
				{
					if (!npc2.active)
						targets.Remove(npc2);
					npc2.GetGlobalNPC<RailNPC>().charge++;
				}
			}
			else
			{
				foreach (var npc2 in targets.ToArray())
				{
					if (npc2.active)
						npc2.GetGlobalNPC<RailNPC>().charge = 0;
				}
				projectile.active = false;
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Player player = Main.player[projectile.owner];
            Vector2 toNPC = targetHitbox.Center.ToVector2() - player.Center;
			return toNPC.Length() < RANGE && Math.Abs(MathHelper.WrapAngle(toNPC.ToRotation()) % 6.28f - MathHelper.WrapAngle(direction.ToRotation()) % 6.28f) < 0.5f;
        }
		public override bool? CanHitNPC(NPC target)
		{
			foreach (var npc2 in targets)
			{
				if (npc2.active)
				{
					if (npc2 == target)
						return target.GetGlobalNPC<RailNPC>().charge % 33 >= 29;
				}
			}
			return false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(target.position, target.width, target.height, 226);
				Main.dust[dust].scale = 0.8f;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if (player.direction == 1)
			{
				SpriteEffects effects1 = SpriteEffects.None;
				Texture2D texture = Main.projectileTexture[projectile.type];
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y2 = height * projectile.frame;
				Vector2 position = (projectile.position - (0.5f * direction) + new Vector2((float) projectile.width, (float) projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();
				float num1 = 1f;
				Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Gun/KineticRailgun/KineticRailgunProj"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, direction.ToRotation(), new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, effects1, 0.0f);
				Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Gun/KineticRailgun/KineticRailgunProj_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, direction.ToRotation(), new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, effects1, 0.0f);
			}
			else if (player.direction != 1)
			{
				SpriteEffects effects1 = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.projectileTexture[projectile.type];
				int height = Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type];
				int y2 = height * projectile.frame;
				Vector2 position = (projectile.position - (0.5f * direction) + new Vector2((float) projectile.width, (float) projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();
				float num1 = 1f;
				Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Gun/KineticRailgun/KineticRailgunProj"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), lightColor, direction.ToRotation() - 3.14f, new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, effects1, 0.0f); 
				Main.spriteBatch.Draw(mod.GetTexture("Items/Weapon/Gun/KineticRailgun/KineticRailgunProj_Glow"), position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture.Width, height)), Color.White, direction.ToRotation() - 3.14f, new Vector2((float) texture.Width / 2f, (float) height / 2f), projectile.scale, effects1, 0.0f); 
			}
			return false;
		}
	}
	public class RailNPC: GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public int charge = 0;
	}
}
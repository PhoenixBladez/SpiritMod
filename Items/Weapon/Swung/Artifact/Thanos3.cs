using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
	public class Thanos3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard of Thanos");
			Tooltip.SetDefault("Occasionally shoots out an afterimage of the Shard\nRight-click to summon three storms of rotating crystals around the player\nMelee or afterimage attacks may crystallize enemies, stopping them in place\nHit enemies may release multiple Ancient Crystal Shards");

		}


		public override void SetDefaults()
		{
			item.damage = 64;
			item.melee = true;
			item.width = 46;
			item.height = 44;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 8, 0, 50);
			item.shoot = mod.ProjectileType("Thanos3Proj");
			item.rare = 7;
			item.shootSpeed = 9f;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
		}

		public override void HoldItem(Player player)
		{
			if (player.GetModPlayer<MyPlayer>(mod).Resolve)
			{
				player.AddBuff(mod.BuffType("Resolve"), 2);
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("Crystal"));
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(12) == 1)
			{
				target.AddBuff(mod.BuffType("Crystallize"), 180);
			}
			if (Main.rand.Next(6) == 1)
			{
				for (int h = 0; h < 6; h++)
				{
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * MathHelper.TwoPi;
					vel = vel.RotatedBy(rand);
					vel *= 8f;
					Projectile.NewProjectile(player.position.X, player.position.Y, vel.X, vel.Y, mod.ProjectileType("AncientCrystal"), (int)(damage*.875f), knockBack * .2f, player.whoAmI);

				}
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				float kb = knockBack * .2f;
				int shield = (int)(damage * .25f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y -45, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X - 45, player.Center.Y +45, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y +40, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y -40, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), shield, knockBack, player.whoAmI);

				shield = (int)(damage * .42f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), shield, knockBack, player.whoAmI);

				shield = (int)(damage * .6f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), shield, knockBack, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), shield, knockBack, player.whoAmI);
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Thanos2", 1);
			recipe.AddIngredient(null, "SearingBand", 1);
			recipe.AddIngredient(null, "CursedMedallion", 1);
			recipe.AddIngredient(null, "DarkCrest", 1);
			recipe.AddIngredient(null, "BatteryCore", 1);
			recipe.AddIngredient(null, "PrimordialMagic", 100);
			recipe.AddTile(null, "CreationAltarTile");
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
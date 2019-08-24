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
	public class Thanos4 : ModItem
	{
		int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard of Thanos");
			Tooltip.SetDefault("'You have become the Artifact Keeper of Thanos'\nShoots out an afterimage of the Shard\nRight-click to summon four storms of rotating crystals around the player\nMelee or afterimage attacks may crystallize enemies, stopping them in place\nHit enemies may release multiple Ancient Crystal Shards\nAttacks with the Shard may cause multiple bolts of Primordial Energy to rain toward the cursor position");

		}


		public override void SetDefaults()
		{
			item.damage = 107;
			item.melee = true;
			item.width = 52;
			item.height = 50;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 8;
			item.value = Item.sellPrice(0, 11, 0, 50);
			item.shoot = mod.ProjectileType("Thanos4Proj");
			item.rare = 10;
			item.shootSpeed = 9f;
			item.UseSound = SoundID.Item69;
			item.autoReuse = true;
			item.useTurn = true;
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
			foreach (TooltipLine line2 in tooltips)
			{
				if (line2.mod == "Terraria" && line2.Name == "ItemName")
				{
					line2.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
			}
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("Crystal"));
			Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
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
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 8f;
					Projectile.NewProjectile(player.position.X, player.position.Y, vel.X, vel.Y, mod.ProjectileType("AncientCrystal"), (int)(damage * .65625f), knockBack * .2f, player.whoAmI, 0f, 0f);

				}
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				int dmg = (int)(damage * .28125f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y -45, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X - 45, player.Center.Y +45, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y +40, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y -40, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), dmg, 0, player.whoAmI);

				dmg = (int)(damage * .375f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos2Crystal"), dmg, 0, player.whoAmI);

				dmg = (int)(damage * .5625f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos3Crystal"), dmg, 0, player.whoAmI);

				dmg = (int)(damage * .8125f);
				Projectile.NewProjectile(player.Center.X - 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos4Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 40, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos4Crystal"), dmg, 0, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 45, player.Center.Y - 45, speedX, speedY, mod.ProjectileType("Thanos4Crystal"), dmg, 0, player.whoAmI);
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Thanos3", 1);
			recipe.AddIngredient(null, "RadiantEmblem", 1);
			recipe.AddIngredient(null, "PlanteraBloom", 1);
			recipe.AddIngredient(null, "ApexFeather", 1);
			recipe.AddIngredient(null, "UnrefinedRuneStone", 1);
			recipe.AddIngredient(null, "Catalyst", 1);
			recipe.AddIngredient(null, "PrimordialMagic", 150);
			recipe.AddTile(null, "CreationAltarTile");
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
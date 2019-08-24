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
	public class Thanos1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard of Thanos");
			Tooltip.SetDefault("'As old as the dawn of Man'\nShoots out an afterimage of the Shard\nRight-click to summon a storm of rotating crystals around the player");

		}


		public override void SetDefaults()
		{
			item.damage = 22;
			item.melee = true;
			item.width = 42;
			item.height = 40;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 3, 0, 50);
			item.shoot = mod.ProjectileType("Thanos1Proj");
			item.rare = 2;
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

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				float kb = knockBack * .2f;
				Projectile.NewProjectile(player.Center.X - 100, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 100, player.Center.Y, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X + 115, player.Center.Y -115, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X - 115, player.Center.Y +115, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y +110, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), damage, kb, player.whoAmI);
				Projectile.NewProjectile(player.Center.X, player.Center.Y -110, speedX, speedY, mod.ProjectileType("Thanos1Crystal"), damage, kb, player.whoAmI);
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OddKeystone", 1);
			recipe.AddIngredient(null, "RootPod", 1);
			recipe.AddIngredient(null, "GildedIdol", 1);
			recipe.AddIngredient(null, "DemonLens", 1);
			recipe.AddIngredient(null, "PrimordialMagic", 50);
			recipe.AddTile(null, "CreationAltarTile");
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

		}
	}
}
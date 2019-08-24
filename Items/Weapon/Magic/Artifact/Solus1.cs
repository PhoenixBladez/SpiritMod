using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace SpiritMod.Items.Weapon.Magic.Artifact
{
	public class Solus1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solus");
			Tooltip.SetDefault("'The ancient companion of Nox'\nShoots out three Phoenix Bolts that inflict 'Blaze'\nPhoenix Bolts explode upon hitting foes");
		}


		public override void SetDefaults()
		{
			item.damage = 15;
			item.magic = true;
			item.mana = 10;
			item.width = 42;
			item.height = 48;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 3, 0, 50);
			item.rare = 2;
			item.crit = 3;
			item.UseSound = SoundID.Item74;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PhoenixBolt");
			item.shootSpeed = 1f;
		}
		public override void HoldItem(Player player)
		{

			if (player.GetModPlayer<MyPlayer>(mod).HolyGrail)
			{
				player.AddBuff(mod.BuffType("Righteous"), 2);
			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(60, 0, 230);
			tooltips.Add(line);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int I = 0; I < 3; I++)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), mod.ProjectileType("PhoenixBolt"), damage, knockBack, player.whoAmI);
			}
			return false;
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

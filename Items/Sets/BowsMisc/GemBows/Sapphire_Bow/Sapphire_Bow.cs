﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Sapphire_Bow
{
	public class Sapphire_Bow : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 27;
			item.useTime = 27;
			item.width = 12;
			item.height = 28;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 12;
			item.shootSpeed = 8f;
			item.knockBack = 0.5f;
			item.rare = ItemRarityID.Blue;
			item.noMelee = true;
            item.value = Item.sellPrice(0, 0, 67, 50);
            item.ranged = true;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Bow");
			Tooltip.SetDefault("Turns wooden arrows into sapphire arrows\nSapphire arrows slightly home toward the cursor");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Sapphire_Arrow>();
							
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBow, 1);
			recipe.AddIngredient(ItemID.Sapphire, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.LeadBow, 1);
			recipe1.AddIngredient(ItemID.Sapphire, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}
}

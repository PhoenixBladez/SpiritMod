﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Clubs;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class GoldClub : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Golden Greathammer");

		public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 20;
            item.width = 58;
            item.height = 58;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
			item.useTurn = true;
			item.value = Item.sellPrice(0, 0, 22, 0);
            item.rare = ItemRarityID.Blue;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<GoldClubProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
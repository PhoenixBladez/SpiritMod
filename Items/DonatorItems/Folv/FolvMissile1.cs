using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	public class FolvMissile1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Arcane Bolt");
			Tooltip.SetDefault("Shoots out a homing missile \n ~Donator Item~");
		}


		public override void SetDefaults()
		{
			item.damage = 13;
			item.magic = true;
			item.mana = 6;
			item.width = 28;
			item.height = 30;
			item.useTime = 26;
			item.useAnimation = 26;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
			item.value = 5400;
			item.rare = 1;
			item.UseSound = SoundID.Item8;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("FolvBolt1");
			item.shootSpeed = 6f;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddIngredient(ItemID.Amethyst, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
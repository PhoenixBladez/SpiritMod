using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	public class FolvMissile2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Magic Missiles");
			Tooltip.SetDefault("Shoots out stationary bolts of energy that seek out nearby enemies \n ~Donator Item~");
		}


		public override void SetDefaults()
		{
			item.damage = 41;
			item.magic = true;
			item.mana = 11;
			item.width = 28;
			item.height = 30;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
			item.value = 15400;
			item.rare = 4;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("FolvBolt2");
			item.shootSpeed = 0f;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FolvMissile1", 1);
            recipe.AddIngredient(ItemID.SorcererEmblem, 1);
            recipe.AddIngredient(ItemID.CrystalShard, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
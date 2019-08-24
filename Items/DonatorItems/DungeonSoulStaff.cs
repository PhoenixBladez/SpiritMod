using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class DungeonSoulStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Soul Staff");
            Tooltip.SetDefault("~ Donator Item ~\nSummons a friendly Dungeon Spirit to latch on to your foes");

        }


		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.mana = 11;
            item.damage = 48;
            item.knockBack = 1;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("DungeonSummon");
            item.buffType = mod.BuffType("DungeonSummonBuff");
            item.buffTime = 3600;
            item.UseSound = SoundID.Item44;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritStaff", 1);
            recipe.AddIngredient(null, "SpiritBar", 4);
            recipe.AddIngredient(ItemID.Ectoplasm, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
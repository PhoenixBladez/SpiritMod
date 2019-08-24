using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class AssassinLight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beacon of the Assassin");
			Tooltip.SetDefault("Increases ranged damage by 8% and arrow damage by 4% when moving\nIncreases bullet damage by 4% and ranged damage and movement speed by 10% when submerged in liquid\nIncreases defense by 2 when not submerged and increases ranged critical strike chance by 5%\nRanged projectiles emit light, inflict a multitude of debuffs, and may bathe enemies in light\nRanged weapons may shoot out extra projectiles and fiery spit");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
			item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 8;
            item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            if (player.wet)
            {
                player.rangedDamage += .10f;
                player.moveSpeed += .10f;
            }
            else
            {
                player.statDefense += 2;
            }
            player.GetModPlayer<MyPlayer>(mod).crystal = true;
            player.GetModPlayer<MyPlayer>(mod).geodeRanged = true;
            player.GetModPlayer<MyPlayer>(mod).anglure = true;
            player.GetModPlayer<MyPlayer>(mod).fireMaw = true;

            player.rangedDamage += 0.08f;

            if (player.velocity.X != 0)
            {
                player.arrowDamage += 0.04f;
            }
            else if (player.velocity.Y != 0)
            {
                player.arrowDamage += 0.04f;
            }
            else
            {
                player.bulletDamage += 0.04f;
            }
            player.rangedCrit += 5;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AssassinMagazine", 1);
            recipe.AddIngredient(null, "HellEater", 1);
            recipe.AddIngredient(null, "JeweledSlime", 1);
            recipe.AddIngredient(null, "Angelure", 1);
            recipe.AddIngredient(null, "Sharkon", 1);
            recipe.AddIngredient(null, "Crystal", 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }
}

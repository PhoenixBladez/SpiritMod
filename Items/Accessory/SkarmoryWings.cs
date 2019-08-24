using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Wings)]
    public class SkarmoryWings : ModItem
    {
        public int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bladewings");
			Tooltip.SetDefault("'It seems to originate from a metallic organism of unknown origin'\nGrants flight and slow fall\nLeaves behind a trail of quicksilver that homes in on foes");
		}
        public override void SetDefaults()
        {
            item.width = 47;
            item.height = 37;
            item.value = 60000;
            item.rare = 5;

            item.accessory = true;

            item.rare = 8;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 137;
            if (Main.rand.Next(4) == 0)
            {

                Dust.NewDust(player.position, player.width, player.height, DustID.SilverCoin);
            }
            timer++;
            if (timer == 200)
            {
                int proj2 = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("QuicksilverBolt"), 40, 0, Main.myPlayer);
                timer = 0;
            }

        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
    ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
			ascentWhenFalling = 0.65f;
			ascentWhenRising = 0.075f;
			maxCanAscendMultiplier = 1.1f;
			maxAscentMultiplier = 2.3f;
			constantAscend = 0.095f;
		}

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 9.5f;
			acceleration *= 1f;
		}  
public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.CopperShortsword, 1);
            modRecipe.AddIngredient(ItemID.TungstenBar, 5);
            modRecipe.AddIngredient(null, "Material", 5);
            modRecipe.AddIngredient(575, 20);
			 modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();

            ModRecipe modRecipe1 = new ModRecipe(mod);
            modRecipe1.AddIngredient(ItemID.CopperShortsword, 1);
            modRecipe1.AddIngredient(ItemID.SilverBar, 5);
            modRecipe1.AddIngredient(null, "Material", 5); 
            modRecipe1.AddIngredient(575, 20);
            modRecipe1.AddTile(TileID.MythrilAnvil);
            modRecipe1.SetResult(this, 1);
            modRecipe1.AddRecipe();
        }		
    }
}
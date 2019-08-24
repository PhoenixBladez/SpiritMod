using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class SpiritHamaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Hamaxe");
		}


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 40;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;

            item.axe = 35;
            item.hammer = 85;

            item.damage = 35;
            item.knockBack = 5;

            item.useStyle = 1;
            item.useTime = 24;
            item.useAnimation = 24;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "SpiritBar", 15);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
            }
        }
    }
}

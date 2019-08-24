using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Tool
{
    public class StellarHamaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Hamaxe");
		}


        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 44;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;

            item.axe = 19;
            item.hammer = 85;

            item.damage = 11;
            item.knockBack = 6;

            item.useStyle = 1;
            item.useTime = 16;
            item.useAnimation = 24;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 14);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 133);
            }
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Tool
{
    public class StellarPickaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Pickaxe");
		}


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;

            item.pick = 180;

            item.damage = 21;
            item.knockBack = 3;

            item.useStyle = 1;
            item.useTime = 9;
            item.useAnimation = 23;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 15);
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
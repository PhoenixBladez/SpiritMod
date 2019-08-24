using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class FloranHamaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Hamaxe");
		}


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 40;
            item.value = Item.buyPrice(0, 0, 16, 0);
            item.rare = 2;

            item.axe = 12;
            item.hammer = 50;

            item.damage = 11;
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
            modRecipe.AddIngredient(null, "FloranBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 44);
            }
        }

    }
}

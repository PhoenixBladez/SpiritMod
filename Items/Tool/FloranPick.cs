using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class FloranPick : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Pickaxe");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.value = 1000;
            item.rare = 2;

            item.pick = 55;

            item.damage = 12;
            item.knockBack = 3;

            item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 25; 

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

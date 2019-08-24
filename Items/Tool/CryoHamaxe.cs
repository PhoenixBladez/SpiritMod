using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class CryoHamaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Hamaxe");
		}


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 40;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 3;

            item.axe = 25;
            item.hammer = 65;

            item.damage = 18;
            item.knockBack = 5;

            item.useStyle = 1;
            item.useTime = 27;
            item.useAnimation = 27;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "CryoliteBar", 12);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 180);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}

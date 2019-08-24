using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
    public class CryoPick : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Pickaxe");
		}


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 3;
            item.pick = 85;
            item.damage = 20;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = 16;
            item.useAnimation = 18;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 180);
                Main.dust[dust].noGravity = true;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"CryoliteBar", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
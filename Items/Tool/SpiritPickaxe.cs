using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Tool
{
    public class SpiritPickaxe : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Spirit Pickaxe");
        }


        public override void SetDefaults() {
            item.width = 36;
            item.height = 38;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;

            item.pick = 185;

            item.damage = 25;
            item.knockBack = 4f;

            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 9;
            item.useAnimation = 23;

            item.melee = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 18);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if(Main.rand.Next(5) == 0) {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
            }
        }
    }
}

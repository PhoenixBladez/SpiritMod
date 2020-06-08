using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class MoonStone : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Azure Gem");
            Tooltip.SetDefault("'Holds a far away power'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 3));
        }


        public override void SetDefaults() {
            item.width = 24;
            item.height = 26;
            item.value = 1000;
            item.rare = 4;
            item.scale = .8f;
            item.maxStack = 999;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            scale *= .6f;
        }
        public override Color? GetAlpha(Color lightColor) {
            return new Color(200, 200, 200, 100);
        }
    }
}
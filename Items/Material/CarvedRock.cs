using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace SpiritMod.Items.Material
{
    public class CarvedRock : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Slagstone");
            Tooltip.SetDefault("'A seething piece of hardened magma'");
        }


        public override void SetDefaults() {
            item.width = 22;
            item.height = 22;
            item.value = 800;
            item.rare = 3;

            item.maxStack = 999;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Lighting.AddLight(item.position, 0.4f, .12f, .036f);
        }
    }
}

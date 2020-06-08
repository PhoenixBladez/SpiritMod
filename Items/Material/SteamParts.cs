using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace SpiritMod.Items.Material
{
    public class SteamParts : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Starplate Fragments");
            Tooltip.SetDefault("'Powered by celestial energy'");
        }


        public override void SetDefaults() {
            item.width = 42;
            item.height = 24;
            item.value = 800;
            item.rare = 3;

            item.maxStack = 999;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Material/SteamParts_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}

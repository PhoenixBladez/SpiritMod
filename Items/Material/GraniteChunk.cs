using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class GraniteChunk : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Granite Chunk");
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .4f, .28f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Material/GraniteChunk_Glow"),
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
        }////

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.value = 5000;

            item.maxStack = 999;
            item.rare = 2;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.useAnimation = 15;


            item.autoReuse = true;
            item.consumable = true;

            item.createTile = ModContent.TileType<GraniteOre>();
        }
    }
}
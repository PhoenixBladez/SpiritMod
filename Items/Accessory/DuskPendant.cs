using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class DuskPendant : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Dusk Pendant");
            Tooltip.SetDefault("13% increased magic and ranged critical strike chance at night");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Accessory/DuskPendant_Glow");
        }



        public override void SetDefaults() {
            item.width = 24;
            item.height = 28;
            item.rare = 4;
            item.value = 80000;
            item.expert = true;
            item.melee = true;
            item.accessory = true;

            item.knockBack = 9f;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Accessory/DuskPendant_Glow"),
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
        public override void UpdateAccessory(Player player, bool hideVisual) {
            if(!Main.dayTime) {
                player.rangedCrit += 13;
                player.magicCrit += 13;
            }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
    public class CryoPick : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Cryolite Pickaxe");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Tool/CryoPick_Glow");
        }


        public override void SetDefaults() {
            item.width = 36;
            item.height = 36;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 3;
            item.pick = 85;
            item.damage = 20;
            item.knockBack = 2;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 16;
            item.useAnimation = 18;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 180);
                Main.dust[dust].noGravity = true;
            }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.06f, .16f, .22f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Tool/CryoPick_Glow"),
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
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
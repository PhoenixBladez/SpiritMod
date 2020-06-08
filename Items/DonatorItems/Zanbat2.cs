using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class Zanbat2 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Zanbat Sword");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/DonatorItems/Zanbat2_Glow");
        }


        int charger;
        public override void SetDefaults() {
            item.damage = 32;
            item.useTime = 13;
            item.useAnimation = 13;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = 25700;
            item.rare = 3;
            item.crit = 2;
            item.shootSpeed = 6f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Lighting.AddLight(item.position, 0.255f, .209f, .072f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/DonatorItems/Zanbat2_Glow"),
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
        public override void UseStyle(Player player) {
            float cosRot = (float)Math.Cos(player.itemRotation - 0.98f * player.direction * player.gravDir * 90f);
            float sinRot = (float)Math.Sin(player.itemRotation - 0.98f * player.direction * player.gravDir * 90f);
            for(int i = 0; i < 5; i++) {

                float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 8 + i; //length to base + arm displacement
                int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 133, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.White, .8f);
                Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(58, Main.LocalPlayer);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Zanbat1", 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

        }
    }
}
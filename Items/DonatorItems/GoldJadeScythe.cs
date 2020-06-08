using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.DonatorItems;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class GoldJadeScythe : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Crook of the Tormented");
            Tooltip.SetDefault("Occasionally spawns ornate scarabs to defend you upon hitting enemies");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/DonatorItems/GoldJadeScythe_Glow");
        }


        public override void SetDefaults() {
            item.damage = 42;
            item.melee = true;
            item.width = 50;
            item.height = 50;
            item.useTime = 19;
            item.useAnimation = 19;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.useTurn = true;
            item.crit = 9;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Lighting.AddLight(item.position, 0.255f, .509f, .072f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/DonatorItems/GoldJadeScythe_Glow"),
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
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            if(Main.rand.Next(8) == 1) {
                Vector2 velocity = new Vector2(player.direction, 0) * 4f;
                int proj = Terraria.Projectile.NewProjectile(player.Center.X, player.position.Y + player.height + -35, velocity.X, velocity.Y, ModContent.ProjectileType<JadeScarab>(), item.damage / 2, item.owner, 0, 0f);
                Main.projectile[proj].friendly = true;
                Main.projectile[proj].hostile = false;
            }
        }

        public override void UseStyle(Player player) {
            float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
            float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
            for(int i = 0; i < 1; i++) {
                float length = (item.width * 1.2f - i * item.width / 9) * item.scale - 4; //length to base + arm displacement
                int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 107, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, .8f);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Chitin", 12);
            recipe.AddIngredient(ItemID.Emerald, 4);
            recipe.AddRecipeGroup("GoldBars", 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
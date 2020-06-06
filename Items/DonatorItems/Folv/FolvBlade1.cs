using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    public class FolvBlade1 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Lost Blade");
			Tooltip.SetDefault("Replenishes mana upon hitting enemies");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/DonatorItems/Folv/FolvBlade1_Glow");
        }


        public override void SetDefaults()
        {
            item.damage = 14;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 23;
            item.useAnimation = 23;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.autoReuse = false;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.crit = 6;
        }
        public override void UseStyle(Player player)
        {
            float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
            float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
            for (int i = 0; i < 12; i++)
            {
                float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 2 + i; //length to base + arm displacement
                int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 187, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, .8f);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = true;
            }
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.statMana += 4;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.06f, .16f, .22f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/DonatorItems/Folv/FolvBlade1_Glow"),
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
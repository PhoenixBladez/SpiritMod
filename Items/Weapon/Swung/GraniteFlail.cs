using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Weapon.Swung
{
    public class GraniteFlail : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Colonnade");
			Tooltip.SetDefault("Killing enemies with this weapon causes them to explode into damaging energy wisps");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/GraniteFlail_Glow");
        }


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = Item.sellPrice(0, 1, 43, 0);
            item.rare = 2;
            item.damage = 25;
            item.knockBack = 8;
            item.useStyle = 5;
            item.useTime = item.useAnimation = 38;   
            item.melee = true;
            item.noMelee = true;
            item.channel = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("GraniteMaceProj");
            item.shootSpeed = 10.25f;
            item.UseSound = SoundID.Item1;   
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.08f, .12f, .52f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Swung/GraniteFlail_Glow"),
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
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
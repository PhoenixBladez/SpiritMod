using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class WayfinderTorch : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Light");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Thrown/WayfinderTorch_Glow");
        }


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 32;
            item.height = 32;           
            item.shoot = ModContent.ProjectileType<WayfinderTorch>();
            item.useAnimation = 21;
            item.useTime = 21;
            item.mana = 4;
            item.shootSpeed = 12f;
            item.damage = 43;
            item.knockBack = 1f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 50);
            item.rare = 4;
            item.magic = true;
            item.autoReuse = true;
        }
        public override void HoldItem(Player player)
        {
            Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
            Lighting.AddLight(position, .3f, .2f, .6f);
        }
        public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
        {
            dryTorch = true;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Thrown/WayfinderTorch_Glow"),
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
            recipe.AddIngredient(null, "MoonStone", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 55);
            recipe.AddRecipe();
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
	public class FolvMissile4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Ancient Staff");
			Tooltip.SetDefault("Shoots out bursts of Arcane Energy");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/DonatorItems/Folv/FolvMissile4_Glow");
        }


		public override void SetDefaults()
		{
			item.damage = 85;
			item.magic = true;
			item.mana = 15;
			item.width = 54;
			item.height = 54;
			item.useTime = 19;
			item.useAnimation = 19;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true; 
			item.knockBack = 5;
			item.value = 95400;
			item.rare = 9;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("FolvBolt3");
			item.shootSpeed = 8f;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Lighting.AddLight(item.position, 0.46f, .16f, .22f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/DonatorItems/Folv/FolvMissile4_Glow"),
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
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 3)) * 45f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int I = 0; I < 3; I++)
            {
                Projectile.NewProjectile(position.X, position.Y + 6, speedX + ((float)Main.rand.Next(-250, 250) / 100), speedY + ((float)Main.rand.Next(-250, 250) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            for (float num2 = 0.0f; (double)num2 < 10; ++num2)
            {
                int dustIndex = Dust.NewDust(position, 2, 2, 272, 0f, 0f, 0, default(Color), 1f);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity = Vector2.Normalize(position.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FolvMissile3", 1);
            recipe.AddRecipeGroup("CelestialFragment", 6);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
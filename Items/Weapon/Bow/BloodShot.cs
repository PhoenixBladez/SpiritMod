using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class BloodShot : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodshot Longbow");
			Tooltip.SetDefault("Arrows shot inflict 'Blood Corruption'");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Bow/BloodShot_Glow");
        }



        public override void SetDefaults()
        {
            item.damage = 19;
            item.noMelee = true;
            item.ranged = true;
            item.width = 24;
            item.height = 46;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 22, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 5f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int p = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Main.projectile[p].GetGlobalProjectile<SpiritGlobalProjectile>().shotFromBloodshot = true;
            return false;
        }
    	public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
            Lighting.AddLight(item.position, 0.46f, .07f, .12f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                mod.GetTexture("Items/Weapon/Bow/BloodShot_Glow"),
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
            recipe.AddIngredient(null, "BloodFire", 13);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}

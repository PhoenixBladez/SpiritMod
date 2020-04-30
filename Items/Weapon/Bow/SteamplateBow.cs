using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using SpiritMod.Projectiles;

namespace SpiritMod.Items.Weapon.Bow
{
    public class SteamplateBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starcharger");
			Tooltip.SetDefault("Shoots oppositely charged arrows that stick to enemies and tiles\nOppositely charged arrows explode upon touching each other");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Bow/SteamplateBow_Glow");
		}


        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.ranged = true;
            item.width = 28;
            item.height = 36;
            item.useTime = 27;
            item.useAnimation = 27;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1;
            item.rare = 3;
            item.UseSound = SoundID.Item5;
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 7f;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3, 0);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI) 	
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Bow/SteamplateBow_Glow"),
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
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (player.altFunctionUse == 2)
			{
				int positive = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("NegativeArrow"), damage, knockBack, player.whoAmI);
			}
			else
			{
				int negative = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PositiveArrow"), damage, knockBack, player.whoAmI);
			}
            return false; 
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SteamParts", 4);
            recipe.AddIngredient(null, "CosmiliteShard", 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
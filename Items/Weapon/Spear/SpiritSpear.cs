using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
	public class SpiritSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Spear");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Spear/SpiritSpear_Glow");
		}


		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 56;
			item.height = 56;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.noMelee = true;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 6f;
			item.knockBack = 7f;
			item.damage = 60;
			item.value = Item.sellPrice(0, 1, 15, 0);
			item.rare = 5;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<SpiritSpearProjectile>();
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Spear/SpiritSpear_Glow"),
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
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<SoulShred>(), 7);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}

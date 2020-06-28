using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
	public class Zanbat1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zanbat Sword");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/DonatorItems/Zanbat1_Glow");
		}
		public override void SetDefaults()
		{
			item.damage = 22;
			item.useTime = 15;
			item.useAnimation = 15;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.value = 25700;
			item.rare = ItemRarityID.Green;
			item.shootSpeed = 6f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.useTurn = true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.255f, .209f, .072f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/DonatorItems/Zanbat1_Glow"),
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
			recipe.AddIngredient(ModContent.ItemType<GoldSword>(), 1);
			recipe.AddIngredient(ItemID.PlatinumBroadsword, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<PlatinumSword>(), 1);
			recipe1.AddIngredient(ItemID.GoldBroadsword, 1);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();

		}
	}
}
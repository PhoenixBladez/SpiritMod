using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SeraphSet
{
	public class WayfinderTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraph's Light");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/SeraphSet/WayfinderTorch_Glow");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Shuriken);
			Item.width = 32;
			Item.height = 32;
			Item.shoot = ModContent.ProjectileType<Projectiles.Thrown.WayfinderTorch>();
			Item.useAnimation = 21;
			Item.useTime = 21;
			Item.mana = 4;
			Item.shootSpeed = 12f;
			Item.damage = 43;
			Item.knockBack = 1f;
			Item.value = Terraria.Item.buyPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.LightRed;
			Item.DamageType = DamageClass.Magic;
			Item.autoReuse = true;
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
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/SeraphSet/WayfinderTorch_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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
			Recipe recipe = CreateRecipe(55);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
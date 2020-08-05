using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
	public class GraniteBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Boltcaster");
			Tooltip.SetDefault("Converts wooden arrows into Unstable Bolts which stick to hit enemies\nKilling stuck enemies causes them to explode into damaging energy wisps");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Bow/GraniteBow_Glow");
		}
		public override void SetDefaults()
		{
			item.damage = 33;
			item.noMelee = true;
			item.ranged = true;
			item.width = 54;
			item.height = 20;
			item.useTime = 39;
			item.useAnimation = 39;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 4.5f;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shootSpeed = 14f;

		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .12f, .52f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Bow/GraniteBow_Glow"),
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
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) {
				type = ModContent.ProjectileType<GraniteRepeaterArrow>();
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
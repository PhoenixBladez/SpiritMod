using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class OrionPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion's Quickdraw");
			Tooltip.SetDefault("Converts regular bullets into Orion Bullets\nOrion Bullets leave lingering stars in their wake\n'Historically accurate'");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/StarplateDrops/OrionPistol_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 28;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 23;
			item.useAnimation = 23;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item41;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<OrionBullet>();
			item.shootSpeed = 6f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<OrionBullet>();

			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/StarplateDrops/OrionPistol_Glow"),
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
			recipe.AddIngredient(ItemID.FlintlockPistol, 1);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
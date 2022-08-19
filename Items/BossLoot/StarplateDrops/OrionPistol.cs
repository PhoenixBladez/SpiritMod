using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.StarplateDrops
{
	public class OrionPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion's Quickdraw");
			Tooltip.SetDefault("Converts regular bullets into Orion Bullets\nOrion Bullets leave lingering stars in their wake\n'Historically accurate'");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/BossLoot/StarplateDrops/OrionPistol_Glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<OrionBullet>();
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<OrionBullet>();

			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/BossLoot/StarplateDrops/OrionPistol_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.FlintlockPistol, 1);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.GunsMisc.Scattergun
{
	public class Scattergun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scattergun");
			Tooltip.SetDefault("Converts regular bullets into neon pellets");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GunsMisc/Scattergun/Scattergun_Glow");

		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/GunsMisc/Scattergun/Scattergun_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item36;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<ScattergunPellet>();
			Item.shootSpeed = 3.2f;
			Item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            SoundEngine.PlaySound(SoundID.Item12, player.Center);
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}

            if (type == ProjectileID.Bullet)
                type = ModContent.ProjectileType<ScattergunPellet>();

            for (int I = 0; I < 3; I++)
                Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-250, 250) / 150), velocity.Y + ((float)Main.rand.Next(-250, 250) / 100), type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.SpaceJunkItem>(), 35);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
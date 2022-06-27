using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.Starflux
{
	public class Conflux : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starflux");
			Tooltip.SetDefault("Converts regular bullets into bouncing stars");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/GunsMisc/Starflux/Conflux_Glow");

		}

		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.useTurn = false;
			Item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item12;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ConfluxPellet>();
			Item.shootSpeed = 5.2f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<ConfluxPellet>();
            }
            Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-250, 250) / 150), velocity.Y + ((float)Main.rand.Next(-100, 100) / 100), type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/GunsMisc/Starflux/Conflux_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Sets.GunsMisc.Scattergun.Scattergun>(), 1);
            recipe.AddIngredient(ItemID.SoulofLight, 11);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class EelRod : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eel Tail");
			Tooltip.SetDefault("Shoots a bolt of lightning that pauses occasionally and redirects to nearby foes\nSometimes electrifies hit foes");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Weapon/Magic/EelRod_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 50;
			Item.value = Item.buyPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Green;
			Item.damage = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.useTime = 29;
			Item.useAnimation = 29;
			Item.mana = 12;
			Item.knockBack = 3;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item21;
			Item.shoot = ModContent.ProjectileType<EelOrb>();
			Item.shootSpeed = 8f;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y - 3)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI, 0, velocity.ToRotation());
			return false;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Weapon/Magic/EelRod_Glow"),
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
	}
}

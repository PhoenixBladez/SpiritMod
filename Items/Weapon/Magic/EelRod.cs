using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using System.IO;
using Terraria;
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
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/EelRod_Glow");
		}

		public override void SetDefaults()
		{
			item.width = 48;
			item.height = 50;
			item.value = Item.buyPrice(0, 0, 30, 0);
			item.rare = ItemRarityID.Green;
			item.damage = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.useTime = 29;
			item.useAnimation = 29;
			item.mana = 12;
			item.knockBack = 3;
			item.magic = true;
			item.noMelee = true;
			item.UseSound = SoundID.Item21;
			item.shoot = ModContent.ProjectileType<EelOrb>();
			item.shootSpeed = 8f;
			item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 3)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, player.whoAmI, 0, new Vector2(speedX, speedY).ToRotation());
			return false;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/EelRod_Glow"),
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
	}
}

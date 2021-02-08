using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class FieryMagicLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Breath");
			Tooltip.SetDefault("Expels spurts of magical flame\nCritical hits shower enemies in damaging sparks");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Magic/FieryMagicLauncher_Glow");
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Magic/FieryMagicLauncher_Glow"),
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
		public override void SetDefaults()
		{
			item.damage = 26;
			item.magic = true;
			item.mana = 9;
			item.width = 32;
			item.height = 26;
			item.useTime = 4;
			item.useAnimation = 16;
			item.reuseDelay = 12;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<FieryFlareMagic>();
			item.shootSpeed = 6f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.wet) {
				return false;
			}
			Vector2 vel = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.Pi / 18);
			player.itemRotation = MathHelper.WrapAngle(vel.ToRotation() - ((player.direction < 0) ? MathHelper.Pi : 0));
			Vector2 muzzleOffset = Vector2.Normalize(vel) * 5f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			for (int j = 0; j < 6; j++) {
				int dust = Dust.NewDust(position, 0, 0, 6);
				Main.dust[dust].velocity = vel.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(1.3f, 2.2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(1.6f, 2f);
			}
			Projectile.NewProjectile(position + new Vector2(-8, 8), vel, type, damage, knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}
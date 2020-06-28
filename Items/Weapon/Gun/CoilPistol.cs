using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class CoilPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coil Pistol");
			Tooltip.SetDefault("Shoots out electrified bullets that chain from enemy to enemy");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Gun/CoilPistol_Glow");
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .4f, .28f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Gun/CoilPistol_Glow"),
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
		}////

		public override void SetDefaults()
		{
			item.damage = 14;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 13;
			item.useAnimation = 13;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 0, 22, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item11;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("CoilBullet1");
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = mod.ProjectileType("CoilBullet1");
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			if(Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
			return true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TechDrive>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
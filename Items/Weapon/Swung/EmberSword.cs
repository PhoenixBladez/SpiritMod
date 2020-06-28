using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Sword;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
	public class EmberSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ember Blade");
			Tooltip.SetDefault("Shoots out a wave of fire that slowly loses velocity");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/EmberSword_Glow");
		}


		public override void SetDefaults()
		{
			item.damage = 50;
			item.useTime = 29;
			item.useAnimation = 29;
			item.melee = true;
			item.width = 38;
			item.height = 38;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 5;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.shootSpeed = 12;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.useTurn = true;
			item.crit = 12;
			item.shoot = ModContent.ProjectileType<EmberSwordProj>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int I = 0; I < 1; I++) {
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<EmberSwordProj>(), 40, knockBack, player.whoAmI);
			}
			return false;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.4f, .26f, .12f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Swung/EmberSword_Glow"),
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
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 12);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}

using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class DesertTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Khamsin");
			Tooltip.SetDefault("Creats a violent sandnado from the player\nEnemies caught by the tornado are lifted upwards");
		}

		public override void SetDefaults()
		{
			Item.damage = 38;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 15;
			Item.mana = 15;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 3;
			Item.value = 50000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item34;
			Item.autoReuse = true;
			Item.shootSpeed = 1;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<SandWall>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			speedX = 0;
			speedY = -0.25f;
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SandWall>(), damage, knockback, player.whoAmI, speedX, speedY);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SandWall2>(), damage, knockback, player.whoAmI, speedX, speedY);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.TitaniumBar, 4);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

			recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.AdamantiteBar, 4);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}

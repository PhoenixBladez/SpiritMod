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
			item.damage = 38;
			item.noMelee = true;
			item.magic = true;
			item.width = 28;
			item.height = 30;
			item.useTime = 15;
			item.mana = 15;
			item.useAnimation = 60;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 3;
			item.value = 50000;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item34;
			item.autoReuse = true;
			item.shootSpeed = 1;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<SandWall>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX = 0;
			speedY = -0.25f;
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SandWall>(), damage, knockBack, player.whoAmI, speedX, speedY);
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SandWall2>(), damage, knockBack, player.whoAmI, speedX, speedY);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 4);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AdamantiteBar, 4);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Kunai_Throwing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kunai");
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 9;
			Item.height = 15;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.noMelee = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shoot = Mod.Find<ModProjectile>("Kunai_Throwing").Type;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 7.5f;
			Item.damage = 12;
			Item.knockBack = 1.5f;
			Item.value = Terraria.Item.sellPrice(0, 0, 1, 0);
			Item.crit = 8;
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.Silk, 1);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
			recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.Silk, 1);
			recipe.AddIngredient(ItemID.LeadBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 direction = velocity;

			Projectile.NewProjectile(source, position, direction.RotatedBy(-0.2f), Mod.Find<ModProjectile>("Kunai_Throwing").Type, damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, position, direction, Mod.Find<ModProjectile>("Kunai_Throwing").Type, damage, knockback, player.whoAmI, 0f, 0f);
			Projectile.NewProjectile(source, position, direction.RotatedBy(0.2f), Mod.Find<ModProjectile>("Kunai_Throwing").Type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}
	}
}

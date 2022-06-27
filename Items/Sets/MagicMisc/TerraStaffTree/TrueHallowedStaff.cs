using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class TrueHallowedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Hallowed Staff");
			Tooltip.SetDefault("Shoots out multiple swords with different effects.");
		}


		public override void SetDefaults()
		{
			Item.damage = 64;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 11;
			Item.width = 70;
			Item.height = 70;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = 120000;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<TrueHallowedStaffProj>();
			Item.shootSpeed = 16f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddIngredient(ModContent.ItemType<HallowedStaff>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;

				for (int i = 0; i < 3; ++i) {
					int p = Projectile.NewProjectile(source, mouse.X + Main.rand.Next(-80, 80), mouse.Y - 50 + Main.rand.Next(-10, 10), 0, Main.rand.Next(2, 4), type, damage, knockback, player.whoAmI);
				}
			}
			return false;
		}

	}
}

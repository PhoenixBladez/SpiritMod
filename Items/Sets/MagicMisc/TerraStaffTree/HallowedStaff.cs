using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.TerraStaffTree
{
	public class HallowedStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Staff");
			Tooltip.SetDefault("Rains down a barrage of magical swords");
		}


		public override void SetDefaults()
		{
			Item.damage = 51;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 9;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = 20000;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HallowedStaffProj>();
			Item.shootSpeed = 3f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;

				for (int i = 0; i < 1; ++i) {
					int p = Projectile.NewProjectile(source, mouse.X + Main.rand.Next(-80, 80), mouse.Y - 50 + Main.rand.Next(-10, 10), 0, Main.rand.Next(3, 6), type, damage, knockback, player.whoAmI);
					if (Main.rand.NextBool(2)) {
						int p1 = Projectile.NewProjectile(source, mouse.X + Main.rand.Next(-80, 80), mouse.Y - 50 + Main.rand.Next(-10, 10), 0, Main.rand.Next(3, 6), type, damage, knockback, player.whoAmI);
					}
				}
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.HallowedBar, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}

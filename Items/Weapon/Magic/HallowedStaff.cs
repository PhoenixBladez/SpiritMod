using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
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
			item.damage = 51;
			item.magic = true;
			item.mana = 9;
			item.width = 48;
			item.height = 48;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = 20000;
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<HallowedStaffProj>();
			item.shootSpeed = 3f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(Main.myPlayer == player.whoAmI) {
				Vector2 mouse = Main.MouseWorld;

				for(int i = 0; i < 1; ++i) {
					int p = Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), mouse.Y - 50 + Main.rand.Next(-10, 10), 0, Main.rand.Next(3, 6), type, damage, knockBack, player.whoAmI);
					if(Main.rand.Next(2) == 0) {
						int p1 = Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), mouse.Y - 50 + Main.rand.Next(-10, 10), 0, Main.rand.Next(3, 6), type, damage, knockBack, player.whoAmI);
					}
				}

			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}

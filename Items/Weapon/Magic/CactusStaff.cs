using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class CactusStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Staff");
			Tooltip.SetDefault("Shoots two cactus needles at foes\nThese pins stick to enemies and poison them");
		}


		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 7;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 11;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = 200;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 8);
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CactusProj>();
			Item.shootSpeed = 8f;
			Item.autoReuse = false;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			for (int k = 0; k < 15; k++) {
				Vector2 offset = mouse - player.Center;
				offset.Normalize();
				if (speedX > 0) {
					offset = offset.RotatedBy(-0.1f);
				}
				else {
					offset = offset.RotatedBy(0.1f);
				}
				offset *= 58f;
				int dust = Dust.NewDust(player.Center + offset, 1, 1, DustID.JungleGrass);
				Main.dust[dust].noGravity = true;
				float dustSpeed = Main.rand.Next(9) / 5;
				switch (Main.rand.Next(2)) {
					case 0:
						Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(1.57f);
						break;
					case 1:
						Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(-1.57f);
						break;
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.Cactus, 22);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}

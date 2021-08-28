using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves
{
	public class OrichalcumStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Staff");
			Tooltip.SetDefault("Summons homing orichalcum blooms at the cursor positon");
		}


		public override void SetDefaults()
		{
			item.damage = 35;
			item.magic = true;
			item.mana = 11;
			item.width = 40;
			item.height = 40;
			item.useTime = 43;
			item.useAnimation = 43;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<OrichalcumStaffProj>();
			item.shootSpeed = 1f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float[] scanarray = new float[3];
			float dist = player.Distance(Main.MouseWorld);
			Collision.LaserScan(player.Center, player.DirectionTo(Main.MouseWorld), 0, dist, scanarray);
			dist = 0;
			foreach (float array in scanarray) {
				dist += array / (scanarray.Length);
			}
			Vector2 spawnpos = player.Center + player.DirectionTo(Main.MouseWorld) * dist;
			Projectile p = Projectile.NewProjectileDirect(spawnpos, Vector2.Zero, type, damage, knockBack, player.whoAmI);
			p.netUpdate = true;
			for (int k = 0; k < 30; k++) {
				Vector2 offset = player.DirectionTo(Main.MouseWorld);
				offset.Normalize();
				if (speedX > 0) {
					offset = offset.RotatedBy(-0.1f);
				}
				else {
					offset = offset.RotatedBy(0.1f);
				}
				offset *= 58f;
				int dust = Dust.NewDust(player.Center + offset, 1, 1, DustID.PinkFlame);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 1.5f;
				float dustSpeed = Main.rand.Next(23) / 5;
				switch (Main.rand.Next(3)) {
					case 0:
						Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(1.57f);
						break;
					case 1:
						Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed);
						break;
					case 2:
						Main.dust[dust].velocity = new Vector2(speedX * dustSpeed, speedY * dustSpeed).RotatedBy(-1.57f);
						break;
				}
			}

			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.HardmodeOreStaves
{
	public class CobaltStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Staff");
			Tooltip.SetDefault("Summons magnetized cobalt shards at the cursor");
		}


		public override void SetDefaults()
		{
			item.damage = 39;
			item.magic = true;
			item.mana = 7;
			item.width = 40;
			item.height = 40;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CobaltStaffProj>();
			item.shootSpeed = 20f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int shardType;
			shardType = Main.rand.Next(new int[] { ModContent.ProjectileType<CobaltStaffProj>(), ModContent.ProjectileType<CobaltStaffProj1>() });
			float[] scanarray = new float[3];
			float dist = player.Distance(Main.MouseWorld);
			Collision.LaserScan(player.Center, player.DirectionTo(Main.MouseWorld), 0, dist, scanarray);
			dist = 0;
			foreach(float array in scanarray) {
				dist += array / (scanarray.Length);
			}
			Vector2 spawnpos = player.Center + player.DirectionTo(Main.MouseWorld) * dist;
			Projectile p = Projectile.NewProjectileDirect(spawnpos + Main.rand.NextVector2Square(-20, 20), Main.rand.NextVector2Circular(10, 10), shardType, damage, knockBack, player.whoAmI);
			p.scale = Main.rand.NextFloat(.4f, 1.1f);
			p.netUpdate = true;
			DustHelper.DrawDiamond(spawnpos, 48, 1.5f, 1.2f, 1f);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}

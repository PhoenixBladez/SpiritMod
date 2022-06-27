using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
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
			Item.damage = 39;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 7;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item43;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CobaltStaffProj>();
			Item.shootSpeed = 20f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int shardType;
			shardType = Main.rand.Next(new int[] { ModContent.ProjectileType<CobaltStaffProj>(), ModContent.ProjectileType<CobaltStaffProj1>() });

			//set the spawn position to the first solid tile in the direction of the cursor, or the cursor, whichever is closer
			float[] scanarray = new float[3];
			float dist = player.Distance(Main.MouseWorld); //set the maximum distance to the distance between the player and the cursor
			Collision.LaserScan(player.Center, player.DirectionTo(Main.MouseWorld), 0, dist, scanarray); //scan to find the first solid tile in the direction of the cursor
			dist = 0;
			foreach(float array in scanarray) //make the distance the average of the samples
				dist += array / (scanarray.Length);

			Vector2 spawnpos = player.Center + player.DirectionTo(Main.MouseWorld) * dist;

			Projectile p = Projectile.NewProjectileDirect(source, spawnpos + Main.rand.NextVector2Square(-20, 20), Main.rand.NextVector2Circular(10, 10), shardType, damage, knockback, player.whoAmI);
			p.scale = Main.rand.NextFloat(.4f, 1.1f);
			p.netUpdate = true;
			DustHelper.DrawDiamond(spawnpos, 48, 1.5f, 1.2f, 1f);
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.CobaltBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Halloween.SpookySet
{
	public class HauntingClaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunting Claw");
			Tooltip.SetDefault("Shoots a cluster of spooky fire.");
		}


		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 25;
			Item.width = 34;
			Item.height = 34;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ProjectileID.GreekFire2;
			Item.shootSpeed = 11f;
		}

		public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num)
		{
			var posArray = new Vector2[num];
			float spread = (float)(angle * 0.0174532925);
			float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
			double baseAngle = System.Math.Atan2(speedX, speedY);
			double randomAngle;
			for (int i = 0; i < num; ++i) {
				randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
				posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
			}
			return (Vector2[])posArray;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2[] speeds = randomSpread(speedX, speedY, 8, 3);
			for (int i = 0; i < 3; ++i) {
				int newProj = Projectile.NewProjectile(position.X, position.Y, speeds[i].X, speeds[i].Y, type, damage, knockback, player.whoAmI);
				Main.projectile[newProj].hostile = false;
				Main.projectile[newProj].friendly = true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.SpookyWood, 14);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
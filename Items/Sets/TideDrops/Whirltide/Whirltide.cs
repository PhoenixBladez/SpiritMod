using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.Whirltide
{
	public class Whirltide : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Whirltide");
			Tooltip.SetDefault("Sprouts tides from the ground below");
		}

		public override void SetDefaults()
		{

			Item.damage = 25;
			Item.noMelee = true;
			Item.noUseGraphic = false;
			Item.DamageType = DamageClass.Magic;
			Item.width = 30;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 12;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Whirltide_Bullet>();
			Item.knockBack = 10f;
			Item.shootSpeed = 7f;
			Item.staff[Item.type] = true;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 50);
			Item.useTurn = true;
			Item.mana = 5;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<Whirltide_Water_Explosion>()] < 1;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(null, "TribalScale", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.direction == 1)
				velocity.X = 6f;
			else
				velocity.X = -6f;

			velocity.Y = 0f;
		}
	}
}

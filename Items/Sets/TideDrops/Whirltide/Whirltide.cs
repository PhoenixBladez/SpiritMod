using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.Whirltide
{
	public class Whirltide : ModItem
	{
		public override void SetDefaults()
		{

			item.damage = 25;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 30;
			item.height = 30;
			item.useTime = 25;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.shoot = mod.ProjectileType("Whirltide_Bullet");
			item.knockBack = 10f;
			item.shootSpeed = 7f;
			Item.staff[item.type] = true;
			item.autoReuse = true;
			item.rare = 2;
			item.value = Item.sellPrice(silver: 50);
			item.useTurn = true;
			item.mana = 5;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[mod.ProjectileType("Whirltide_Water_Explosion")] < 1;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Whirltide");
			Tooltip.SetDefault("Sprouts tides from the ground below");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TribalScale", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.direction == 1)
			{
				speedX = 6f;
				speedY = 0f;
			}
			else
			{
				speedX = -6f;
				speedY = 0f;
			}
			return true;
		}
	}
}

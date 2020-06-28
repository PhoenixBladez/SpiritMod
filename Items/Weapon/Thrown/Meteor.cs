using SpiritMod.Projectiles.Thrown;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class Meteor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor");
			Tooltip.SetDefault("Explodes on contact with foes");
		}


		public override void SetDefaults()
		{
			item.damage = 21;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 22;
			item.height = 22;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<MeteorProjectile>();
			item.useAnimation = 29;
			item.consumable = true;
			item.maxStack = 999;
			item.useTime = 29;
			item.shootSpeed = 13.0f;
			item.knockBack = 3f;
			item.value = Terraria.Item.sellPrice(0, 0, 0, 90);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = false;
			item.maxStack = 999;
			item.consumable = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 30);
			recipe.AddRecipe();
		}
	}
}

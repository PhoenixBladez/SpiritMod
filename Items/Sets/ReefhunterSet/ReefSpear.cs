using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class ReefSpear : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Reef Trident");

		public override void SetDefaults()
		{
			item.damage = 18;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.knockBack = 2f;
			item.shootSpeed = 9;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.ranged = true;
			item.channel = false;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(gold: 2);
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<ReefSpearProjectile>();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		//public override void AddRecipes()
		//{
		//	var recipe = new ModRecipe(mod);
		//	recipe.AddIngredient(ModContent.ItemType<GranitechMaterial>(), 12);
		//	recipe.AddTile(TileID.MythrilAnvil);
		//	recipe.SetResult(this, 1);
		//	recipe.AddRecipe();
		//}
	}
}
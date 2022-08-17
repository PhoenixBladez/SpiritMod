using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Consumable.Food
{
	public class AcornCake : FoodItem
	{
		internal override Point Size => new(22, 22);
		public override void StaticDefaults() => Tooltip.SetDefault("'If you're really hungry, go for it'");

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Poisoned, 480);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ItemID.Acorn, 5);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.Register();
		}
	}
}

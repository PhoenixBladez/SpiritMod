
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class AcornCake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acorn Cake");
			Tooltip.SetDefault("'If you're really hungry, go for it'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 14800;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;
		}

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

using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
	public class SpiritPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Potion");
			Tooltip.SetDefault("Increases damage and critical strike chance by 5% \nGetting hurt occasionally spawns a damaging bolt to chase enemies");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 30;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = ModContent.BuffType<SpiritBuff>();
			item.buffTime = 7200;

			item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SpiritKoi>(), 1);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

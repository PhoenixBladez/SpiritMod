
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Neck)]
	public class HuntingNecklace : SpiritAccessory
	{
		public override string SetDisplayName => "Briarhunt Charm";
		public override string SetTooltip
			=> "Increases armor penetration by 3\n"
			+ "Melee attacks occasionally strike enemies twice\n"
			+ "5% increased melee speed and slightly increased life regeneration while standing on grass\n"
			+ "4% increased critical strike chance\n"
			+ "Allows for increased night vision in the Briar";
		public override int AllCrit => 4;
		public override int ArmorPenetration => 3;
		public override List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>() {
			new FloranCharmEffect(),
			new CleftHornEffect(),
			new ReachBroochEffect()
		};
		public override List<int> MutualExclusives => new List<int>() {
			ModContent.ItemType<FloranCharm>(),
			ModContent.ItemType<CleftHorn>(),
			ModContent.ItemType<ReachBrooch>()
		};

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 42;
			item.value = Item.buyPrice(0, 1, 20, 0);
			item.rare = ItemRarityID.LightRed;
			item.defense = 3;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ReachBrooch>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CleftHorn>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FloranCharm>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

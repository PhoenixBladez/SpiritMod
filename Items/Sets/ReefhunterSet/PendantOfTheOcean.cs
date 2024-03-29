using SpiritMod.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.GlobalClasses.Players;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class PendantOfTheOcean : AccessoryItem, ITimerItem
	{
		public override bool Autoload(ref string name)
		{
			DoubleTapPlayer.OnDoubleTap += DoubleTapUp;
			return base.Autoload(ref name);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pendant of the Ocean");
			Tooltip.SetDefault("Double tap {0} to gain unhindered movement underwater for a short time");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string down = Main.ReversedUpDownArmorSetBonuses ? "UP" : "DOWN";

			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0")
					line.text = line.text.Replace("{0}", down);
			}
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 48;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 0, 80, 0);
			item.melee = true;
			item.accessory = true;
			item.knockBack = 5f;
		}

		private void DoubleTapUp(Player player, int keyDir)
		{
			if (keyDir == 0 && player.HasAccessory<PendantOfTheOcean>() && player.ItemTimer<PendantOfTheOcean>() <= 0)
			{
				player.AddBuff(ModContent.BuffType<Buffs.EmpoweredSwim>(), 60 * 10);
				player.SetItemTimer<PendantOfTheOcean>(60 * 45);
			}
		}

		public int TimerCount() => 1;

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 8);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 4);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 8);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 4);
			recipe.AddIngredient(ItemID.LeadBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}

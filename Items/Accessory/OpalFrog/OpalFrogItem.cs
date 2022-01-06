using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Accessory.OpalFrog
{
	public class OpalFrogItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Opal Frog");
			Tooltip.SetDefault("15% increased hook speed" +
				"\nAutomatically detatch from hooks upon reaching the end of the hook" +
				"\nDisable accessory visibility to disable auto-unhooking");
		}

		public override void SetDefaults()
		{
			item.Size = new Vector2(40, 34);
			item.value = Item.sellPrice(gold: 8);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OpalFrogPlayer modPlayer = player.GetModPlayer<OpalFrogPlayer>();
			modPlayer.HookStat += .15f;
			if (!hideVisual)
				modPlayer.AutoUnhook = true;
		}
	}
}
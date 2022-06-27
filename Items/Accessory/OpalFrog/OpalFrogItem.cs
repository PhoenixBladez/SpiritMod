using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			Item.Size = new Vector2(40, 34);
			Item.value = Item.sellPrice(gold: 8);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = 550;
			Item.rare = ItemRarityID.LightRed;
			Item.createTile = ModContent.TileType<OpalFrog_Tile>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OpalFrogPlayer modPlayer = player.GetModPlayer<OpalFrogPlayer>();
			modPlayer.HookStat += 0.25f;
			if (!hideVisual)
				modPlayer.AutoUnhook = true;
		}
	}
}
using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class GoldLumothItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Lumoth");
			Tooltip.SetDefault("'It glimmers beautifully'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 32;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<GoldLumoth>());
			return true;
		}
	}
}

using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class CleftItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cleft Hopper");
			Tooltip.SetDefault("'Its shell is quite sturdy'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 20;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = true;

		}
		public override bool UseItem(Player player)
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Cleft>());
			return true;
		}
	}
}

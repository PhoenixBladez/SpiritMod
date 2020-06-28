
using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class LuvdiscItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ardorfish");
			Tooltip.SetDefault("'It exudes affection'");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 22;
			item.rare = 1;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 0, 4, 0);
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = true;

		}
		public override bool UseItem(Player player)
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Luvdisc>());
			return true;
		}

	}
}

using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class GaleSnailItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Gale Snail");

		public override void SetDefaults()
		{
			item.width = item.height = 20;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 20;
            item.bait = 35;
			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = true;
		}

		public override bool UseItem(Player player)
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<GaleSnail>());
			return true;
		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.GamblerChestLoot.GamblerChestNPCs;

namespace SpiritMod.Items.Sets.GamblerChestLoot
{
	public class GoldChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Coffer");
			Tooltip.SetDefault("Right click to open\n'May contain a fortune'");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 30;
			item.autoReuse = true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			Main.npc[NPC.NewNPC((int)player.Center.X + player.direction * 30, (int)player.Center.Y, ModContent.NPCType<GoldChestBottom>(), 0)].netUpdate = true;
		}
	}
}

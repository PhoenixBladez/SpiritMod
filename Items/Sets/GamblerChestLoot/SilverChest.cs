using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.GamblerChestLoot.GamblerChestNPCs;

namespace SpiritMod.Items.Sets.GamblerChestLoot
{
	public class SilverChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silver Lockbox");
			Tooltip.SetDefault("Right click to open\n'May contain a fortune'");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.buyPrice(silver: 50);
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 30;
			Item.autoReuse = true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			Main.npc[NPC.NewNPC(player.GetSource_OpenItem("RightClick"), (int)player.Center.X + player.direction * 30, (int)player.Center.Y, ModContent.NPCType<SilverChestBottom>(), 0)].netUpdate = true;
		}
	}
}

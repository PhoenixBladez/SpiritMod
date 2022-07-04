using SpiritMod.NPCs.Reach;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class BriarmothItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Briarmoth");

		public override void SetDefaults()
		{
			Item.width = Item.height = 20;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;
            Item.bait = 24;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Briarmoth>());
			return null;
		}
	}
}

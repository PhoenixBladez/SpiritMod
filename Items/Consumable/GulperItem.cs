
using SpiritMod.NPCs.Critters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class GulperItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gulper");
			Tooltip.SetDefault("'Quite the perky lil' fella'");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<Gulper>());
			return null;
		}
	}
}

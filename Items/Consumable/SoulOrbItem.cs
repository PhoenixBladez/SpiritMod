using SpiritMod.NPCs.Spirit;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class SoulOrbItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Orb");
			Tooltip.SetDefault("'Legend says touching it gives good luck'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;

			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;

		}
		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<SoulOrb>());
			return true;
		}

	}
}

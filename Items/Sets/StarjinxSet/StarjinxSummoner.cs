using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.StarjinxEvent;

namespace SpiritMod.Items.Sets.StarjinxSet
{
	public class StarjinxSummoner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx Summoner");
			Tooltip.SetDefault("Placeholder! Summons the Starjinx Meteor.");
		}

		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = item.height = 16;
			item.useTime = item.useAnimation = 20;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item43;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<StarjinxMeteorite>());

		public override bool UseItem(Player player)
		{
			Main.NewText("An enchanted comet has appeared in the asteroid field!", 252, 150, 255);
			NPC.NewNPC(MyWorld.asteroidSide == 1 ? 1800 : Main.maxTilesX - 1800, 1800, ModContent.NPCType<StarjinxMeteorite>());
			StarjinxEventWorld.SpawnedStarjinx = true;
			return true;
		}
	}
}

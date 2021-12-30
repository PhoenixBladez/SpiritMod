using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class OliveBranch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Olive Branch");
			Tooltip.SetDefault("Greatly reduces enemy spawns");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.LightRed;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;
			item.buffType = ModContent.BuffType<OliveBranchBuff>();
			item.buffTime = 10800;
			item.UseSound = SoundID.Item29;
		}
	}
}

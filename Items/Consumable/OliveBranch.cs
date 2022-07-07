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
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.buffType = ModContent.BuffType<OliveBranchBuff>();
			Item.buffTime = 10800;
			Item.UseSound = SoundID.Item29;
		}
	}
}

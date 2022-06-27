using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ToolsMisc.BrilliantHarvester
{
	public class GemPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brilliant Harvester");
			Tooltip.SetDefault("Mining stone may also yield gems and ores\nCan mine Demonite and Crimtane");
		}


		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = 12000;
			Item.rare = ItemRarityID.Blue;

			Item.pick = 55;

			Item.damage = 8;
			Item.knockBack = 3;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 16;
			Item.useAnimation = 20;

			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;

			Item.UseSound = SoundID.Item1;
		}
		public override void HoldItem(Player player)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.gemPickaxe = true;
		}
	}
}

using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ToolsMisc.Evergreen
{
	public class UnfellerOfEvergreens : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evergreen");
			Tooltip.SetDefault("Plants saplings when it chops trees down");
		}

		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 46;
			item.value = 10000;
			item.rare = ItemRarityID.Orange;
			item.axe = 12;
			item.damage = 18;
			item.knockBack = 6;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 22;
			item.useAnimation = 22;
			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
		}
	}
}
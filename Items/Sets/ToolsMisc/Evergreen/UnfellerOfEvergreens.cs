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
			Item.width = 46;
			Item.height = 46;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.axe = 12;
			Item.damage = 18;
			Item.knockBack = 6;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}
	}
}
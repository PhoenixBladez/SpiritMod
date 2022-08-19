using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.InfernonDrops
{
	public class HellsGaze : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Lash");
			Tooltip.SetDefault("Nearby enemies are engulfed in flames\n6% increased critical strike chance\nYou emit a fiery glow");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(gold: 8);
			Item.expert = true;
			Item.DamageType = DamageClass.Melee;
			Item.accessory = true;

			Item.knockBack = 9f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().HellGaze = true;
			player.GetCritChance(DamageClass.Generic) += 6;
		}
	}
}

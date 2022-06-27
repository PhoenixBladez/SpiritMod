using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class IchorPendant : SpiritAccessory
	{
		public override string SetDisplayName => "Ichor Pendant";
		public override string SetTooltip => "6% increased melee damage\nMelee hits occasionally inflict Ichor";
		public override float MeleeDamage => 0.06f;
		public override List<SpiritPlayerEffect> AccessoryEffects => new List<SpiritPlayerEffect>() {
			new IchorPendantEffect()
		};

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(gold: 3);
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
	}

	public class IchorPendantEffect : SpiritPlayerEffect
	{
		public override void PlayerModifyHitNPC(Player player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			if (Main.rand.NextBool(10)) target.AddBuff(BuffID.Ichor, 180);
		}
	}
}

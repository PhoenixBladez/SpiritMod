using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items
{
	public abstract class SpiritItem : ModItem
	{
		public virtual string SetDisplayName => "";
		public virtual string SetTooltip => "";
		public virtual float DontConsumeAmmoChance => 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(SetDisplayName);
			Tooltip.SetDefault(SetTooltip);
		}

		public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > DontConsumeAmmoChance;
	}
}

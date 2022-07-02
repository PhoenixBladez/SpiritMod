using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class UnstableAffliction : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Affliction");
			Description.SetDefault("Reduces movement speed by 10%");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed -= .10f;
			player.moveSpeed -= .10f;
		}
	}
}
using SpiritMod.Buffs.Potion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class GBuff : GlobalBuff
	{
		public override void Update (int type, Player player, ref int buffIndex)
		{
			if (type == BuffID.OnFire && player.HasBuff(ModContent.BuffType<IceBerryBuff>()))
			{
				player.DelBuff(buffIndex);
			}
		}
	}
}

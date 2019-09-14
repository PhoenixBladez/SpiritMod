using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class AcidImbue : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Acid Imbue");
			Description.SetDefault("Throwing attacks occasionally inflict Acid Burn");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			modPlayer.acidImbue = true;
		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class DrainLife : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Drain Life");
            Description.SetDefault("Your life energy becomes theirs.");
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}
		int counter = 0;
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.lifeRegen -= 2;
			Player player = Main.player[npc.target];
			counter++;
			if (counter % 15 == 0)
			{
				player.HealEffect(1);
				player.statLife += 1;
			}
		}
	}
}

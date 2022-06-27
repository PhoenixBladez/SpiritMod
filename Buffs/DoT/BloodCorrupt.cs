using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.DoT
{
	public class BloodCorrupt : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Corruption");
			Description.SetDefault("Your blood burns...");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.friendly)
			{
				if (npc.lifeRegen > 0)
					npc.lifeRegen = 0;
				npc.lifeRegen -= 4;

				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood);
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0)
				player.lifeRegen = 0;
			player.lifeRegen -= 4;

			Dust.NewDust(player.position, player.width, player.height, DustID.Blood);
		}
	}
}
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class FesteringWounds : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Festering Wounds");
            Description.SetDefault("Deals more damage if the player is under half health");
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
                {
                    npc.lifeRegen = 0;
                }
                if (npc.life <= npc.lifeMax / 2)
                {
                    npc.lifeRegen -= 5;
                }
                else
                {
                    npc.lifeRegen -= 3;
                }
                if (Main.rand.Next(3) == 0)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 167);
                    Main.dust[d].velocity.X *= 0f;
                    Main.dust[d].velocity.Y *= 0.5f;
                }
            }
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.lifeRegen > 0)
            {
                player.lifeRegen = 0;
            }
            if (player.statLife <= player.statLife / 2)
            {
                player.lifeRegen -= 5;
            }
            else
            {
                player.lifeRegen -= 3;
            }
            if (Main.rand.Next(3) == 0)
            {
                int d = Dust.NewDust(player.position, player.width, player.height, 167);
                Main.dust[d].velocity.X *= 0f;
                Main.dust[d].velocity.Y *= 0.5f;
            }
        }
	}
}

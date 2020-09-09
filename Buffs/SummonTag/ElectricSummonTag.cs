using SpiritMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.SummonTag
{
	public class ElectricSummonTag : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Summon Tag");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<GNPC>().summonTag4 = true;
            for (int npcFinder = 0; npcFinder < 200; ++npcFinder)
            {
				int distance = (int)Vector2.Distance(npc.Center, Main.npc[npcFinder].Center);

                if (!Main.npc[npcFinder].townNPC && !Main.npc[npcFinder].friendly && distance < 82 && distance > npc.width/3)
                {
                    if (Main.rand.NextBool(65))
                    {
                        for (int k = 0; k < 15; k++)
                        {
                            Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, Main.rand.NextFloat(.4f, .8f));
                            d.noGravity = true;
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            DustHelper.DrawElectricity(npc.Center, Main.npc[npcFinder].Center, 226, 0.3f);
                        }
                        Main.npc[npcFinder].StrikeNPC(6, 1f, 0, false);
                    }
                }

            }
        }

	}
}
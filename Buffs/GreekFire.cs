using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.Buffs
{
	public class GreekFire : ModBuff
	{
		private readonly Color Blue = new Color(0, 114, 201);
		private readonly Color Orange = new Color(255, 194, 89);

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Greek Fire");
			Description.SetDefault("Set ablaze by the Gods themselves");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.friendly) {
				if (npc.lifeRegen > 0) {
					npc.lifeRegen = 0;
				}

				npc.lifeRegen -= 12;
				npc.defense = (int)(npc.defense * 0.95f);

				if (Main.rand.NextBool(3))
					ParticleHandler.SpawnParticle(new FireParticle(new Vector2(npc.Center.X + Main.rand.Next(-10, 10), npc.Center.Y + Main.rand.Next(-10, 10)), new Vector2(Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-4.5f, -2.5f)),
						Blue, Orange, Main.rand.NextFloat(0.25f, 0.75f), 30, delegate (Particle p)
						{
							p.Velocity *= 0.93f;
							p.Velocity.Y += .0125f;
						}));
			}
		}
	}
}

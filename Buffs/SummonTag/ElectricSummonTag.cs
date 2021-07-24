using SpiritMod.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Linq;
using SpiritMod.Projectiles.Summon;
using System;

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

		public override void Update(NPC npc, ref int buffIndex) => npc.GetGlobalNPC<GNPC>().summonTag = 4;
	}

	public class ElecricSummonTagGProj : GlobalProjectile
	{
		public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
		{
			bool summon = (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type] || ProjectileID.Sets.SentryShot[projectile.type] || projectile.sentry);
			if (summon && target.HasBuff(ModContent.BuffType<ElectricSummonTag>()) && projectile.type != ModContent.ProjectileType<ElectricGunProjectile>())
			{
				float maxdist = 300;
				var potentialtargets = Main.npc.Where(x => x.CanBeChasedBy(this) && x.active && x != null && x.Distance(target.Center) < maxdist && x != target);
				if (potentialtargets.Any())
				{
					NPC finaltarget = null;
					foreach (NPC potentialtarget in potentialtargets)
					{
						if (potentialtarget.Distance(target.Center) < maxdist)
						{
							maxdist = potentialtarget.Distance(target.Center);
							finaltarget = potentialtarget;
						}
					}
					for (int k = 0; k < 15; k++)
					{
						Dust d = Dust.NewDustPerfect(target.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, Main.rand.NextFloat(.4f, .8f));
						d.noGravity = true;
					}
					for (int i = 0; i < 3; i++)
					{
						DustHelper.DrawElectricity(target.Center, finaltarget.Center, 226, 0.3f);
					}
					if (finaltarget != null)
						finaltarget.StrikeNPC((int)(Math.Min(damage / 3, 12) * Main.rand.NextFloat(0.8f, 1.2f)), 1f, 0, false);
				}
			}
		}
	}
}
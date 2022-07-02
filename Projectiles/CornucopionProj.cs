using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Projectiles.Magic;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;

namespace SpiritMod.Projectiles
{
	public class CornucopionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cornucop-ion");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 999999;
		}

		Vector2 direction = Vector2.Zero;
        int counter = 0;
        Vector2 holdOffset = new Vector2(0, -15);
        int chargeStacks = 0;
        public override bool PreAI()
		{
            Player player = Main.player[Projectile.owner];
            if (player.channel)
            {
				if (counter == 1)
                {
                    direction = Main.MouseWorld - (player.Center - new Vector2(4, 4));
                    direction.Normalize();
                    direction *= 5f;
                }
                player.itemTime = 5;
                player.itemAnimation = 5;
                Projectile.position = player.Center + holdOffset;
                player.velocity.X *= 0.97f;
                counter++;

				if (counter % 45 == 0)
                {
                    chargeStacks++;
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
					for (int i = 0; i < 14; i++)
                        DoDustEffect(player.Center, 40f);
                }

				if (counter > 240)
                    player.AddBuff(BuffID.Electrified, (chargeStacks - 5) * 45);
            }
            else
            {
                if (chargeStacks > 0)
                {
                    for (int i = 0; i < chargeStacks; ++i)
                    {
                        for (int npcFinder = 0; npcFinder < 200; ++npcFinder)
                        {
                            int distance = (int)Vector2.Distance(player.Center, Main.npc[npcFinder].Center);
                            if (!Main.npc[npcFinder].friendly && !Main.npc[npcFinder].townNPC && Main.npc[npcFinder].active && distance < 800 && Main.npc[npcFinder].CanBeChasedBy(this))
                            {
                                int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Main.npc[npcFinder].Center, Vector2.Zero, ModContent.ProjectileType<MoonThunder>(), 20, 8);
                                SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Thunder2"), Main.projectile[p].Center);
                                Main.npc[npcFinder].StrikeNPC(Projectile.damage + (2 * chargeStacks), 12, 0, false);
                                SpiritMod.tremorTime = 18;
                                Main.projectile[p].friendly = true;
                                Main.projectile[p].hostile = false;
                                break;
                            }
                        }
                    }
                }
                
                Projectile.active = false;
            }
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            return true;
		}
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, DustID.Electric);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .3f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
    }
}

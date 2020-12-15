using Microsoft.Xna.Framework;
using System;
using Terraria;
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
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = false;
            projectile.alpha = 255;
            projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.timeLeft = 999999;
		}

		bool firing = false;
		Vector2 direction = Vector2.Zero;
        int counter = 0;
        Vector2 holdOffset = new Vector2(0, -15);
        int chargeStacks = 0;
        public override bool PreAI()
		{
            Player player = Main.player[projectile.owner];
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
                projectile.position = player.Center + holdOffset;
                player.velocity.X *= 0.97f;
                counter++;
				if (counter % 45 == 0)
                {
                    chargeStacks++;
                    Main.PlaySound(25, (int)projectile.position.X, (int)projectile.position.Y);
                }
				if (counter > 240)
                {
                    player.AddBuff(BuffID.Electrified, (chargeStacks - 5) * 45);
                }
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
                            if (!Main.npc[npcFinder].friendly && !Main.npc[npcFinder].townNPC && Main.npc[npcFinder].active && distance < 800)
                            {
                                int p = Projectile.NewProjectile(Main.npc[npcFinder].Center, Vector2.Zero, mod.ProjectileType("MoonThunder"), 20, 8);
                                Main.PlaySound(SoundLoader.customSoundType, Main.projectile[p].Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/Thunder2"));
                                Main.npc[npcFinder].StrikeNPC(projectile.damage + (2 * chargeStacks), 12, 0, false);
                                SpiritMod.tremorTime = 18;
                                Main.projectile[p].friendly = true;
                                Main.projectile[p].hostile = false;
                                break;
                            }

                        }
                    }
                }
                
                projectile.active = false;
            }
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            return true;
		}
        private void DoDustEffect(Vector2 position, float distance, float minSpeed = 2f, float maxSpeed = 3f, object follow = null)
        {
            float angle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
            Vector2 vec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 vel = vec * Main.rand.NextFloat(minSpeed, maxSpeed);

            int dust = Dust.NewDust(position - vec * distance, 0, 0, 226);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale *= .3f;
            Main.dust[dust].velocity = vel;
            Main.dust[dust].customData = follow;
        }
    }
}

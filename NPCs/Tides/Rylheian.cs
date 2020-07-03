
using Microsoft.Xna.Framework;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Tide;
using System;

namespace SpiritMod.NPCs.Tides
{
	[AutoloadBossHead]
	public class Rylheian : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("R'lyehian");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 66;
			npc.height = 88;
			npc.damage = 42;
			npc.lifeMax = 2800;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}
		int counter;
		int phase = 0; //0 = charging, 1 = special attack
		float angle = 0;
		public override void AI()
		{
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			counter++;
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, 173);
			npc.spriteDirection = npc.direction;
			if (counter % 400 == 100)
			{
				int distance = 500;
				bool teleported = false;
				while (!teleported)
				{
					int angleT = Main.rand.Next(360);
					double anglex = Math.Sin(angleT * (Math.PI / 180));
					double angley = Math.Cos(angleT * (Math.PI / 180));
					npc.position.X = player.Center.X + (int)(distance * anglex);
					npc.position.Y = player.Center.Y + (int)(distance * angley);
					if(Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active()) {
						npc.alpha = 255;
					} else {
						teleported = true;
						npc.alpha = 0;
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
					}
				}
			}
			if (counter % 400 == 200)
			{
				phase = Main.rand.Next(2) + 1;
				angle = Main.rand.NextFloat(0.785f);
			}
			
			#region phase 1
			if (counter % 400 == 316 && phase == 1)
			{
				for (angle = 0; angle < 6.29; angle+= 0.785f)
				{
					Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 90f;
					int laser = Terraria.Projectile.NewProjectile(npc.Center.X+ offset.X, npc.Center.Y + offset.Y, 0, 0, ModContent.ProjectileType<RyBolt>(), npc.damage, 0);
					Projectile p = Main.projectile[laser];
					Vector2 direction = Main.player[npc.target].Center - p.Center;
					direction.Normalize();
					direction *= 15;
					p.velocity = direction;
				}
				phase = 0;
			}
			if (phase == 1)
			{
				if (counter % 12 == 0 && counter % 400 < 300)
				{
					Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 90f;
					DustHelper.DrawTriangle(npc.Center + offset, 173, 4);
					angle += 0.785f;
				}
			}
			#endregion
			#region phase 2
			if (phase == 2)
			{
				if (counter % 15 == 10)
				{
					int laser = Terraria.Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 10, ModContent.ProjectileType<RyTentacle>(), npc.damage, 0);
				}
				if (counter % 400 == 390)
				{
					phase = 0;
				}
			}
			#endregion
		}
		public override void FindFrame(int frameHeight)
		{
				npc.frameCounter += 0.2f;
				npc.frameCounter %= Main.npcFrameCount[npc.type];
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(3))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TomeOfRylien>());
            }
            if (Main.rand.NextBool(3))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TentacleChain>());
            }
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			for(int i = 0; i < 10; i++)
				;
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tentacle"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/TentacleHead"), 1f);
				if(TideWorld.TheTide) {
					TideWorld.TidePoints += 10;
				}
			}
		}
	}
}

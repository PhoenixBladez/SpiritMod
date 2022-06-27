using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DungeonCube
{

	public abstract class BaseDungeonCube : ModNPC
	{
		private bool xacc = true;
		private bool yacc = false;
		private bool xchase = true;
		private int timer = 0;
		private bool ychase = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Cube");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.width = 36;
			NPC.height = 32;
			NPC.noGravity = true;
			NPC.lifeMax = 150;
			NPC.defense = 10;
			NPC.damage = 32;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath44;

			NPC.knockBackResist = 0f;
			NPC.value = 500f;

			NPC.netAlways = true;
			NPC.chaseable = true;
			NPC.lavaImmune = true;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Cursed, 60, true);

		public override bool PreAI()
		{
			timer++;
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (NPC.velocity == Vector2.Zero) {
				xchase = true;
				xacc = true;
				yacc = false;
				ychase = false;
				if (player.position.X > NPC.position.X) {
					NPC.velocity.X = 0.1f;
				}
				else {
					NPC.velocity.X = -0.1f;
				}
			}
			if (xchase) {
				NPC.velocity.Y = 0;
				if (Math.Abs(NPC.position.X - player.position.X) > 48 && xacc && timer < 200) {
					if (xacc && Math.Abs(NPC.velocity.X) < 8) {
						NPC.velocity.X *= 1.06f;
					}
				}
				else {
					xacc = false;
					timer = 0;
					NPC.velocity.X *= 0.94f;
					if (Math.Abs(NPC.velocity.X) < 0.1f) {
						yacc = true;
						ychase = true;

						if (player.position.Y > NPC.position.Y) {
							NPC.velocity.Y = 0.1f;
						}
						else {
							NPC.velocity.Y = -0.1f;
						}
						xchase = false;
					}
				}
				if (NPC.velocity.X == 0) {
					yacc = true;
					ychase = true;

					if (player.position.Y > NPC.position.Y) {
						NPC.velocity.Y = 0.1f;
					}
					else {
						NPC.velocity.Y = -0.1f;
					}
					timer = 0;
					xchase = false;
				}
			}

			if (ychase) {
				NPC.velocity.X = 0;
				if (Math.Abs(NPC.position.Y - player.position.Y) > 48 && yacc && timer < 200) {
					if (yacc && Math.Abs(NPC.velocity.Y) < 8) {
						NPC.velocity.Y *= 1.06f;
					}
				}
				else {
					yacc = false;
					timer = 0;
					NPC.velocity.Y *= 0.94f;
					if (Math.Abs(NPC.velocity.Y) < 0.1f) {
						xacc = true;
						xchase = true;

						if (player.position.X > NPC.position.X) {
							NPC.velocity.X = 0.1f;
						}
						else {
							NPC.velocity.X = -0.1f;
						}
						ychase = false;
					}
				}
				if (NPC.velocity.Y == 0) {
					xacc = true;
					xchase = true;

					if (player.position.X > NPC.position.X) {
						NPC.velocity.X = 0.1f;
					}
					else {
						NPC.velocity.X = -0.1f;
					}
					timer = 0;
					ychase = false;
				}
			}
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= 6;
			if (xchase) 
				NPC.frame.Y = (int)(MathHelper.Clamp(Math.Abs(NPC.velocity.X * 3), 0, 6)) * frameHeight;

			else
				NPC.frame.Y = (int)(MathHelper.Clamp(Math.Abs(NPC.velocity.Y * 3), 0, 6)) * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.SpawnTileType == TileID.BlueDungeonBrick) 
				return spawnInfo.Player.ZoneDungeon ? 0.04f : 0f;

			return 0f;
		}

		public virtual int TileDropType => 134;
		public override void OnKill()
		{
			if (Main.rand.Next(10) == 1) Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.GoldenKey);
			if (Main.rand.Next(75) == 1) Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Nazar);
			if (Main.rand.Next(100) == 1) Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.TallyCounter);

			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, TileDropType, Main.rand.Next(4));
		}

		public virtual string GoreColor => "Blue";
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) 
				for (int i = 1; i <= 4; i++) 
					Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/DungeonCube" + GoreColor + "Gore" + i.ToString()).Type);
		}
	}
}
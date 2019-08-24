using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.NPCs.Spirit
{
	public class GhastlyBeing : ModNPC
	{
		private bool circling;
		int startdist = 0;
		Vector2 target = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghastly Being");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 56;
			npc.height = 42;
			npc.damage = 20;
			npc.defense = 20;
			npc.lifeMax = 300;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 60f;
			npc.knockBackResist = .45f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		//public override void HitEffect(int hitDirection, double damage)
		//{
		//	if (npc.life <= 0)
		//	{
		//		Gore.NewGore(npc.position, npc.velocity, 13);
		//		Gore.NewGore(npc.position, npc.velocity, 12);
		//		Gore.NewGore(npc.position, npc.velocity, 11);
		//	}
		//}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.spawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.spawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
			{
				int[] TileArray2 = { mod.TileType("SpiritDirt"), mod.TileType("SpiritStone"), mod.TileType("Spiritsand"), mod.TileType("SpiritGrass"), mod.TileType("SpiritIce"), };
				return TileArray2.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && NPC.downedMechBossAny && spawnInfo.spawnTileY > (Main.rockLayer + 50) ? 3.08f : 0f;
			}
			return 0f;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			Vector2 delta = player.position - npc.position;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.12f, 0.12f, 1f);
			//64 pixel radius

			if (Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) <= 250f && circling == false) //circle starting
			{
				npc.ai[1] = 0; //reset rotation
				npc.aiStyle = -1;
				circling = true; //launch circle action into effect
			}
			if (Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) > 250f && circling == false) //normal AI
			{
				npc.aiStyle = 22;
				aiType = NPCID.Wraith;
			}
			if (Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) > 500f && circling == true) //stop circling
			{
				circling = false;
				npc.velocity = Vector2.Zero;
			}
			if (circling == true)
			{
				double deg = (double)npc.ai[1]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
				double rad = deg * (Math.PI / 180); //Convert degrees to radians
				double dist = 250; //Distance away from the player

				/*Position the npc based on where the player is, the Sin/Cos of the angle times the /
                /distance for the desired distance away from the player minus the npc's width   /
                /and height divided by two so the center of the npc is at the right place.     */
				target.X = player.Center.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
				target.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;

				//Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
				npc.ai[1] += 1f;
				Vector2 Vel = target - npc.Center;
				Vel.Normalize();
				Vel *= 4f;
				npc.velocity = Vel;
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(5) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SoulShred"), Main.rand.Next(1) + 1);
		}
	}
}
